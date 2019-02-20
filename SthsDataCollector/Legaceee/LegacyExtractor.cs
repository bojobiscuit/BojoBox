using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using BojoBox.SthsDataCollector.Modern;
using SthsDataCollector.Legacy;

namespace BojoBox.SthsDataCollector.Legaceee
{
    public class LegacyExtractor
    {
        SeasonData season;
        List<TeamSection> teamSections;

        public LegacyExtractor(SeasonData season)
        {
            this.season = season;
        }

        public SeasonData Extract(HtmlDocument htmlDocument)
        {
            GetTeamSections(htmlDocument);

            foreach (var teamSection in teamSections)
            {
                int parenthesisIndex = teamSection.TeamInformationLine.IndexOf('(');
                TeamInfo team = new TeamInfo()
                {
                    Name = teamSection.TeamInformationLine.Substring(0, parenthesisIndex).Replace(".", "").Trim().SplitCamelCase(),
                    Acronym = teamSection.TeamInformationLine.GetAcronymns().First()
                };
                season.Teams.Add(team);

                ExtractSkatersFromSection(team, teamSection.SkaterInformationLines);
                ExtractGoaliesFromSection(team, teamSection.GoalieInformationLines);
            }

            return season;
        }

        private void GetTeamSections(HtmlDocument htmlDocument)
        {
            IEnumerable<string> teamSectionData = htmlDocument.DocumentNode.Descendants("pre").Select(a => a.InnerText);

            teamSections = new List<TeamSection>();
            foreach (var data in teamSectionData)
                teamSections.Add(new TeamSection(data));
        }

        private void ExtractSkatersFromSection(TeamInfo team, IList<string> skaterLines)
        {
            string previousName = "";
            foreach (string line in skaterLines)
            {
                var nameValue = line.Substring(0, 30).Replace("_", "").Trim();
                var nameAcronyms = nameValue.GetAcronymns();

                SkaterRow skaterRow = new SkaterRow()
                {
                    Name = GetName(nameValue),
                    TeamAcronym = Helper.GetTeamAcronym(nameAcronyms)
                };

                if (skaterRow.TeamAcronym == null)
                    skaterRow.TeamAcronym = team.Acronym;

                if (nameAcronyms.Contains("TOT"))
                    skaterRow.TeamAcronym = null;

                if (previousName == skaterRow.Name)
                    skaterRow.IsSubTotal = true;

                string rawStatsLine = line.Substring(30, line.Length - 30).Replace("%", "").Trim();
                string[] rawStats = rawStatsLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                skaterRow.Stats = GetStats(rawStats, skaterSkipColumns);
                skaterRow.Stats.Add(GetFaceoffsWon(rawStats, skaterRow.Stats.ElementAt(26)));

                season.SkaterRows.Add(skaterRow);
            }
        }

        private void ExtractGoaliesFromSection(TeamInfo team, IList<string> goalieLines)
        {
            string previousName = "";
            foreach (string line in goalieLines)
            {
                var nameValue = line.Substring(0, 30).Replace("_", "").Trim();
                var nameAcronyms = nameValue.GetAcronymns();

                GoalieRow goalieRow = new GoalieRow()
                {
                    Name = GetName(nameValue),
                    TeamAcronym = Helper.GetTeamAcronym(nameAcronyms)
                };

                if (goalieRow.TeamAcronym == null)
                    goalieRow.TeamAcronym = team.Acronym;

                if (nameAcronyms.Contains("TOT"))
                    goalieRow.TeamAcronym = null;

                if (previousName == goalieRow.Name)
                    goalieRow.IsSubTotal = true;

                string rawStatsLine = line.Substring(30, line.Length - 30).Replace("%", "").Trim();
                string[] rawStats = rawStatsLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                goalieRow.Stats = GetStats(rawStats, goalieSkipColumns);
                goalieRow.Stats.Add(GetPenaltyShotsSaved(rawStats, goalieRow.Stats.ElementAt(11)));

                season.GoalieRows.Add(goalieRow);
            }
        }

        private static int GetFaceoffsWon(IEnumerable<string> rowValues, int faceoffTotal)
        {
            var faceoffValue = rowValues.ElementAt(28).Replace("%", "");
            var faceoffDouble = double.Parse(faceoffValue);
            var faceoffsWon = Helper.GetPercentageAmount(faceoffDouble, faceoffTotal);
            return faceoffsWon;
        }

        private static int GetPenaltyShotsSaved(IEnumerable<string> rowValues, int penaltyShotsTotal)
        {
            var shotsValue = rowValues.ElementAt(14).Replace("%", "");
            var shotsDouble = double.Parse(shotsValue);
            var shotsStopped = (int)Math.Round(shotsDouble * penaltyShotsTotal);
            return shotsStopped;
        }

        private static string GetName(string nameValue)
        {
            var name = nameValue;
            name = name.RemoveAcronyms();
            name = name.Replace("_", "");
            name = name.Trim();
            return name;
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

        int[] skaterSkipColumns = { 12, 15, 28, 30, 31, 34, 40, 41, 42, 43, 44, 45, 46 };
        int[] goalieSkipColumns = { 4, 5, 11, 14, 18, 19, 20 };

    }
}
