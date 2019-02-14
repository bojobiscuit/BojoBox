using BojoBox.SthsDataCollector.Moderno;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //http://simulationhockey.com/games/shl/S44/Season/SHL-ProTeamScoring.html
            string urlTemplate = "http://simulationhockey.com/games/{leagueLow}/S{seasonNumber}/{seasonType}/{leagueUp}-ProTeamScoring.html";
            string leagueAcronym = "SHL";
            bool isPlayoffs = false;
            int seasonNumber = 45;

            SeasonData seasonData = new SeasonData(seasonNumber, leagueAcronym, isPlayoffs);

            var loader = new FileLoader(seasonData);
            var document = loader.DownloadFile(urlTemplate, false);

            var extractor = new Extractor(seasonData);
            seasonData = extractor.Extract(document);

            var uploader = new SeasonUploader(seasonData);
            uploader.Upload();
        }
    }
}
