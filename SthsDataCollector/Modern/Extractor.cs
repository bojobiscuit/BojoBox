using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace BojoBox.SthsDataCollector.Modern
{
    public class Extractor
    {
        public Extractor(SeasonData season)
        {
            this.season = season;
        }

        public SeasonData Extract(HtmlDocument seasonFile)
        {
            List<TeamSection> teamSections = GetTeamSections(seasonFile.DocumentNode);

            foreach (var teamSection in teamSections)
            {
                TeamInfo team = GetTeam(teamSection);

                var pairedSkaterRows = teamSection.skaterGroup.sectionA.Zip(teamSection.skaterGroup.sectionB, (a, b) => new { a, b });
                foreach (var row in pairedSkaterRows)
                    AddSkaterRows(team, row.a, row.b);

                foreach (var row in teamSection.goalieRows)
                    AddGoalieRows(team, row);
            }

            return season;
        }

        private TeamInfo GetTeam(TeamSection teamSection)
        {
            var team = new TeamInfo()
            {
                Name = teamSection.teamHeader.InnerText,
                Acronym = GetTeamAcronym(teamSection),
            };
            season.Teams.Add(team);
            return team;
        }

        private void AddGoalieRows(TeamInfo team, HtmlNode row)
        {
            var rowValues = row.Descendants("td").Select(a => a.InnerText).ToList();
            var nameValue = rowValues.ElementAt(0);
            var nameAcronyms = nameValue.GetAcronymns();
            var goalieRow = new GoalieRow()
            {
                Name = GetName(nameValue),
                TeamAcronym = Helper.GetTeamAcronym(nameAcronyms),
            };

            if (goalieRow.TeamAcronym == null)
                goalieRow.TeamAcronym = team.Acronym;

            if (nameAcronyms.Contains("TOT"))
                goalieRow.TeamAcronym = null;

            if (previousName == goalieRow.Name)
                goalieRow.IsSubTotal = true;

            goalieRow.Stats = GetStats(rowValues, goalieSkipColumns);
            goalieRow.Stats.Add(GetPenaltyShotsSaved(rowValues, goalieRow.Stats.ElementAt(12)));

            previousName = goalieRow.Name;
            season.GoalieRows.Add(goalieRow);
        }

        private void AddSkaterRows(TeamInfo team, HtmlNode rowA, HtmlNode rowB)
        {
            var rowValuesA = rowA.Descendants("td").Select(a => a.InnerText).ToList();
            var rowValuesB = rowB.Descendants("td").Select(b => b.InnerText).ToList();
            var nameValue = rowValuesA.ElementAt(0);
            var nameAcronyms = nameValue.GetAcronymns();
            var skaterRow = new SkaterRow()
            {
                Name = GetName(nameValue),
                TeamAcronym = Helper.GetTeamAcronym(nameAcronyms),
                IsForward = rowValuesA.ElementAt(1).Trim().ToUpperInvariant() == "X",
                IsDefense = rowValuesA.ElementAt(2).Trim().ToUpperInvariant() == "X",
            };

            if (skaterRow.TeamAcronym == null)
                skaterRow.TeamAcronym = team.Acronym;

            if (nameAcronyms.Contains("TOT"))
                skaterRow.TeamAcronym = null;

            if (previousName == skaterRow.Name)
                skaterRow.IsSubTotal = true;

            skaterRow.Stats.AddRange(GetStats(rowValuesA, skaterSkipColumnsA));
            skaterRow.Stats.AddRange(GetStats(rowValuesB, skaterSkipColumnsB));
            skaterRow.Stats.Add(GetFaceoffsWon(rowValuesB, skaterRow.Stats.ElementAt(26)));

            previousName = skaterRow.Name;
            season.SkaterRows.Add(skaterRow);
        }


        private static string GetName(string nameValue)
        {
            var name = nameValue;
            name = name.RemoveAcronyms();
            name = name.Replace("_", "");
            name = name.Trim();
            return name;
        }

        private static int GetPenaltyShotsSaved(List<string> rowValues, int penaltyShotsTotal)
        {
            var shotsValue = rowValues.ElementAt(15).Replace("%", "");
            var shotsDouble = double.Parse(shotsValue);
            var shotsStopped = (int)Math.Round(shotsDouble * penaltyShotsTotal);
            return shotsStopped;
        }

        private static int GetFaceoffsWon(List<string> rowValuesB, int faceoffTotal)
        {
            var faceoffValue = rowValuesB.ElementAt(5).Replace("%", "");
            var faceoffDouble = double.Parse(faceoffValue);
            var faceoffsWon = Helper.GetPercentageAmount(faceoffDouble, faceoffTotal);
            return faceoffsWon;
        }

        private static List<TeamSection> GetTeamSections(HtmlNode doc)
        {
            var teamHeaders = doc.Descendants("h1").Where(a => a.Attributes["class"].Value.Contains("TeamScoringPro_"));
            var skaterRowsA = GetTableSection(doc, "STHSScoring_PlayersTable1");
            var skaterRowsB = GetTableSection(doc, "STHSScoring_PlayersTable2");
            var goalieRows = GetTableSection(doc, "STHSScoring_GoaliesTable");

            int count = teamHeaders.Count();
            var teamSections = new List<TeamSection>();
            for (int i = 0; i < count; i++)
            {
                var skaterSection = new SkaterGroup()
                {
                    sectionA = skaterRowsA.ElementAt(i).Skip(1),
                    sectionB = skaterRowsB.ElementAt(i).Skip(1)
                };

                teamSections.Add(new TeamSection()
                {
                    teamHeader = teamHeaders.ElementAt(i),
                    skaterGroup = skaterSection,
                    goalieRows = goalieRows.ElementAt(i).Skip(1),
                });
            }

            return teamSections;
        }

        private static List<int> GetStats(IEnumerable<string> rowValues, int[] skipColumns)
        {
            List<int> stats = new List<int>();
            for (int i = 0; i < rowValues.Count(); i++)
            {
                if (skipColumns.Contains(i))
                    continue;

                stats.Add(int.Parse(rowValues.ElementAt(i)));
            }
            return stats;
        }

        private static string GetTeamAcronym(TeamSection teamSection)
        {
            string teamClass = teamSection.teamHeader.Attributes["class"].Value;
            string teamAcronym = teamClass.Substring(teamClass.Length - 3);
            return teamAcronym;
        }

        private static IEnumerable<IEnumerable<HtmlNode>> GetTableSection(HtmlNode doc, string className)
        {
            return doc.Descendants("table")
                .Where(a => a.HasClass(className))
                .Select(a => a.Descendants("tr"));
        }

        private readonly SeasonData season;
        private string previousName = "";

        int[] skaterSkipColumnsA = { 0, 1, 2, 15, 18 };
        int[] skaterSkipColumnsB = { 0, 1, 2, 5, 7, 8, 11, 17, 18, 19, 20, 21, 22, 23 };
        int[] goalieSkipColumns = { 0, 5, 6, 12, 15, 19, 20, 21 };

        private class TeamSection
        {
            public HtmlNode teamHeader;
            public SkaterGroup skaterGroup;
            public IEnumerable<HtmlNode> goalieRows;
        }

        private class SkaterGroup
        {
            public IEnumerable<HtmlNode> sectionA;
            public IEnumerable<HtmlNode> sectionB;
        }
    }
}
