using BojoBox.EntityFramework;
using BojoBox.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System;

namespace BojoBox.Service
{
    public class WebsiteService : IWebsiteService
    {
        public StatTableDto GetSeasonSkaterTable(StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanSeasonParameters(paramDto);

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);

                statTableDto.Teams = GetTeamParameters();
                statTableDto.Seasons = GetSeasonParameters();
                statTableDto.HeaderText = leagueDto.Acronym + " - Season Stats";

                var skaterSeasonQuery = db.SkaterSeasons
                    .Where(a => a.LeagueId == leagueDto.Id);

                if (teamDto != null)
                {
                    skaterSeasonQuery = skaterSeasonQuery.Where(a => a.TeamId == teamDto.Id);
                }
                else
                {
                    skaterSeasonQuery = skaterSeasonQuery.Where(a => a.SubtotalForId == null);
                }

                switch (cleanParameters.SeasonType)
                {
                    case 1: skaterSeasonQuery = skaterSeasonQuery.Where(a => !a.isPlayoffs); break;
                    case 2: skaterSeasonQuery = skaterSeasonQuery.Where(a => a.isPlayoffs); break;
                }

                if (cleanParameters.Era > 0)
                {
                    switch (cleanParameters.Era)
                    {
                        case 1: skaterSeasonQuery = skaterSeasonQuery.Where(a => a.Season >= 20); break;
                        case 2: skaterSeasonQuery = skaterSeasonQuery.Where(a => a.Season < 20 && a.Season > 10); break;
                        case 3: skaterSeasonQuery = skaterSeasonQuery.Where(a => a.Season <= 10); break;
                    }
                }

                if (cleanParameters.Season > 0)
                {
                    skaterSeasonQuery = skaterSeasonQuery.Where(a => a.Season == cleanParameters.Season);
                }

