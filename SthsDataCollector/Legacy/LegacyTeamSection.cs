using System;
using System.Collections.Generic;
using System.Linq;

namespace BojoBox.SthsDataCollector.Legacy
{
    internal class LegacyTeamSection
    {
        public string TeamInformationLine { get; set; }
        public IList<string> SkaterInformationLines { get; set; }
        public IList<string> GoalieInformationLines { get; set; }

        public LegacyTeamSection(string teamSectionData)
        {
            teamSectionData = teamSectionData.Replace("\r", "").Trim();
            LineParseSection parseSection = LineParseSection.TeamInfo;
            IList<string> currentLineList = new List<string>();

            string[] lines = teamSectionData.Split('\n');
            foreach (var line in lines)
            {
                if (line.Length < 10) continue;
                if (line.StartsWith("---------")) continue;
                if (line.Contains("Player Name"))
                {
                    if (parseSection != LineParseSection.Skater)
                    {
                        TeamInformationLine = currentLineList.First();
                        currentLineList = new List<string>();
                        parseSection++;
                    }
                    continue;
                }
                if (line.Contains("Goalie Name"))
                {
                    if (parseSection != LineParseSection.Goalie)
                    {
                        SkaterInformationLines = currentLineList;
                        currentLineList = new List<string>();
                        parseSection++;
                    }
                    continue;
                }
                currentLineList.Add(line);
            }
            if (parseSection == LineParseSection.Goalie)
                GoalieInformationLines = currentLineList;
            else
                throw new Exception("Something went wrong parsing the legacy team sections");
        }

        private enum LineParseSection
        {
            TeamInfo,
            Skater,
            Goalie
        }
    }
}
