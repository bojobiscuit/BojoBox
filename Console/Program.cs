using BojoBox.EntityFramework;
using BojoBox.SthsDataCollector;
using BojoBox.SthsDataCollector.Legacy;
using BojoBox.SthsDataCollector.Model;
using BojoBox.SthsDataCollector.Modern;
using System;
using System.Collections.Generic;

namespace BojoBox.DatabaseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Manually updating databases
            //BojoBoxContext.ConnectionString = "Server=tcp:bojoboxdbserver.database.windows.net,1433;Initial Catalog=BojoBoxDb;Persist Security Info=False;User ID=bojobiscuit;Password=omgCAT123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            BojoBoxContext.ConnectionString = "Data Source=localhost;Database=bojoboxdb;Initial Catalog=bojoboxdb;User ID=sa;Password=Passw0rd123;";

            //UploadData();
            // ConvertLegacyToModern();

            Console.ReadKey();
        }

        private static void CheckForLeagues() 
        {
            
        }

        private static void UploadData()
        {
            //http://simulationhockey.com/games/shl/S28/Season/SHL-ProTeamScoring.html
            //http://simulationhockey.com/games/shl/S31/Playoff/SHL-PLF-ProTeamScoring.html
            //http://simulationhockey.com/games/smjhl/S43/Season/SMJHL-ProTeamScoring.html
            //http://simulationhockey.com/games/smjhl/S43/Playoffs/SMJHL-PLF-ProTeamScoring.html
            string urlTemplate = "http://simulationhockey.com/games/{leagueLow}/S{seasonNumber}/{seasonType}/{leagueUp}-{playoffAcro}ProTeamScoring.html";

            string leagueAcronym = "SHL";
            //string leagueAcronym = "SMJHL";
            //bool isPlayoffs = true;
            bool isPlayoffs = false;

            List<int> seasonNumbers = new List<int>();
            for (int i = 28; i <= 45; i++)
                seasonNumbers.Add(i);

            foreach (int season in seasonNumbers)
            {
                Console.WriteLine("");
                Console.WriteLine("Season " + season + ": ");
                SeasonData seasonData = new SeasonData(season, leagueAcronym, isPlayoffs);

                try
                {
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
                catch (Exception e)
                {
                    Console.Write("Error: " + e.Message);
                }
            }
        }

        private static void ConvertLegacyToModern()
        {
            string leagueAcronym = "SHL";
            leagueAcronym = "SMJHL";

            bool isPlayoffs = false;
            isPlayoffs = true;

            List<int> seasonNumbers = new List<int>();
            for (int i = 3; i <= 27; i++)
                seasonNumbers.Add(i);

            foreach (int season in seasonNumbers)
            {
                Console.WriteLine("");
                Console.WriteLine("Season " + season + ": ");
                SeasonData seasonData = new SeasonData(season, leagueAcronym, isPlayoffs);

                try
                {
                    Console.Write("Loading - ");
                    var loader = new LegacyFileLoader(seasonData);
                    var document = loader.LoadFile();

                    Console.Write("Extracting - ");
                    var extractor = new LegacyExtractor(seasonData);
                    seasonData = extractor.Extract(document);

                    Console.Write("Uploading - ");
                    var uploader = new SeasonUploader(seasonData);
                    uploader.Upload();
                }
                catch (Exception e)
                {
                    Console.Write("Error: " + e.Message);
                }
            }
        }
    }
}
