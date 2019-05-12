using BojoBox.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BojoBox.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using BojoBox.SthsDataCollector.Model;
using System.Data.SqlClient;

namespace BojoBox.SthsDataCollector
{
    public class DatabaseHelper
    {
        public static void ResetDatabase()
        {
            using (var db = new BojoBoxContext())
            {
                DeleteData(db);
                InitializeData(db);
            }
        }

        public static IEnumerable<string> GetTeamsFromFranchise(string teamAcronym)
        {
            using (var db = new BojoBoxContext())
            {
                var franchise = db.Franchises.Include(a => a.CurrentTeam).Include(a => a.Teams)
                    .FirstOrDefault(a => a.CurrentTeam.Acronym == teamAcronym);

                if (franchise == null)
                    return null;

                return franchise.Teams.Select(a => a.Name);
            }
        }

        public static void AddTeamToFranchise(string teamAcronym, string franchiseAcro)
        {
            using (var db = new BojoBoxContext())
            {
                var franchise = db.Franchises.Include(a => a.CurrentTeam).Include(a => a.Teams)
                    .FirstOrDefault(a => a.CurrentTeam.Acronym == franchiseAcro);

                var team = db.Teams.FirstOrDefault(a => a.Acronym == teamAcronym);

                if (franchise == null || team == null)
                    return;

                var teams = franchise.Teams.ToList();
                teams.Add(team);
                franchise.Teams = teams;

                db.SaveChanges();
            }
        }

        public static void RemoveExtraPlayers()
        {
            using (var db = new BojoBoxContext())
            {
                var skatersToRemove = db.Skaters.Include(a => a.Seasons).Where(a => !a.Seasons.Any()).ToList();
                db.Skaters.RemoveRange(skatersToRemove);
                db.SaveChanges();
            }
        }

        public static void MergePlayers(int skaterToKeepId, int skaterToJoinId)
        {
            using (var db = new BojoBoxContext())
            {
                var skaterKeep = db.Skaters.First(a => a.Id == skaterToKeepId);
                var skaterJoin = db.Skaters.First(a => a.Id == skaterToJoinId);

                var skaterKeepSeasons = db.SkaterSeasons.Where(a => a.SkaterId == skaterKeep.Id).ToList();
                var skaterJoinSeasons = db.SkaterSeasons.Where(a => a.SkaterId == skaterJoin.Id).ToList();

                var mergeSeasons = new List<SkaterSeason>();
                mergeSeasons.AddRange(skaterKeepSeasons);
                mergeSeasons.AddRange(skaterJoinSeasons);

                foreach (var season in mergeSeasons)
                {
                    season.Skater = skaterKeep;
                    season.SkaterId = skaterKeep.Id;
                }

                db.SaveChanges();

                db.Skaters.Remove(skaterJoin);
                db.SaveChanges();
            }
        }

        public static void MergeTeams(int idKeep, int[] idsjoin)
        {
            using (var db = new BojoBoxContext())
            {
                var teamKeep = db.Teams.First(a => a.Id == idKeep);
                var teamsJoin = db.Teams.Where(a => idsjoin.Contains(a.Id));

                if (teamKeep == null || teamsJoin.Count() != idsjoin.Count())
                    throw new Exception("Team not found");

                var teamKeepSeasons = db.SkaterSeasons.Where(a => a.TeamId == teamKeep.Id).ToList();
                var teamJoinSeasons = db.SkaterSeasons.Where(a => a.TeamId.HasValue && idsjoin.Contains(a.TeamId.Value)).ToList();
                var teamKeepSeasonsGoalies = db.GoalieSeasons.Where(a => a.TeamId == teamKeep.Id).ToList();
                var teamJoinSeasonsGoalies = db.GoalieSeasons.Where(a => a.TeamId.HasValue && idsjoin.Contains(a.TeamId.Value)).ToList();

                var mergeSeasons = new List<SkaterSeason>();
                mergeSeasons.AddRange(teamKeepSeasons);
                mergeSeasons.AddRange(teamJoinSeasons);
                foreach (var season in mergeSeasons)
                {
                    season.Team = teamKeep;
                    season.TeamId = teamKeep.Id;
                }

                var mergeSeasonsGoalies = new List<GoalieSeason>();
                mergeSeasonsGoalies.AddRange(teamKeepSeasonsGoalies);
                mergeSeasonsGoalies.AddRange(teamJoinSeasonsGoalies);
                foreach (var season in mergeSeasonsGoalies)
                {
                    season.Team = teamKeep;
                    season.TeamId = teamKeep.Id;
                }

                db.SaveChanges();

                foreach (var team in teamsJoin)
                    db.Teams.Remove(team);

                db.SaveChanges();
            }
        }

