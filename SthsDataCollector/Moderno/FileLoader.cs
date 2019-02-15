using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace BojoBox.SthsDataCollector.Moderno
{
    public class FileLoader
    {
        public FileLoader(SeasonData season)
        {
            this.season = season;
        }

        public HtmlDocument DownloadFile(string urlTemplate, bool isLegacy)
        {
            //http://simulationhockey.com/games/shl/S44/Season/SHL-ProTeamScoring.html
            //var url = "http://simulationhockey.com/games/{leagueLow}/S{seasonNumber}/{seasonType}/{leagueUp}-ProTeamScoring.html";

            HtmlDocument htmlDocument;

            var nameFormat = (season.IsPlayoffs) ?
                "{1} - PLF - Pro Team Scoring {0}.html" :
                "{1} - Pro Team Scoring {0}.html";

            var folderName = (isLegacy) ?
                @"LegacyFiles\" :
                @"SeasonFiles\";

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
                url = url.Replace("{seasonType}", season.IsPlayoffs ? "Playoffs" : "Season");

                htmlDocument = new HtmlWeb().Load(url);
                htmlDocument.Save(filePath);
            }

            htmlDocument = new HtmlDocument();
            htmlDocument.Load(filePath);

            return htmlDocument;
        }

        private SeasonData season;
    }
}
