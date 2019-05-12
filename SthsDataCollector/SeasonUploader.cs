using BojoBox.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BojoBox.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using BojoBox.SthsDataCollector.Model;

namespace BojoBox.SthsDataCollector
{
    public class SeasonUploader
    {
        public SeasonUploader(SeasonData season)
        {
            this.season = season;
        }

        public void Upload()
        {
            using (db = new BojoBoxContext())
            {
                GetLeague();
                DeleteExistingSeason();
                db.SaveChanges();

                AddTeams();
                AddSkaters();
                AddGoalies();
                db.SaveChanges();
            }
        }

        private void GetLeague()
        {
            if (!db.Leagues.Any(a => a.Id == 1))
                db.Leagues.Add(new League() { Id = 1, Acronym = "SHL", Name = "Simulated Hockey League" });
            if (!db.Leagues.Any(a => a.Id == 2))
                db.Leagues.Add(new League() { Id = 2, Acronym = "SMJHL", Name = "Simulated Major Junior Hockey League" });
            if (!db.Leagues.Any(a => a.Id == 3))
                db.Leagues.Add(new League() { Id = 3, Acronym = "IIHF", Name = "International Ice Hockey Federation" });

            db.SaveChanges();

            league = db.Leagues.First(a => a.Acronym == season.LeagueAcronym);

            if (league == null)
                throw new Exception("League doesn't exist: " + season.LeagueAcronym);
        }

        private void DeleteExistingSeason()
        {
            var skaterSeasons = db.SkaterSeasons.Where(a => a.Season == season.SeasonNumber).Where(a => a.isPlayoffs == season.IsPlayoffs).Where(a => a.LeagueId == league.Id);
            var goalieSeasons = db.GoalieSeasons.Where(a => a.Season == season.SeasonNumber).Where(a => a.isPlayoffs == season.IsPlayoffs).Where(a => a.LeagueId == league.Id);
            db.SkaterSeasons.RemoveRange(skaterSeasons);
            db.GoalieSeasons.RemoveRange(goalieSeasons);

            //db.Skaters.RemoveRange(db.Skaters);
            //db.Goalies.RemoveRange(db.Goalies);
        }

        private void AddTeams()
        {
            foreach (var team in season.Teams)
            {
                var dbTeam = db.Teams
                    .FirstOrDefault(a => a.Name == team.Name && a.Acronym == team.Acronym);

                if (dbTeam == null)
                {
                    dbTeam = new Team()
                    {
                        Acronym = team.Acronym,
                        Name = team.Name,
                    };
                }
                teams.Add(dbTeam);
            }
        }

        private void AddGoalies()
        {
            Goalie previousGoalie = new Goalie() { Name = "" };
            GoalieSeason previousSeason = null;
            foreach (var goalieRow in season.GoalieRows)
            {
                Goalie dbGoalie = GetGoalieFromRow(previousGoalie, goalieRow);

                int i = 0;
                GoalieSeason dbGoalieSeason = new GoalieSeason()
                {
                    Goalie = dbGoalie,
                    League = league,
                    Season = season.SeasonNumber,
                    isPlayoffs = season.IsPlayoffs,
                    GamesPlayed = goalieRow.Stats[i++],
                    Wins = goalieRow.Stats[i++],
                    Losses = goalieRow.Stats[i++],
                    OvertimeLosses = goalieRow.Stats[i++],
                    Minutes = goalieRow.Stats[i++],
                    PenaltyMinutes = goalieRow.Stats[i++],
                    Shutouts = goalieRow.Stats[i++],
                    GoalsAgainst = goalieRow.Stats[i++],
                    ShotsAgainst = goalieRow.Stats[i++],
                    Assists = goalieRow.Stats[i++],
                    EmptyGoalAgainst = goalieRow.Stats[i++],
                    PenaltyShotAttempts = goalieRow.Stats[i++],
                    Starts = goalieRow.Stats[i++],
                    Backups = goalieRow.Stats[i++],
                    PenaltyShotSaves = goalieRow.Stats[i++],
                };


                dbGoalieSeason.Team = (goalieRow.TeamAcronym != null) ?
                    teams.First(a => a.Acronym == goalieRow.TeamAcronym) : null;

                if (goalieRow.IsSubTotal)
                {
                    dbGoalieSeason.SubtotalFor = previousSeason;
                }
                else
                {
                    previousGoalie = dbGoalie;
                    previousSeason = dbGoalieSeason;
                }

                db.GoalieSeasons.Add(dbGoalieSeason);
            }
        }

