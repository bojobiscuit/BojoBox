using BojoBox.SthsDataCollector.Moderno;
using System;
using System.Collections.Generic;

namespace BojoBox.DatabaseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //http://simulationhockey.com/games/shl/S44/Season/SHL-ProTeamScoring.html
            // http://simulationhockey.com/games/shl/S31/Playoff/SHL-PLF-ProTeamScoring.html
            string urlTemplate = "http://simulationhockey.com/games/{leagueLow}/S{seasonNumber}/{seasonType}/{leagueUp}-{playoffAcro}ProTeamScoring.html";

            string leagueAcronym = "SHL";
            bool isPlayoffs = false;

            List<int> seasonNumbers = new List<int>();
            for (int i = 31; i <= 44; i++)
                seasonNumbers.Add(i);

            foreach (int season in seasonNumbers)
            {
                Console.WriteLine("");
                Console.WriteLine("Season " + season + ": ");
                SeasonData seasonData = new SeasonData(season, leagueAcronym, isPlayoffs);

                Console.Write("Downloading - ");
                var loader = new FileLoader(seasonData);
                var document = loader.DownloadFile(urlTemplate, false);

                Console.Write("Extracting - ");
                var extractor = new Extractor(seasonData);
                seasonData = extractor.Extract(document);

                Console.Write("Uploading - ");
                var uploader = new SeasonUploader(seasonData);
                uploader.Upload();
            }
        }
    }
}