                IQueryable<IdStatPair> idStatPairs = null;
                switch (cleanParameters.SelectedColumnIndex)
                {
                    case 0: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.GamesPlayed)); break;
                    case 1: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Goals)); break;
                    case 2: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Assists)); break;
                    case null:
                    case 3: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Points)); break;
                    case 4: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PlusMinus)); break;
                    case 5: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PenaltyMinutes)); break;
                    case 6: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PenaltyMajors)); break;
                    case 7: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Hits)); break;
                    case 8: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.HitsTaken)); break;
                    case 9: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Shots)); break;
                    case 10: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.OwnShotsBlocked)); break;
                    case 11: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.OwnShotsMissed)); break;
                    case 12: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.ShotsBlocked)); break;
                    case 13: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.MinutesPlayed)); break;
                    case 14: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PPGoals)); break;
                    case 15: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PPAssists)); break;
                    case 16: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PPPoints)); break;
                    case 17: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PPShots)); break;
                    case 18: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PPMinutes)); break;
                    case 19: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PKGoals)); break;
                    case 20: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PKAssists)); break;
                    case 21: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PKPoints)); break;
                    case 22: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PKShots)); break;
                    case 23: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PKMinutes)); break;
                    case 24: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.GameWinningGoals)); break;
                    case 25: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.GameTyingGoals)); break;
                    case 26: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.FaceoffWins)); break;
                    case 27: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.FaceoffsTotal)); break;
                    case 28: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.EmptyNetGoals)); break;
                    case 29: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.HatTricks)); break;
                    case 30: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PenaltyShotGoals)); break;
                    case 31: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.PenaltyShotAttempts)); break;
                    case 32: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.FightsWon)); break;
                    case 33: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.FightsLost)); break;
                    case 34: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.FightsDraw)); break;
                    case 35: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.FaceoffWins)); break;
                }
                // var idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Points));
                // TODO: Do this for each stat. ugh.

                idStatPairs = idStatPairs.OrderByDescending(a => a.Stat);
                // TODO: Check if to sort ascending

                int count = idStatPairs.Count();

                int[] selectedIds = idStatPairs.Select(b => b.Id).Skip(0).Take(20).ToArray();
                // TODO: Pagniation

                var skaterSeasonDtos = db.SkaterSeasons
                    .Include(a => a.Team)
                    .Include(a => a.Skater)
                    .Where(a => selectedIds.Contains(a.Id))
                    .Select(a => SkaterSeasonFullDto.Create(a))
                    .ToList();

                List<PlayerTableRow> rows = new List<PlayerTableRow>();
                foreach (var dto in skaterSeasonDtos)
                {
                    PlayerTableRow row = new PlayerTableRow();
                    row.Player = dto.Skater;
                    row.Season = dto.Season;
                    row.Team = dto.Team;
                    row.Stats = GetSkaterStats(dto);
                    rows.Add(row);
                }

                int sortColumn = cleanParameters.SelectedColumnIndex ?? 3;
                statTableDto.PlayerRows = rows.OrderByDescending(a => a.Stats.ElementAt(sortColumn));
                AddRanks(statTableDto.PlayerRows);
            }

            statTableDto.DisplayType = "season";
            statTableDto.PlayerType = "skater";
            statTableDto.StatParameters = cleanParameters;
            return statTableDto;
        }

        public StatTableDto GetCareerSkaterTable(StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanCareerParameters(paramDto);

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);

                statTableDto.Teams = GetTeamParameters();

                statTableDto.HeaderText = leagueDto.Acronym + " - Career Stats";

                var skaterCareerQuery = db.SkaterSeasons.Where(a => a.LeagueId == leagueDto.Id);

                if (teamDto != null)
                {
                    skaterCareerQuery = skaterCareerQuery.Where(a => a.TeamId == teamDto.Id);
                }
                else
                {
                    skaterCareerQuery = skaterCareerQuery.Where(a => a.TeamId.HasValue);
                }

                bool isPlayoffs = CheckIfPlayoffs(cleanParameters.SeasonType.Value);
                skaterCareerQuery = skaterCareerQuery.Where(a => a.isPlayoffs == isPlayoffs);

                cleanParameters.SelectedColumnIndex = 3;
                var idStatPairs = skaterCareerQuery.Select(a => new IdStatPair(a.SkaterId, a.Points));
                // TODO: Do this for each stat. ugh.

                idStatPairs = idStatPairs.GroupBy(a => a.Id, b => b.Stat, (x, y) => new IdStatPair(x, y.Sum()));

                idStatPairs = idStatPairs.OrderByDescending(a => a.Stat);
                // TODO: Check if to sort ascending

                var selectedSkaterIds = idStatPairs.Select(b => b.Id).Skip(0).Take(20).ToArray();

                var count = idStatPairs.Count();
                // TODO: Pagniation

                var skaterRows = db.SkaterSeasons
                    .Include(a => a.Skater)
                    .Where(a => selectedSkaterIds.Contains(a.SkaterId))
                    .Where(a => a.TeamId.HasValue)
                    .Where(a => a.LeagueId == leagueDto.Id)
                    .Where(a => a.isPlayoffs == isPlayoffs);

                int teamCount = skaterCareerQuery.Where(a => a.TeamId.HasValue).Select(a => a.TeamId).Distinct().Count();

                statTableDto.PlayerRows = skaterRows.GroupBy(a => a.Skater, b => b, (skater, rows) => new PlayerTableRow()
                {
                    Player = PlayerDto.Create(skater),
                    Season = null,
                    Team = null,
                    SeasonCount = rows.Select(a => a.Season).Distinct().Count(),
                    TeamCount = rows.Where(x => x.SkaterId == skater.Id).Select(x => x.TeamId).Distinct().Count(),
                    Stats = new int[]
                    {
                        rows.Sum(x => x.GamesPlayed         ),
                        rows.Sum(x => x.Goals               ),
                        rows.Sum(x => x.Assists             ),
                        rows.Sum(x => x.Points              ),
                        rows.Sum(x => x.PlusMinus           ),
                        rows.Sum(x => x.PenaltyMinutes      ),
                        rows.Sum(x => x.PenaltyMajors       ),
                        rows.Sum(x => x.Hits                ),
                        rows.Sum(x => x.HitsTaken           ),
                        rows.Sum(x => x.Shots               ),
                        rows.Sum(x => x.OwnShotsBlocked     ),
                        rows.Sum(x => x.OwnShotsMissed      ),
                        rows.Sum(x => x.ShotsBlocked        ),
                        rows.Sum(x => x.MinutesPlayed       ),
                        rows.Sum(x => x.PPGoals             ),
                        rows.Sum(x => x.PPAssists           ),
                        rows.Sum(x => x.PPPoints            ),
                        rows.Sum(x => x.PPShots             ),
                        rows.Sum(x => x.PPMinutes           ),
                        rows.Sum(x => x.PKGoals             ),
                        rows.Sum(x => x.PKAssists           ),
                        rows.Sum(x => x.PKPoints            ),
                        rows.Sum(x => x.PKShots             ),
                        rows.Sum(x => x.PKMinutes           ),
                        rows.Sum(x => x.GameWinningGoals    ),
                        rows.Sum(x => x.GameTyingGoals      ),
                        rows.Sum(x => x.FaceoffWins         ),
                        rows.Sum(x => x.FaceoffsTotal       ),
                        rows.Sum(x => x.EmptyNetGoals       ),
                        rows.Sum(x => x.HatTricks           ),
                        rows.Sum(x => x.PenaltyShotGoals    ),
                        rows.Sum(x => x.PenaltyShotAttempts ),
                        rows.Sum(x => x.FightsWon           ),
                        rows.Sum(x => x.FightsLost          ),
                        rows.Sum(x => x.FightsDraw          )
                    }
                })
                .ToList();

                statTableDto.PlayerRows = statTableDto.PlayerRows.OrderByDescending(a => a.Stats.ElementAt(3));
                AddRanks(statTableDto.PlayerRows);
            }

            statTableDto.DisplayType = "career";
            statTableDto.PlayerType = "skater";
            statTableDto.StatParameters = cleanParameters;
            return statTableDto;
        }

        public StatTableDto GetPlayerSkaterTable(int skaterId, StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanPlayerParameters(paramDto);

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);
                PlayerDto skater = GetSkater(skaterId);

                statTableDto.HeaderText = skater.Name;

                var skaterQuery = db.SkaterSeasons
                    .Include(a => a.Skater)
                    .Include(a => a.Team)
                    .Include(a => a.League)
                    .Where(a => a.SkaterId == skater.Id);

                bool isPlayoffs = CheckIfPlayoffs(cleanParameters.SeasonType.Value);
                skaterQuery = skaterQuery.Where(a => a.isPlayoffs == isPlayoffs);

                if (cleanParameters.League > 0)
                {
                    skaterQuery = skaterQuery.Where(a => a.LeagueId == leagueDto.Id);
                }

                int teamCount = skaterQuery.Where(a => a.Team != null).Select(a => a.Team.Id).Distinct().Count();

                statTableDto.Teams = skaterQuery.Select(a => a.Team).Where(a => a != null).DistinctBy(a => a.Id).Select(a => TeamDto.Create(a)).ToList();

                if (cleanParameters.Team > 0)
                {
                    skaterQuery = skaterQuery.Where(a => a.TeamId == cleanParameters.Team);
                }
                else
                {
                    skaterQuery = skaterQuery.Where(a => a.SubtotalForId == null);
                }

                List<SkaterSeasonFullDto> skaterSeasonDtos = skaterQuery.Select(a => SkaterSeasonFullDto.Create(a)).ToList();


                int i = 1;
                List<PlayerTableRow> rows = new List<PlayerTableRow>();
                foreach (var dto in skaterSeasonDtos)
                {
                    PlayerTableRow row = new PlayerTableRow();
                    row.Player = dto.Skater;
                    row.Rank = i++;
                    row.Season = dto.Season;
                    row.Team = dto.Team;
                    row.Stats = GetSkaterStats(dto);
                    rows.Add(row);
                }

                rows = rows.OrderByDescending(a => a.Season).ToList();
                statTableDto.PlayerRows = rows;

                List<int> totals = new List<int>();
                if (rows.Any())
                {
                    totals = GetTotals(rows);
                    totals.Insert(0, teamCount);
                    totals.Insert(0, rows.Count());
                }
                statTableDto.Totals = totals;
            }

            statTableDto.DisplayType = "player";
            statTableDto.PlayerType = "skater";
            statTableDto.StatParameters = cleanParameters;
            return statTableDto;
        }


        public StatTableDto GetSeasonGoalieTable(StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanSeasonParameters(paramDto);

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);

                statTableDto.Teams = GetTeamParameters();
                statTableDto.Seasons = GetSeasonParameters();
                statTableDto.HeaderText = leagueDto.Acronym + " - Season Stats";

                var goalieSeasonQuery = db.GoalieSeasons
                    .Where(a => a.LeagueId == leagueDto.Id);

                if (teamDto != null)
                {
                    goalieSeasonQuery = goalieSeasonQuery.Where(a => a.TeamId == teamDto.Id);
                }
                else
                {
                    goalieSeasonQuery = goalieSeasonQuery.Where(a => a.SubtotalForId == null);
                }

                switch (cleanParameters.SeasonType)
                {
                    case 1: goalieSeasonQuery = goalieSeasonQuery.Where(a => !a.isPlayoffs); break;
                    case 2: goalieSeasonQuery = goalieSeasonQuery.Where(a => a.isPlayoffs); break;
                }

                if (cleanParameters.Era > 0)
                {
                    switch (cleanParameters.Era)
                    {
                        case 1: goalieSeasonQuery = goalieSeasonQuery.Where(a => a.Season >= 20); break;
                        case 2: goalieSeasonQuery = goalieSeasonQuery.Where(a => a.Season < 20 && a.Season > 10); break;
                        case 3: goalieSeasonQuery = goalieSeasonQuery.Where(a => a.Season <= 10); break;
                    }
                }

                if (cleanParameters.Season > 0)
                {
                    goalieSeasonQuery = goalieSeasonQuery.Where(a => a.Season == cleanParameters.Season);
                }

                cleanParameters.SelectedColumnIndex = 1;
                var idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.Wins));
                // TODO: Do this for each stat. ugh.

                idStatPairs = idStatPairs.OrderByDescending(a => a.Stat);
                // TODO: Check if to sort ascending

                int count = idStatPairs.Count();

                int[] selectedIds = idStatPairs.Select(b => b.Id).Skip(0).Take(20).ToArray();
                // TODO: Pagniation

                var goalieSeasonDtos = db.GoalieSeasons
                    .Include(a => a.Team)
                    .Include(a => a.Goalie)
                    .Where(a => selectedIds.Contains(a.Id))
                    .Select(a => GoalieSeasonFullDto.Create(a))
                    .ToList();

                List<PlayerTableRow> rows = new List<PlayerTableRow>();
                foreach (var dto in goalieSeasonDtos)
                {
                    PlayerTableRow row = new PlayerTableRow();
                    row.Player = dto.Goalie;
                    row.Season = dto.Season;
                    row.Team = dto.Team;
                    row.Stats = GetGoalieStats(dto);
                    rows.Add(row);
                }
                statTableDto.PlayerRows = rows.OrderByDescending(a => a.Stats.ElementAt(1));
                AddRanks(statTableDto.PlayerRows);
            }

            statTableDto.DisplayType = "season";
            statTableDto.PlayerType = "goalie";
            statTableDto.StatParameters = cleanParameters;
            return statTableDto;
        }

        public StatTableDto GetCareerGoalieTable(StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanCareerParameters(paramDto);

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);

                statTableDto.Teams = GetTeamParameters();

                statTableDto.HeaderText = leagueDto.Acronym + " - Career Stats";

                var goalieCareerQuery = db.GoalieSeasons.Where(a => a.LeagueId == leagueDto.Id);

                if (teamDto != null)
                {
                    goalieCareerQuery = goalieCareerQuery.Where(a => a.TeamId == teamDto.Id);
                }
                else
                {
                    goalieCareerQuery = goalieCareerQuery.Where(a => a.TeamId.HasValue);
                }

                bool isPlayoffs = CheckIfPlayoffs(cleanParameters.SeasonType.Value);
                goalieCareerQuery = goalieCareerQuery.Where(a => a.isPlayoffs == isPlayoffs);

                cleanParameters.SelectedColumnIndex = 1;
                var idStatPairs = goalieCareerQuery.Select(a => new IdStatPair(a.GoalieId, a.Wins));
                // TODO: Do this for each stat. ugh.

                idStatPairs = idStatPairs.GroupBy(a => a.Id, b => b.Stat, (x, y) => new IdStatPair(x, y.Sum()));

                idStatPairs = idStatPairs.OrderByDescending(a => a.Stat);
                // TODO: Check if to sort ascending

                var selectedGoalieIds = idStatPairs.Select(b => b.Id).Skip(0).Take(20).ToArray();

                var count = idStatPairs.Count();
                // TODO: Pagniation

                var goalieRows = db.GoalieSeasons
                    .Include(a => a.Goalie)
                    .Where(a => selectedGoalieIds.Contains(a.GoalieId))
                    .Where(a => a.TeamId.HasValue)
                    .Where(a => a.LeagueId == leagueDto.Id)
                    .Where(a => a.isPlayoffs == isPlayoffs);

                int teamCount = goalieCareerQuery.Where(a => a.TeamId.HasValue).Select(a => a.TeamId).Distinct().Count();

                statTableDto.PlayerRows = goalieRows.GroupBy(a => a.Goalie, b => b, (skater, rows) => new PlayerTableRow()
                {
                    Player = PlayerDto.Create(skater),
                    Season = null,
                    Team = null,
                    SeasonCount = rows.Select(a => a.Season).Distinct().Count(),
                    TeamCount = rows.Where(x => x.GoalieId == skater.Id).Select(x => x.TeamId).Distinct().Count(),
                    Stats = new int[]
                    {
                        rows.Sum(x => x.GamesPlayed         ),
                        rows.Sum(x => x.Wins                ),
                        rows.Sum(x => x.Losses              ),
                        rows.Sum(x => x.OvertimeLosses      ),
                        rows.Sum(x => x.Minutes             ),
                        rows.Sum(x => x.PenaltyMinutes      ),
                        rows.Sum(x => x.Shutouts            ),
                        rows.Sum(x => x.GoalsAgainst        ),
                        rows.Sum(x => x.ShotsAgainst        ),
                        rows.Sum(x => x.Assists             ),
                        rows.Sum(x => x.EmptyGoalAgainst    ),
                        rows.Sum(x => x.PenaltyShotAttempts ),
                        rows.Sum(x => x.Starts              ),
                        rows.Sum(x => x.Backups             ),
                        rows.Sum(x => x.PenaltyShotSaves    ),
                    }
                })
                .ToList();

                statTableDto.PlayerRows = statTableDto.PlayerRows.OrderByDescending(a => a.Stats.ElementAt(1));
                AddRanks(statTableDto.PlayerRows);
            }

            statTableDto.DisplayType = "career";
            statTableDto.PlayerType = "goalie";
            statTableDto.StatParameters = cleanParameters;
            return statTableDto;
        }

        public StatTableDto GetPlayerGoalieTable(int goalieId, StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanPlayerParameters(paramDto);

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);
                PlayerDto goalieDto = GetGoalie(goalieId);

                statTableDto.HeaderText = goalieDto.Name;

                var goalieSeasonsQuery = db.GoalieSeasons
                    .Include(a => a.Goalie)
                    .Include(a => a.Team)
                    .Include(a => a.League)
                    .Where(a => a.GoalieId == goalieDto.Id);

                bool isPlayoffs = CheckIfPlayoffs(cleanParameters.SeasonType.Value);
                goalieSeasonsQuery = goalieSeasonsQuery.Where(a => a.isPlayoffs == isPlayoffs);

                if (cleanParameters.League > 0)
                {
                    goalieSeasonsQuery = goalieSeasonsQuery.Where(a => a.LeagueId == leagueDto.Id);
                }

                int teamCount = goalieSeasonsQuery.Where(a => a.Team != null).Select(a => a.Team.Id).Distinct().Count();

                statTableDto.Teams = goalieSeasonsQuery.Select(a => a.Team).Where(a => a != null).DistinctBy(a => a.Id).Select(a => TeamDto.Create(a)).ToList();

                if (cleanParameters.Team > 0)
                {
                    goalieSeasonsQuery = goalieSeasonsQuery.Where(a => a.TeamId == cleanParameters.Team);
                }
                else
                {
                    goalieSeasonsQuery = goalieSeasonsQuery.Where(a => a.SubtotalForId == null);
                }

                List<GoalieSeasonFullDto> goalieSeasonDtos = goalieSeasonsQuery.Select(a => GoalieSeasonFullDto.Create(a)).ToList();

                int i = 1;
                List<PlayerTableRow> rows = new List<PlayerTableRow>();
                foreach (var dto in goalieSeasonDtos)
                {
                    PlayerTableRow row = new PlayerTableRow();
                    row.Player = dto.Goalie;
                    row.Rank = i++;
                    row.Season = dto.Season;
                    row.Team = dto.Team;
                    row.Stats = GetGoalieStats(dto);
                    rows.Add(row);
                }

                rows = rows.OrderByDescending(a => a.Season).ToList();
                statTableDto.PlayerRows = rows;

                List<int> totals = new List<int>();
                if (rows.Any())
                {
                    totals = GetTotals(rows);
                    totals.Insert(0, teamCount);
                    totals.Insert(0, rows.Count());
                }
                statTableDto.Totals = totals;
            }

            statTableDto.DisplayType = "player";
            statTableDto.PlayerType = "goalie";
            statTableDto.StatParameters = cleanParameters;
            return statTableDto;
        }



        private static bool CheckIfPlayoffs(int seasonType)
        {
            bool isPlayoffs = seasonType == 2;
            if (seasonType != 1 && seasonType != 2)
                throw new Exception("season type not valid");
            return isPlayoffs;
        }

        private PlayerDto GetSkater(int skaterId)
        {
            var skater = db.Skaters.Select(a => PlayerDto.Create(a)).FirstOrDefault(a => a.Id == skaterId);
            if (skater == null)
                throw new Exception("skater not found");
            return skater;
        }

        private PlayerDto GetGoalie(int skaterId)
        {
            var skater = db.Goalies.Select(a => PlayerDto.Create(a)).FirstOrDefault(a => a.Id == skaterId);
            if (skater == null)
                throw new Exception("goalie not found");
            return skater;
        }

        private TeamDto GetTeam(int teamId)
        {
            TeamDto teamDto = null;
            if (teamId > 0)
                teamDto = db.Teams.Select(a => TeamDto.Create(a)).FirstOrDefault(a => a.Id == teamId);
            return teamDto;
        }

        private LeagueDto GetLeague(int leagueId)
        {
            var leagueDto = db.Leagues.Select(a => LeagueDto.Create(a)).FirstOrDefault(a => a.Id == leagueId);
            if (leagueDto == null)
                throw new Exception("league doesn't exist");
            return leagueDto;
        }

        private static List<int> GetTotals(List<PlayerTableRow> rows)
        {
            List<int> totals;
            var statLists = rows.Select(a => a.Stats);
            totals = statLists.First().ToList();
            foreach (var row in statLists.Skip(1))
                totals = totals.Zip(row, (a, b) => a + b).ToList();
            return totals;
        }

        private static int[] GetSkaterStats(SkaterSeasonFullDto dto)
        {
            return new int[]
            {
                dto.GamesPlayed         ,
                dto.Goals               ,
                dto.Assists             ,
                dto.Points              ,
                dto.PlusMinus           ,
                dto.PenaltyMinutes      ,
                dto.PenaltyMajors       ,
                dto.Hits                ,
                dto.HitsTaken           ,
                dto.Shots               ,
                dto.OwnShotsBlocked     ,
                dto.OwnShotsMissed      ,
                dto.ShotsBlocked        ,
                dto.MinutesPlayed       ,
                dto.PPGoals             ,
                dto.PPAssists           ,
                dto.PPPoints            ,
                dto.PPShots             ,
                dto.PPMinutes           ,
                dto.PKGoals             ,
                dto.PKAssists           ,
                dto.PKPoints            ,
                dto.PKShots             ,
                dto.PKMinutes           ,
                dto.GameWinningGoals    ,
                dto.GameTyingGoals      ,
                dto.FaceoffWins         ,
                dto.FaceoffsTotal       ,
                dto.EmptyNetGoals       ,
                dto.HatTricks           ,
                dto.PenaltyShotGoals    ,
                dto.PenaltyShotAttempts ,
                dto.FightsWon           ,
                dto.FightsLost          ,
                dto.FightsDraw          ,
            };
        }

        private static int[] GetGoalieStats(GoalieSeasonFullDto dto)
        {
            return new int[]
            {
                dto.GamesPlayed         ,
                dto.Wins                ,
                dto.Losses              ,
                dto.OvertimeLosses      ,
                dto.Minutes             ,
                dto.PenaltyMinutes      ,
                dto.Shutouts            ,
                dto.GoalsAgainst        ,
                dto.ShotsAgainst        ,
                dto.Assists             ,
                dto.EmptyGoalAgainst    ,
                dto.PenaltyShotAttempts ,
                dto.Starts              ,
                dto.Backups             ,
                dto.PenaltyShotSaves    ,
            };
        }

        private List<int> GetSeasonParameters()
        {
            return db.SkaterSeasons.Select(a => a.Season).Distinct().OrderByDescending(a => a).ToList();
        }

        private static void AddRanks(IEnumerable<PlayerTableRow> rows)
        {
            int i = 1;
            foreach (var row in rows)
                row.Rank = i++;
        }

        private List<TeamDto> GetTeamParameters()
        {
            return db.Teams.Select(a => TeamDto.Create(a)).OrderBy(a => a.Name).ToList();
        }

        private static StatParametersDto CleanPlayerParameters(StatParametersDto paramDto)
        {
            StatParametersDto cleanParameters = new StatParametersDto();
            cleanParameters.Season = null;
            cleanParameters.Era = null;
            cleanParameters.Team = paramDto.Team ?? 0;
            cleanParameters.League = paramDto.League ?? 1;
            cleanParameters.SeasonType = paramDto.SeasonType ?? 1;
            cleanParameters.SelectedColumnIndex = paramDto.SelectedColumnIndex ?? 3;
            return cleanParameters;
        }

        private static StatParametersDto CleanSeasonParameters(StatParametersDto paramDto)
        {
            StatParametersDto cleanParameters = new StatParametersDto();
            cleanParameters.Season = paramDto.Season ?? 0;
            cleanParameters.Era = paramDto.Era ?? 0;
            cleanParameters.Team = paramDto.Team ?? 0;
            cleanParameters.League = paramDto.League ?? 1;
            cleanParameters.SeasonType = paramDto.SeasonType ?? 1;
            cleanParameters.SelectedColumnIndex = paramDto.SelectedColumnIndex;

            if (cleanParameters.Era > 0)
                cleanParameters.Season = 0;

            return cleanParameters;
        }

        private static StatParametersDto CleanCareerParameters(StatParametersDto paramDto)
        {
            StatParametersDto cleanParameters = new StatParametersDto();
            cleanParameters.Season = null;
            cleanParameters.Era = null;
            cleanParameters.Team = paramDto.Team ?? 0;
            cleanParameters.League = paramDto.League ?? 1;
            cleanParameters.SeasonType = paramDto.SeasonType ?? 1;
            cleanParameters.SelectedColumnIndex = paramDto.SelectedColumnIndex ?? 3;
            return cleanParameters;
        }

        private BojoBoxContext db;

        private class IdStatPair
        {
            public IdStatPair(int id, int stat)
            {
                Id = id;
                Stat = stat;
            }

            public int Id { get; set; }
            public int Stat { get; set; }
        }
    }
}