        public static void RenamePlayer(int skaterId, string name)
        {
            using (var db = new BojoBoxContext())
            {
                var skaterA = db.Skaters.First(a => a.Id == skaterId);
                skaterA.Name = name;
                db.SaveChanges();
            }
        }

        public static void SplitPlayer(int skaterId, int firstPlayerLasstSeason, string suffixA, string suffixB)
        {
            using (var db = new BojoBoxContext())
            {
                var skaterA = db.Skaters.First(a => a.Id == skaterId);
                var skaterName = skaterA.Name;

                var skaterSeasonsA = db.SkaterSeasons.Where(a => a.SkaterId == skaterA.Id && a.Season <= firstPlayerLasstSeason).ToList();
                var skaterSeasonsB = db.SkaterSeasons.Where(a => a.SkaterId == skaterA.Id && a.Season > firstPlayerLasstSeason).ToList();

                skaterA.Name = skaterName + " " + suffixA;
                skaterA.Seasons = skaterSeasonsA;

                var skaterB = new Skater();
                skaterB.Name = skaterName + " " + suffixB;
                skaterB.Seasons = skaterSeasonsB;
                skaterB.LeagueId = skaterA.LeagueId;
                db.Skaters.Add(skaterB);

                db.SaveChanges();
            }
        }

        private static void DeleteData(BojoBoxContext db)
        {
            db.SkaterSeasons.RemoveRange(db.SkaterSeasons);
            db.GoalieSeasons.RemoveRange(db.GoalieSeasons);
            db.Goalies.RemoveRange(db.Goalies);
            db.Skaters.RemoveRange(db.Skaters);
            db.Teams.RemoveRange(db.Teams);
            db.SaveChanges();

            db.Franchises.RemoveRange(db.Franchises);
            db.Leagues.RemoveRange(db.Leagues);
            db.SaveChanges();

            string[] tableNames = { "SkaterSeasons", "GoalieSeasons", "Goalies", "Skaters", "Teams", "Franchises", "Leagues" };
            foreach (var table in tableNames)
            {
                string sql = "DBCC CHECKIDENT('" + table + "', RESEED, 0)";

#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                db.Database.ExecuteSqlCommand(sql);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.

                db.SaveChanges();
            }
        }

