﻿using BojoBox.EntityFramework;
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

            //try
            {
                ResetDatabase();
                //RemoveSeason();
            }
            //catch (Exception e)
            {
                //Console.Write(" Error: " + e.Message);
            }

            Console.WriteLine("Press key to exit");
            Console.ReadKey();
            Console.WriteLine("Eat a diiiiiick");
        }

        private static void ResetDatabase()
        {
            Console.WriteLine("### Resetting Database ###");
            Console.WriteLine("Are you sure? (y)");

            var key = Console.ReadKey();
            if (key.KeyChar == 'y')
            {
                DatabaseHelper.ResetDatabase();
                Console.WriteLine("\nDone");
            }
        }

        private static void UploadData()
        {
            //http://simulationhockey.com/games/shl/S28/Season/SHL-ProTeamScoring.html
            //http://simulationhockey.com/games/shl/S31/Playoff/SHL-PLF-ProTeamScoring.html
            //http://simulationhockey.com/games/smjhl/S43/Season/SMJHL-ProTeamScoring.html
            //http://simulationhockey.com/games/smjhl/S43/Playoffs/SMJHL-PLF-ProTeamScoring.html
            //http://simulationhockey.com/games/iihf/S45/roundrobin/IIHF-ProTeamScoring.html
            string urlTemplate = "http://simulationhockey.com/games/{leagueLow}/S{seasonNumber}/{seasonType}/{leagueUp}-{playoffAcro}ProTeamScoring.html";

            string leagueAcronym;
            //leagueAcronym = "SHL";
            leagueAcronym = "SMJHL";
            //leagueAcronym = "IIHF";

            bool isPlayoffs = false;
            //isPlayoffs = true;

            bool isLegacy = false;
            //isLegacy = true;

            List<int> seasonNumbers = new List<int>();
            for (int i = 22; i <= 42; i++)
                seasonNumbers.Add(i);

            foreach (int season in seasonNumbers)
            {
                Console.WriteLine("");
                Console.WriteLine("Season " + season + ": ");
                SeasonData seasonData = new SeasonData(season, leagueAcronym, isPlayoffs);

                try
                {
                    if (isLegacy)
                    {
                        Console.Write("Loading - ");
                        var loader = new LegacyFileLoader(seasonData);
                        var document = loader.LoadFile();

                        Console.Write("Extracting - ");
                        var extractor = new LegacyExtractor(seasonData);
                        seasonData = extractor.Extract(document);
                    }
                    else
                    {
                        Console.Write("Downloading - ");
                        var loader = new FileLoader(seasonData);
                        var document = loader.DownloadFile(urlTemplate, false);

                        Console.Write("Extracting - ");
                        var extractor = new Extractor(seasonData);
                        seasonData = extractor.Extract(document);
                    }

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
