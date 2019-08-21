using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using BojoBox.SthsDataCollector.Model;
using System.Runtime.InteropServices;

namespace BojoBox.SthsDataCollector.Modern
{
    public class FileLoader
    {
        public FileLoader(SeasonData season)
        {
            this.season = season;
        }

        public HtmlDocument LoadFile()
        {
            HtmlDocument htmlDocument;

            var nameFormat = (season.IsPlayoffs) ?
                "{1} - PLF - Pro Team Scoring {0}.html" :
                "{1} - Pro Team Scoring {0}.html";

            var folderName = @"LegacyFiles";

            string seasonNumberText = (season.SeasonNumber < 10 ? "0" : "") + season.SeasonNumber.ToString();
            string fileName = string.Format(nameFormat, seasonNumberText, season.LeagueAcronym);

            string directory = Path.Combine(Environment.CurrentDirectory, folderName);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                directory = Path.Combine(Environment.CurrentDirectory, "bin/debug/netcoreapp2.2", folderName);

            string filePath = Path.Combine(directory, fileName);

            Directory.CreateDirectory(directory);
            if (!File.Exists(filePath))
            {
                return null;
            }

            htmlDocument = new HtmlDocument();
            htmlDocument.Load(filePath);

            return htmlDocument;
        }

        public HtmlDocument DownloadFile(string urlTemplate, bool isLegacy)
        {
            HtmlDocument htmlDocument;

            var nameFormat = (season.IsPlayoffs) ?
                "{1} - PLF - Pro Team Scoring {0}.html" :
                "{1} - Pro Team Scoring {0}.html";

            var folderName = "SeasonFiles";

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

                if (season.LeagueAcronym.ToLowerInvariant() == "shl")
                    url = url.Replace("{seasonType}", season.IsPlayoffs ? "Playoff" : "Season");
                if (season.LeagueAcronym.ToLowerInvariant() == "smjhl")
                    url = url.Replace("{seasonType}", season.IsPlayoffs ? "Playoffs" : "Season");
                if (season.LeagueAcronym.ToLowerInvariant() == "iihf")
                    url = url.Replace("{seasonType}", season.IsPlayoffs ? "medalround" : "roundrobin");

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