        private static void InitializeData(BojoBoxContext db)
        {
            db.Leagues.Add(new League() { Acronym = "SHL", Name = "Simulated Hockey League" });
            db.SaveChanges();
            db.Leagues.Add(new League() { Acronym = "SMJHL", Name = "Simulated Major Junior Hockey League" });
            db.SaveChanges();
            db.Leagues.Add(new League() { Acronym = "IIHF", Name = "International Ice Hockey Federation" });
            db.SaveChanges();

            string[] shlNames = {
                    "Winnipeg Jets"            ,
                    "Buffalo Stampede"         ,
                    "New England Wolfpack"     ,
                    "Los Angeles Panthers"     ,
                    "Toronto North Stars"      ,
                    "Calgary Dragons"          ,
                    "San Francisco Pride"      ,
                    "Hamilton Steelhawks"      ,
                    "West Kendall Platoon"     ,
                    "Seattle Riot"             ,
                    "Manhattan Rage"           ,
                    "Edmonton Blizzard"        ,
                    "Minnesota Chiefs"         ,
                    "Texas Renegades"          ,
            };
            string[] shlAcronyms = {
                    "WIN",
                    "BUF",
                    "NEW",
                    "LAP",
                    "TOR",
                    "CAL",
                    "SFP",
                    "HAM",
                    "WKP",
                    "SEA",
                    "MAN",
                    "EDM",
                    "MIN",
                    "TEX",
            };

            string[] smjhlNames = {
                    "Anchorage Armada"         ,
                    "Colorado Raptors"         ,
                    "Vancouver Whalers"        ,
                    "Halifax Raiders"          ,
                    "Detroit Falcons"          ,
                    "St.Louis Scarecrows"      ,
                    "Kelowna Knights"          ,
                    "Montreal Militia"         ,
                    "Lethbridge Lions"         ,
                    "Anaheim Outlaws"          ,
            };
            string[] smjhlAcronyms = {
                    "ANC",
                    "COL",
                    "VAN",
                    "HAL",
                    "DET",
                    "STL",
                    "KEL",
                    "MTL",
                    "LBL",
                    "ANA",
            };

            string[] iihfNames = {
                    "Sweden"         ,
                    "USA"            ,
                    "United Kingdom" ,
                    "Ireland"        ,
                    "Norway"         ,
                    "Latvia"         ,
                    "Germany"        ,
                    "Finland"        ,
                    "Czechoslovakia" ,
                    "Canada"         ,
                    "Russia"         ,
                    "Austria"        ,
            };
            string[] iihfAcronyms = {
                    "SWE",
                    "USA",
                    "GBR",
                    "IRL",
                    "NOR",
                    "LAT",
                    "GER",
                    "FIN",
                    "CZE",
                    "CAN",
                    "RUS",
                    "AUS",
            };

            AddTeamsAndFranchises(db, shlNames, shlAcronyms, 1);
            AddTeamsAndFranchises(db, smjhlNames, smjhlAcronyms, 2);
            AddTeamsAndFranchises(db, iihfNames, iihfAcronyms, 3);

            db.SaveChanges();
        }

        private static void AddTeamsAndFranchises(BojoBoxContext db, string[] teamNames, string[] teamAcronyms, int leagueId)
        {
            for (int i = 0; i < teamNames.Length; i++)
            {
                var name = teamNames[i];
                var acronym = teamAcronyms[i];
                var team = new Team()
                {
                    Acronym = acronym,
                    Name = name,
                };
                db.Teams.Add(team);
            }
            db.SaveChanges();

            for (int i = 0; i < teamAcronyms.Length; i++)
            {
                var acronym = teamAcronyms[i];
                var team = db.Teams.First(a => a.Acronym == acronym);
                var franchise = new Franchise()
                {
                    LeagueId = leagueId,
                    CurrentTeamId = team.Id
                };
                franchise.Teams = new Team[] { team };
                db.Franchises.Add(franchise);
            }

            db.SaveChanges();
        }

        public static void DeleteExistingSeason(BojoBoxContext db, int seasonNumber, bool isPlayoffs, int leagueId)
        {
            var skaterSeasons = db.SkaterSeasons.Where(a => a.Season == seasonNumber).Where(a => a.isPlayoffs == isPlayoffs).Where(a => a.LeagueId == leagueId);
            var goalieSeasons = db.GoalieSeasons.Where(a => a.Season == seasonNumber).Where(a => a.isPlayoffs == isPlayoffs).Where(a => a.LeagueId == leagueId);
            db.SkaterSeasons.RemoveRange(skaterSeasons);
            db.GoalieSeasons.RemoveRange(goalieSeasons);
        }
    }
}
