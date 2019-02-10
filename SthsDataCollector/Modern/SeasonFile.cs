using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using HtmlAgilityPack;

namespace SthsDataCollector.Modern
{
    internal class SeasonFile
    {
        public SeasonFile(string url, int seasonNumber, string leagueAcronym, bool isPlayoffs)
        {
            DownloadFile(url, seasonNumber, leagueAcronym, isPlayoffs);
        }

        public IEnumerable<TeamSection> GetTeamSections()
        {
            HtmlNode[] teamInfoHeaders = GetTeamInfoHeaders();
            HtmlNode[] playerInfoContainers = GetPlayerInfoContainers();
            HtmlNode[] tableSections = GetTableDatas();

            List<TeamSection> teamSections = new List<TeamSection>();
            if (playerInfoContainers.Any())
            {
                if (teamInfoHeaders.Count() != playerInfoContainers.Count())
                    throw new Exception("Error getting stat information on modern stats");

                for (int i = 0; i < teamInfoHeaders.Length; i++)
                    teamSections.Add(new TeamSection(teamInfoHeaders[i], playerInfoContainers[i]));
            }
            else
            {
                if (teamInfoHeaders.Count() * 3 != tableSections.Count())
                    throw new Exception("Error getting stat information on modern stats");

                for (int i = 0, j = i; i < teamInfoHeaders.Length; i++, j = i * 3)
                    teamSections.Add(new TeamSection(teamInfoHeaders[i], tableSections[j], tableSections[j + 1], tableSections[j + 2]));
            }


            return teamSections;
        }

        private void DownloadFile(string url, int seasonNumber, string leagueAcronym, bool isPlayoffs)
        {
            var nameFormat = (isPlayoffs) ?
                "{1} - PLF - Pro Team Scoring {0}.html" :
                "{1} - Pro Team Scoring {0}.html";

            string seasonNumberText = (seasonNumber < 10 ? "0" : "") + seasonNumber.ToString();
            string fileName = string.Format(nameFormat, seasonNumberText, leagueAcronym);
            string directory = Path.Combine(Environment.CurrentDirectory, @"SeasonFiles\");
            string filePath = Path.Combine(directory, fileName);

            Directory.CreateDirectory(directory);
            //if (!File.Exists(filePath))
            {
                _htmlDocument = new HtmlWeb().Load(url);
                _htmlDocument.Save(filePath);
            }

            _htmlDocument = new HtmlDocument();
            _htmlDocument.Load(filePath);
        }

        private HtmlNode[] GetPlayerInfoContainers()
        {
            return _htmlDocument.DocumentNode.Descendants("div").Where(a => a.Id.StartsWith("STHS_JS_Team_")).ToArray();
        }
        private HtmlNode[] GetTableDatas()
        {
            return _htmlDocument.DocumentNode.Descendants("table").Where(a => a.HasClass("basictablesorter")).ToArray();
        }

        private HtmlNode[] GetTeamInfoHeaders()
        {
            return _htmlDocument.DocumentNode.Descendants("h1").Where(a => a.Attributes.Contains("class") && a.Attributes["class"].Value.StartsWith("TeamScoringPro_")).ToArray();
        }

        private HtmlDocument _htmlDocument;
    }
}
