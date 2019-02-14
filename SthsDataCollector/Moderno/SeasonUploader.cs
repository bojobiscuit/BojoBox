using BojoBox.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BojoBox.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace BojoBox.SthsDataCollector.Moderno
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
                // TODO: Add leagues by default
                //league = db.Leagues.First(a => a.Acronym == season.LeagueAcronym);
                league = new League();

                DeleteExistingSeason();
                db.SaveChanges();

                AddTeams();
                AddSkaters();
                AddGoalies();
                db.SaveChanges();
            }
        }

        private void DeleteExistingSeason()
        {
            var skaterSeasons = db.SkaterSeasons.Where(a => a.Season == season.SeasonNumber);
            var goalieSeasons = db.GoalieSeasons.Where(a => a.Season == season.SeasonNumber);

            db.SkaterSeasons.RemoveRange(skaterSeasons);
            db.GoalieSeasons.RemoveRange(goalieSeasons);
        }

        private void AddTeams()
        {
            foreach (var team in season.Teams)
            {
                var dbTeam = db.Teams.FirstOrDefault(a => a.Acronym == team.Acronym);
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
            GoalieSeason previousTotalGoalieSeason = null;
            foreach (var goalieRow in season.GoalieRows)
            {
                var dbGoalie = db.Goalies.FirstOrDefault(a => a.Name == goalieRow.Name);
                if (dbGoalie == null)
                {
                    dbGoalie = new Goalie() { Name = goalieRow.Name, League = league };
                    db.Goalies.Add(dbGoalie);
                }

                GoalieSeason dbGoalieSeason = new GoalieSeason()
                {
                    Team = teams.First(a => a.Acronym == goalieRow.TeamAcronym),
                    Goalie = dbGoalie,
                    Season = season.SeasonNumber,
                    isPlayoffs = season.IsPlayoffs,
                };

                if (goalieRow.IsSubTotal)
                {
                    dbGoalieSeason.SubtotalFor = previousTotalGoalieSeason;
                    previousTotalGoalieSeason.Team = null;
                }
                else
                {
                    previousTotalGoalieSeason = dbGoalieSeason;
                }

                db.GoalieSeasons.Add(dbGoalieSeason);
            }
        }

        private void AddSkaters()
        {
            SkaterSeason previousTotalSkaterSeason = null;
            foreach (var skaterRow in season.SkaterRows)
            {
                var dbSkater = db.Skaters.FirstOrDefault(a => a.Name == skaterRow.Name);
                if (dbSkater == null)
                {
                    dbSkater = new Skater() { Name = skaterRow.Name, League = league };
                    db.Skaters.Add(dbSkater);
                }

                SkaterSeason dbSkaterSeason = new SkaterSeason()
                {
                    Team = teams.First(a => a.Acronym == skaterRow.TeamAcronym),
                    Skater = dbSkater,
                    Season = season.SeasonNumber,
                    isPlayoffs = season.IsPlayoffs,
                };

                if (skaterRow.IsSubtotal)
                {
                    dbSkaterSeason.SubtotalFor = previousTotalSkaterSeason;
                    previousTotalSkaterSeason = null;
                }
                else
                {
                    previousTotalSkaterSeason = dbSkaterSeason;
                }

                db.SkaterSeasons.Add(dbSkaterSeason);
            }
        }

        private SeasonData season;
        private BojoBoxContext db;
        private League league;
        private List<Team> teams = new List<Team>();
    }
}
