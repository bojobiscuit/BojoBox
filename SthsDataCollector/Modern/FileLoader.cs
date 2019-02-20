using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace BojoBox.SthsDataCollector.Modern
{
    public class FileLoader
    {
        public FileLoader(SeasonData season)
        {
            this.season = season;
        }

        public HtmlDocument DownloadFile(string urlTemplate, bool isLegacy)
        {
            HtmlDocument htmlDocument;

            var nameFormat = (season.IsPlayoffs) ?
                "{1} - PLF - Pro Team Scoring {0}.html" :
                "{1} - Pro Team Scoring {0}.html";

            var folderName = (isLegacy) ?
                @"LegacyFiles" :
                @"SeasonFiles";

            string seasonNumberText = (season.SeasonNumber < 10 ? "0" : "") + season.SeasonNumber.ToString();
            string fileName = string.Format(nameFormat, seasonNumberText, season.LeagueAcronym);
            string directory = Path.Combine(Environment.CurrentDirectory, folderName);
            string filePath = Path.Combine(directory, fileName);

            Directory.CreateDirectory(directory);
            if (!File.Exists(filePath))
            {
                var url = urlTemplate;
                url = url.Replace("{leagueLow}", season.LeagueAcronym.ToLowerInvariant());
                url = url.Replace("{leagueUp}", season.LeagueAcronym.ToUpperInvariant());
                url = url.Replace("{seasonNumber}", season.SeasonNumber.ToString());
                url = url.Replace("{playoffAcro}", season.IsPlayoffs ? "PLF-" : "");

                if (season.LeagueAcronym.ToLowerInvariant() == "smjhl")
                    url = url.Replace("{seasonType}", season.IsPlayoffs ? "Playoffs" : "Season");
                else
                    url = url.Replace("{seasonType}", season.IsPlayoffs ? "Playoff" : "Season");

                htmlDocument = new HtmlWeb().Load(url);

                var title = htmlDocument.DocumentNode.Descendants("title").FirstOrDefault();
                if (title == null || !title.InnerText.ToLowerInvariant().Contains("team scoring"))
                    throw new Exception("Page was not found online");

                htmlDocument.Save(filePath);
            }

            htmlDocument = new HtmlDocument();
            htmlDocument.Load(filePath);

            return htmlDocument;
        }

        private SeasonData season;
    }
}
