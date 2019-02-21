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
            db.Leagues.Add(new League() { Acronym = "SMJHL", Name = "Simulated Major Junior Hockey League" });
            db.Leagues.Add(new League() { Acronym = "IIHF", Name = "International Ice Hockey Federation" });

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
