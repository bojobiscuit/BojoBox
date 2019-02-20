using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using BojoBox.SthsDataCollector.Modern;
using BojoBox.SthsDataCollector.Model;

namespace BojoBox.SthsDataCollector.Legacy
{
    public class LegacyExtractor
    {
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
                TeamInfo team = GetTeamInfo(teamSection, parenthesisIndex);
                ExtractSkatersFromSection(team, teamSection.SkaterInformationLines);
                ExtractGoaliesFromSection(team, teamSection.GoalieInformationLines);
            }

            return season;
        }

        private void GetTeamSections(HtmlDocument htmlDocument)
        {
            IEnumerable<string> teamSectionData = htmlDocument.DocumentNode.Descendants("pre").Select(a => a.InnerText);

            teamSections = new List<LegacyTeamSection>();
            foreach (var data in teamSectionData)
                teamSections.Add(new LegacyTeamSection(data));
        }

        private TeamInfo GetTeamInfo(LegacyTeamSection teamSection, int parenthesisIndex)
        {
            TeamInfo team = new TeamInfo()
            {
                Name = teamSection.TeamInformationLine.Substring(0, parenthesisIndex).Replace(".", "").Trim().SplitCamelCase(),
                Acronym = teamSection.TeamInformationLine.GetAcronymns().First()
            };
            season.Teams.Add(team);
            return team;
        }

        private void ExtractSkatersFromSection(TeamInfo team, IList<string> skaterLines)
        {
            foreach (string line in skaterLines)
            {
                var nameValue = line.Substring(0, 30).Replace("_", "").Trim();
                var nameAcronyms = nameValue.GetAcronymns();

                SkaterRow skaterRow = new SkaterRow()
                {
                    Name = Helper.GetName(nameValue),
                    TeamAcronym = Helper.GetTeamAcronym(nameAcronyms)
                };

                Helper.GetPlayerTeam(team.Acronym, nameAcronyms, skaterRow);

                string rawStatsLine = line.Substring(30, line.Length - 30).Replace("%", "").Trim();
                string[] rawStats = rawStatsLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                skaterRow.Stats = Helper.GetStats(rawStats, skaterSkipColumns);
                skaterRow.Stats.Add(Helper.GetFaceoffsWon(rawStats[28], skaterRow.Stats.ElementAt(26)));

                season.SkaterRows.Add(skaterRow);
            }
        }

        private void ExtractGoaliesFromSection(TeamInfo team, IList<string> goalieLines)
        {
            foreach (string line in goalieLines)
            {
                var nameValue = line.Substring(0, 30).Replace("_", "").Trim();
                var nameAcronyms = nameValue.GetAcronymns();

                GoalieRow goalieRow = new GoalieRow()
                {
                    Name = Helper.GetName(nameValue),
                    TeamAcronym = Helper.GetTeamAcronym(nameAcronyms)
                };

                Helper.GetPlayerTeam(team.Acronym, nameAcronyms, goalieRow);

                string rawStatsLine = line.Substring(30, line.Length - 30).Replace("%", "").Trim();
                string[] rawStats = rawStatsLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                goalieRow.Stats = Helper.GetStats(rawStats, goalieSkipColumns);
                goalieRow.Stats.Add(Helper.GetPenaltyShotsSaved(rawStats[14], goalieRow.Stats.ElementAt(11)));

                season.GoalieRows.Add(goalieRow);
            }
        }

        private SeasonData season;
        private List<LegacyTeamSection> teamSections;

        int[] skaterSkipColumns = { 12, 15, 28, 30, 31, 34, 40, 41, 42, 43, 44, 45, 46 };
        int[] goalieSkipColumns = { 4, 5, 11, 14, 18, 19, 20 };
    }
}
