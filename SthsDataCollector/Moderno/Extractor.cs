using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace BojoBox.SthsDataCollector.Moderno
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
                season.Teams.Add(team);

                var zipped = teamSection.skaterGroup.sectionA.Zip(teamSection.skaterGroup.sectionB, (a, b) => new { a, b });
                foreach (var row in zipped)
                    AddSkaterRows(team, row.a, row.b);

                foreach (var row in teamSection.goalieRows)
                    AddGoalieRows(team, row);
            }

            return season;
        }

        private void AddGoalieRows(TeamInfo team, HtmlNode row)
        {
            var playerRow = new GoalieRow() { TeamAcronym = team.Acronym };
            var rowValues = row.Descendants("td").Select(a => a.InnerText).ToList();

            playerRow.Name = GetName(rowValues);
            playerRow.Stats = GetStats(rowValues, goalieSkipColumns);

            season.GoalieRows.Add(playerRow);
        }

        private void AddSkaterRows(TeamInfo team, HtmlNode rowA, HtmlNode rowB)
        {
            var playerRow = new SkaterRow() { TeamAcronym = team.Acronym };
            var rowValuesA = rowA.Descendants("td").Select(a => a.InnerText).ToList();
            var rowValuesB = rowB.Descendants("td").Select(b => b.InnerText).ToList();

            playerRow.Name = GetName(rowValuesA);
            playerRow.IsForward = rowValuesA.ElementAt(1).Trim().ToUpperInvariant() == "X";
            playerRow.IsDefense = rowValuesA.ElementAt(2).Trim().ToUpperInvariant() == "X";

            playerRow.Stats = new List<int>();
            playerRow.Stats.AddRange(GetStats(rowValuesA, skaterSkipColumnsA));
            playerRow.Stats.AddRange(GetStats(rowValuesB, skaterSkipColumnsB));

            //TODO: subtotals

            season.SkaterRows.Add(playerRow);
        }

        private static string GetName(IEnumerable<string> rowValuesA)
        {
            // TODO: Get Rid of a, c, r, and teams
            return rowValuesA.ElementAt(0).Trim();
        }

        private static TeamInfo GetTeam(TeamSection teamSection)
        {
            return new TeamInfo()
            {
                Name = teamSection.teamHeader.InnerText,
                Acronym = GetTeamAcronym(teamSection),
            };
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

        private SeasonData season;

        int[] skaterSkipColumnsA = { 0, 1, 2, 15, 18 };
        int[] skaterSkipColumnsB = { 0, 1, 2, 5, 11, 21, 22, 23 };
        int[] goalieSkipColumns = { 0, 5, 6, 15 };

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
