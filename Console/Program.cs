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
            //BojoBoxContext.ConnectionString = "Data Source=localhost;Database=bojoboxdb;Initial Catalog=bojoboxdb;User ID=sa;Password=Passw0rd123;";
            BojoBoxContext.ConnectionString = "Server=209.182.219.138,1433;Initial Catalog=startnetdb;User ID=sa;Password=Passw0rd123;Connection Timeout=30;";

            ResetDatabase();

            UploadData();

            //RemoveExtraSkaters();

            // int seasonNumber = 46;
            // UploadSeason(new SeasonPack() { number = seasonNumber, leagueAcro = "SHL", isPlayoffs = false });
            //UploadSeason(new SeasonPack() { number = seasonNumber, leagueAcro = "SHL", isPlayoffs = true });
            //UploadSeason(new SeasonPack() { number = seasonNumber, leagueAcro = "SMJHL", isPlayoffs = false });
            //UploadSeason(new SeasonPack() { number = seasonNumber, leagueAcro = "SMJHL", isPlayoffs = true });

            // MergeSkaters(1239, 642);

            // SplitSkater(737, 25, "I", "II");

            // RenameSkater(619, "John Langabeer");

            //ListTeamsInFranchise("SFP");

            // AddTeamToFranchise("POR", "SFP");

            // MergeTeams(109, 110,11,112,113,114,115,116,117);

            Console.WriteLine("Press key to exit");
            Console.ReadKey();
            Console.WriteLine("Eat a diiiiiick");
        }

        private static void MergeTeams(int idKeep, params int[] idsjoin)
        {
            Console.WriteLine("### Merging Teams ###");
            Console.WriteLine("Are you sure? (y)");

            var key = Console.ReadKey();

            if (key.KeyChar == 'y')
            {
                DatabaseHelper.MergeTeams(idKeep, idsjoin);
                Console.WriteLine("\nDone");
            }            
        }

        private static void MergeSkaters(int idKeep, int idJoin)
        {
            Console.WriteLine("### Merging Skaters ###");
            Console.WriteLine("Are you sure? (y)");

            var key = Console.ReadKey();

            if (key.KeyChar == 'y')
            {
                DatabaseHelper.MergePlayers(idKeep, idJoin);
                Console.WriteLine("\nDone");
            }
        }

        private static void SplitSkater(int skaterId, int lastSeason, string suffixA, string suffixB)
        {
            Console.WriteLine("### Splitting Skaters ###");
            Console.WriteLine("Are you sure? (y)");

            var key = Console.ReadKey();

            if (key.KeyChar == 'y')
            {
                DatabaseHelper.SplitPlayer(skaterId, lastSeason, suffixA, suffixB);
                Console.WriteLine("\nDone");
            }
        }

        private static void RenameSkater(int skaterId, string name)
        {
            Console.WriteLine("### Renaming Skater ###");
            Console.WriteLine("Are you sure? (y)");

            var key = Console.ReadKey();

            if (key.KeyChar == 'y')
            {
                DatabaseHelper.RenamePlayer(skaterId, name);
                Console.WriteLine("\nDone");
            }
        }

        private static void ResetDatabase()
        {
            Console.WriteLine("### Resetting Database ###");
            Console.WriteLine("Are you sure? (y)");

            var key = Console.ReadKey();

            if (key.KeyChar == 'y')
            {
                DatabaseHelper.ResetDatabase();
                // UploadData();
                Console.WriteLine("\nDone");
            }
        }

        private static void RemoveExtraSkaters()
        {
            Console.WriteLine("### Removing extra players ###");
            DatabaseHelper.RemoveExtraPlayers();
            Console.WriteLine("\nDone");
        }

        private static void ListTeamsInFranchise(string acronym)
        {
            Console.WriteLine("### Listing teams in franchise: " + acronym + " ###");
            foreach (var team in DatabaseHelper.GetTeamsFromFranchise(acronym))
                Console.WriteLine(team);
            Console.WriteLine("\nDone");
        }
        private static void AddTeamToFranchise(string teamAcro, string franchiseAcro)
        {
            Console.WriteLine("### Adding team to franchise: " + teamAcro + " ###");
            DatabaseHelper.AddTeamToFranchise(teamAcro, franchiseAcro);
            ListTeamsInFranchise(franchiseAcro);
            Console.WriteLine("\nDone");
        }

        private static void UploadData()
        {
            //http://simulationhockey.com/games/shl/S28/Season/SHL-ProTeamScoring.html
            //http://simulationhockey.com/games/shl/S31/Playoff/SHL-PLF-ProTeamScoring.html
            //http://simulationhockey.com/games/smjhl/S43/Season/SMJHL-ProTeamScoring.html
            //http://simulationhockey.com/games/smjhl/S43/Playoffs/SMJHL-PLF-ProTeamScoring.html
            //http://simulationhockey.com/games/iihf/S45/roundrobin/IIHF-ProTeamScoring.html
            //string urlTemplate = "http://simulationhockey.com/games/{leagueLow}/S{seasonNumber}/{seasonType}/{leagueUp}-{playoffAcro}ProTeamScoring.html";

            int lastSeason = 47;

            List<SeasonPack> seasonPacks = new List<SeasonPack>();

            for (int i = 3; i <= lastSeason; i++)
                seasonPacks.Add(new SeasonPack() { number = i, leagueAcro = "SHL", isPlayoffs = false, isLegacy = i <= 27 });

            for (int i = 17; i <= lastSeason; i++)
               seasonPacks.Add(new SeasonPack() { number = i, leagueAcro = "SHL", isPlayoffs = true, isLegacy = i <= 22 });

            for (int i = 15; i <= lastSeason; i++)
                seasonPacks.Add(new SeasonPack() { number = i, leagueAcro = "SMJHL", isPlayoffs = false, isLegacy = i <= 22 });

            for (int i = 17; i <= lastSeason; i++)
               seasonPacks.Add(new SeasonPack() { number = i, leagueAcro = "SMJHL", isPlayoffs = true, isLegacy = i <= 21 });

            for (int i = 22; i <= lastSeason; i++)
               seasonPacks.Add(new SeasonPack() { number = i, leagueAcro = "IIHF", isPlayoffs = false, isLegacy = i <= 22 });

            for (int i = 22; i <= lastSeason; i++)
               seasonPacks.Add(new SeasonPack() { number = i, leagueAcro = "IIHF", isPlayoffs = true, isLegacy = i <= 22 });

            foreach (var season in seasonPacks)
            {
                // if (!season.isLegacy)
                    UploadSeason(season);
            }
        }

        private static void UploadSeason(SeasonPack season)
        {
            string urlTemplate = "http://simulationhockey.com/games/{leagueLow}/S{seasonNumber}/{seasonType}/{leagueUp}-{playoffAcro}ProTeamScoring.html";

            Console.WriteLine("");
            Console.WriteLine(season.leagueAcro + ": " + season.number + ": " + (season.isPlayoffs ? "PLF" : "") + ": " + (season.isLegacy ? "LEG" : ""));
            SeasonData seasonData = new SeasonData(season.number, season.leagueAcro, season.isPlayoffs);

            try
            {
                if (season.isLegacy)
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

        private class SeasonPack
        {
            public int number;
            public bool isPlayoffs;
            public bool isLegacy;
            public string leagueAcro;
        }
    }
}