        private void AddSkaters()
        {
            Skater previousSkater = new Skater() { Name = "" };
            SkaterSeason previousSeason = new SkaterSeason();
            foreach (var skaterRow in season.SkaterRows)
            {
                Skater dbSkater = GetSkaterFromRow(previousSkater, skaterRow);

                int i = 0;
                SkaterSeason dbSkaterSeason = new SkaterSeason()
                {
                    Skater = dbSkater,
                    League = league,
                    Season = season.SeasonNumber,
                    isPlayoffs = season.IsPlayoffs,
                    GamesPlayed = skaterRow.Stats[i++],
                    Goals = skaterRow.Stats[i++],
                    Assists = skaterRow.Stats[i++],
                    Points = skaterRow.Stats[i++],
                    PlusMinus = skaterRow.Stats[i++],
                    PenaltyMinutes = skaterRow.Stats[i++],
                    PenaltyMajors = skaterRow.Stats[i++],
                    Hits = skaterRow.Stats[i++],
                    HitsTaken = skaterRow.Stats[i++],
                    Shots = skaterRow.Stats[i++],
                    OwnShotsBlocked = skaterRow.Stats[i++],
                    OwnShotsMissed = skaterRow.Stats[i++],
                    ShotsBlocked = skaterRow.Stats[i++],
                    MinutesPlayed = skaterRow.Stats[i++],
                    PPGoals = skaterRow.Stats[i++],
                    PPAssists = skaterRow.Stats[i++],
                    PPPoints = skaterRow.Stats[i++],
                    PPShots = skaterRow.Stats[i++],
                    PPMinutes = skaterRow.Stats[i++],
                    PKGoals = skaterRow.Stats[i++],
                    PKAssists = skaterRow.Stats[i++],
                    PKPoints = skaterRow.Stats[i++],
                    PKShots = skaterRow.Stats[i++],
                    PKMinutes = skaterRow.Stats[i++],
                    GameWinningGoals = skaterRow.Stats[i++],
                    GameTyingGoals = skaterRow.Stats[i++],
                    FaceoffsTotal = skaterRow.Stats[i++],
                    EmptyNetGoals = skaterRow.Stats[i++],
                    HatTricks = skaterRow.Stats[i++],
                    PenaltyShotGoals = skaterRow.Stats[i++],
                    PenaltyShotAttempts = skaterRow.Stats[i++],
                    FightsWon = skaterRow.Stats[i++],
                    FightsLost = skaterRow.Stats[i++],
                    FightsDraw = skaterRow.Stats[i++],
                    FaceoffWins = skaterRow.Stats[i++],
                };

                dbSkaterSeason.Team = (skaterRow.TeamAcronym != null) ?
                    teams.First(a => a.Acronym == skaterRow.TeamAcronym) : null;

                if (skaterRow.IsSubTotal)
                {
                    dbSkaterSeason.SubtotalFor = previousSeason;
                }
                else
                {
                    previousSkater = dbSkater;
                    previousSeason = dbSkaterSeason;
                }

                db.SkaterSeasons.Add(dbSkaterSeason);
            }
        }

        private Skater GetSkaterFromRow(Skater previousSkater, SkaterRow skaterRow)
        {
            Skater dbSkater = null;
            if (previousSkater.Name == skaterRow.Name)
            {
                dbSkater = previousSkater;
            }
            if (dbSkater == null)
            {
                dbSkater = db.Skaters.FirstOrDefault(a => a.Name == skaterRow.Name);
            }
            if (dbSkater == null)
            {
                dbSkater = new Skater() { Name = skaterRow.Name };
                db.Skaters.Add(dbSkater);
            }
            return dbSkater;
        }

        private Goalie GetGoalieFromRow(Goalie previousGoalie, GoalieRow goalieRow)
        {
            Goalie dbGoalie = null;
            if (previousGoalie.Name == goalieRow.Name)
            {
                dbGoalie = previousGoalie;
            }
            if (dbGoalie == null)
            {
                dbGoalie = db.Goalies.FirstOrDefault(a => a.Name == goalieRow.Name);
            }
            if (dbGoalie == null)
            {
                dbGoalie = new Goalie() { Name = goalieRow.Name };
                db.Goalies.Add(dbGoalie);
            }
            return dbGoalie;
        }

        private readonly SeasonData season;
        private BojoBoxContext db;
        private League league;
        private List<Team> teams = new List<Team>();
    }
}
