using BojoBox.EntityFramework;
using BojoBox.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System;
using BojoBox.EntityFramework.Entities;

namespace BojoBox.Service
{
    public class WebsiteService : IWebsiteService
    {
        public void Test()
        {
            using (db = new BojoBoxContext())
            {
                var leagues = db.Leagues.ToList();
            }
        }

        public StatTableDto GetSeasonSkaterTable(StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanSeasonParameters(paramDto);
            cleanParameters.Col = cleanParameters.Col ?? 3;

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);

                statTableDto.Teams = GetTeamParameters(leagueDto.Id);
                statTableDto.Seasons = GetSeasonParameters(leagueDto.Id, cleanParameters.SeasonType == 2);
                statTableDto.HeaderText = leagueDto.Acronym + " - Season Stats";

                var skaterSeasonQuery = db.SkaterSeasons
                    .Where(a => a.LeagueId == leagueDto.Id);

                if (teamDto != null)
                {
                    int? franchiseId = null;
                    if (teamDto.FranchiseId.HasValue)
                        franchiseId = db.Franchises.FirstOrDefault(a => a.Id == teamDto.FranchiseId).Id;

                    if (franchiseId.HasValue)
                    {
                        skaterSeasonQuery = skaterSeasonQuery.Include(a => a.Team).Where(a => a.Team.FranchiseId == franchiseId);
                    }
                    else
                    {
                        skaterSeasonQuery = skaterSeasonQuery.Where(a => a.TeamId == teamDto.Id);
                    }
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

                IQueryable<IdStatPair> idStatPairs = GetIdStatPairs(cleanParameters.Col, skaterSeasonQuery);

                idStatPairs = idStatPairs.OrderByDescending(a => a.Stat);
                // TODO: Check if to sort ascending

                int pageSize = 20;
                statTableDto.PageCount = GetPageCount(cleanParameters, idStatPairs);
                int[] selectedIds = idStatPairs.Select(b => b.Id).Skip(cleanParameters.Page.Value * pageSize).Take(pageSize).ToArray();

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

                statTableDto.PlayerRows = rows.OrderByDescending(a => a.Stats.ElementAt(cleanParameters.Col.Value)).ThenByDescending(a => a.Stats.ElementAt(13));
                AddRanks(statTableDto.PlayerRows);
            }

            statTableDto.DisplayType = "season";
            statTableDto.PlayerType = "skater";
            statTableDto.StatParameters = cleanParameters;
            return statTableDto;
        }

        private static int GetPageCount(StatParametersDto cleanParameters, IQueryable<IdStatPair> idStatPairs)
        {
            int pageSize = 20;
            int pageCount = (int)Math.Ceiling((double)idStatPairs.Count() / (double)pageSize);
            pageCount = Math.Min(pageCount, 10);

            if (cleanParameters.Page.Value > pageCount)
                cleanParameters.Page = pageCount;
            if (cleanParameters.Page.Value < 0)
                cleanParameters.Page = 0;

            return pageCount;
        }

        public StatTableDto GetCareerSkaterTable(StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanCareerParameters(paramDto);
            cleanParameters.Col = cleanParameters.Col ?? 3;

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);

                statTableDto.Teams = GetTeamParameters(leagueDto.Id);

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

                var idStatPairs = GetIdStatPairsCareer(cleanParameters.Col, skaterCareerQuery);

                idStatPairs = idStatPairs.GroupBy(a => a.Id, b => b.Stat, (x, y) => new IdStatPair(x, y.Sum()));

                idStatPairs = idStatPairs.OrderByDescending(a => a.Stat);
                // TODO: Check if to sort ascending

                int pageSize = 20;
                statTableDto.PageCount = GetPageCount(cleanParameters, idStatPairs);
                var selectedSkaterIds = idStatPairs.Select(b => b.Id).Skip(cleanParameters.Page.Value * pageSize).Take(pageSize).ToArray();

                var skaterRows = db.SkaterSeasons
                    .Include(a => a.Skater)
                    .Where(a => selectedSkaterIds.Contains(a.SkaterId))
                    .Where(a => a.TeamId.HasValue)
                    .Where(a => a.LeagueId == leagueDto.Id)
                    .Where(a => a.isPlayoffs == isPlayoffs);

                if (teamDto != null)
                    skaterRows = skaterRows.Where(a => a.TeamId == teamDto.Id);

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

                statTableDto.PlayerRows = statTableDto.PlayerRows.OrderByDescending(a => a.Stats.ElementAt(cleanParameters.Col.Value)).ThenByDescending(a => a.Stats.ElementAt(13));
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

                if (cleanParameters.Col == null)
                    rows = rows.OrderByDescending(a => a.Season).ToList();
                else
                    rows = rows.OrderByDescending(a => a.Stats.ElementAt(cleanParameters.Col.Value)).ThenByDescending(a => a.Stats.ElementAt(13)).ToList();

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
            cleanParameters.Col = cleanParameters.Col ?? 1;

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);

                statTableDto.Teams = GetTeamParameters(leagueDto.Id);
                statTableDto.Seasons = GetSeasonParameters(leagueDto.Id, cleanParameters.SeasonType == 2);
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

                var idStatPairs = GetIdStatPairs(cleanParameters.Col, goalieSeasonQuery);

                idStatPairs = idStatPairs.OrderByDescending(a => a.Stat);
                // TODO: Check if to sort ascending

                int pageSize = 20;
                statTableDto.PageCount = GetPageCount(cleanParameters, idStatPairs);
                int[] selectedIds = idStatPairs.Select(b => b.Id).Skip(cleanParameters.Page.Value * pageSize).Take(pageSize).ToArray();

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
                statTableDto.PlayerRows = rows.OrderByDescending(a => a.Stats.ElementAt(cleanParameters.Col.Value)).ThenByDescending(a => a.Stats.ElementAt(4));
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
            cleanParameters.Col = cleanParameters.Col ?? 1;

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);

                statTableDto.Teams = GetTeamParameters(leagueDto.Id);

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

                var idStatPairs = GetIdStatPairsCareer(cleanParameters.Col, goalieCareerQuery);
                idStatPairs = idStatPairs.GroupBy(a => a.Id, b => b.Stat, (x, y) => new IdStatPair(x, y.Sum()));

                idStatPairs = idStatPairs.OrderByDescending(a => a.Stat);
                // TODO: Check if to sort ascending

                int pageSize = 20;
                statTableDto.PageCount = GetPageCount(cleanParameters, idStatPairs);
                var selectedGoalieIds = idStatPairs.Select(b => b.Id).Skip(cleanParameters.Page.Value * pageSize).Take(pageSize).ToArray();

                var goalieRows = db.GoalieSeasons
                    .Include(a => a.Goalie)
                    .Where(a => selectedGoalieIds.Contains(a.GoalieId))
                    .Where(a => a.TeamId.HasValue)
                    .Where(a => a.LeagueId == leagueDto.Id)
                    .Where(a => a.isPlayoffs == isPlayoffs);

                if (teamDto != null)
                    goalieRows = goalieRows.Where(a => a.TeamId == teamDto.Id);

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
                        rows.Sum(x => x.PenaltyShotSaves    ),
                        rows.Sum(x => x.PenaltyShotAttempts ),
                        rows.Sum(x => x.Starts              ),
                        rows.Sum(x => x.Backups             ),
                    }
                })
                .ToList();

                statTableDto.PlayerRows = statTableDto.PlayerRows.OrderByDescending(a => a.Stats.ElementAt(cleanParameters.Col.Value)).ThenByDescending(a => a.Stats.ElementAt(4));
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

                if (cleanParameters.Col == null)
                    rows = rows.OrderByDescending(a => a.Season).ToList();
                else
                    rows = rows.OrderByDescending(a => a.Stats.ElementAt(cleanParameters.Col.Value)).ThenByDescending(a => a.Stats.ElementAt(13)).ToList();

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


        private static IQueryable<IdStatPair> GetIdStatPairs(int? columnIndex, IQueryable<SkaterSeason> skaterSeasonQuery)
        {
            IQueryable<IdStatPair> idStatPairs;
            switch (columnIndex)
            {
                case 0: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.GamesPlayed)); break;
                case 1: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Goals)); break;
                case 2: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Assists)); break;
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
                case 3:
                case null:
                default: idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Points)); break;
            }
            return idStatPairs;
        }

        private static IQueryable<IdStatPair> GetIdStatPairsCareer(int? columnIndex, IQueryable<SkaterSeason> skaterQuery)
        {
            IQueryable<IdStatPair> idStatPairs;
            switch (columnIndex)
            {
                case 0: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.GamesPlayed)); break;
                case 1: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.Goals)); break;
                case 2: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.Assists)); break;
                case 4: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PlusMinus)); break;
                case 5: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PenaltyMinutes)); break;
                case 6: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PenaltyMajors)); break;
                case 7: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.Hits)); break;
                case 8: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.HitsTaken)); break;
                case 9: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.Shots)); break;
                case 10: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.OwnShotsBlocked)); break;
                case 11: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.OwnShotsMissed)); break;
                case 12: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.ShotsBlocked)); break;
                case 13: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.MinutesPlayed)); break;
                case 14: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PPGoals)); break;
                case 15: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PPAssists)); break;
                case 16: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PPPoints)); break;
                case 17: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PPShots)); break;
                case 18: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PPMinutes)); break;
                case 19: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PKGoals)); break;
                case 20: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PKAssists)); break;
                case 21: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PKPoints)); break;
                case 22: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PKShots)); break;
                case 23: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PKMinutes)); break;
                case 24: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.GameWinningGoals)); break;
                case 25: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.GameTyingGoals)); break;
                case 26: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.FaceoffWins)); break;
                case 27: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.FaceoffsTotal)); break;
                case 28: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.EmptyNetGoals)); break;
                case 29: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.HatTricks)); break;
                case 30: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PenaltyShotGoals)); break;
                case 31: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.PenaltyShotAttempts)); break;
                case 32: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.FightsWon)); break;
                case 33: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.FightsLost)); break;
                case 34: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.FightsDraw)); break;
                case 35: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.FaceoffWins)); break;
                case 3:
                case null:
                default: idStatPairs = skaterQuery.Select(a => new IdStatPair(a.SkaterId, a.Points)); break;
            }
            return idStatPairs;
        }

        private static IQueryable<IdStatPair> GetIdStatPairs(int? columnIndex, IQueryable<GoalieSeason> goalieSeasonQuery)
        {
            IQueryable<IdStatPair> idStatPairs;
            switch (columnIndex)
            {
                case 0: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.GamesPlayed)); break;
                case 2: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.Losses)); break;
                case 3: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.OvertimeLosses)); break;
                case 4: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.Minutes)); break;
                case 5: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.PenaltyMinutes)); break;
                case 6: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.Shutouts)); break;
                case 7: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.GoalsAgainst)); break;
                case 8: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.ShotsAgainst)); break;
                case 9: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.Assists)); break;
                case 10: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.EmptyGoalAgainst)); break;
                case 11: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.PenaltyShotAttempts)); break;
                case 12: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.PenaltyShotSaves)); break;
                case 13: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.Starts)); break;
                case 14: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.Backups)); break;
                case 1:
                case null:
                default: idStatPairs = goalieSeasonQuery.Select(a => new IdStatPair(a.Id, a.Wins)); break;
            }
            return idStatPairs;
        }

        private static IQueryable<IdStatPair> GetIdStatPairsCareer(int? columnIndex, IQueryable<GoalieSeason> goalieQuery)
        {
            IQueryable<IdStatPair> idStatPairs;
            switch (columnIndex)
            {
                case 0: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.GamesPlayed)); break;
                case 2: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.Losses)); break;
                case 4: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.OvertimeLosses)); break;
                case 5: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.Minutes)); break;
                case 6: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.PenaltyMinutes)); break;
                case 7: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.Shutouts)); break;
                case 8: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.GoalsAgainst)); break;
                case 9: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.ShotsAgainst)); break;
                case 10: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.Assists)); break;
                case 11: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.EmptyGoalAgainst)); break;
                case 12: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.PenaltyShotAttempts)); break;
                case 13: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.PenaltyShotSaves)); break;
                case 14: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.Starts)); break;
                case 15: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.Backups)); break;
                case 1:
                case null:
                default: idStatPairs = goalieQuery.Select(a => new IdStatPair(a.GoalieId, a.Wins)); break;
            }
            return idStatPairs;
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
                dto.PenaltyShotSaves    ,
                dto.PenaltyShotAttempts ,
                dto.Starts              ,
                dto.Backups             ,
            };
        }

        private List<int> GetSeasonParameters(int leagueId, bool isPlayoffs)
        {
            return db.SkaterSeasons
                .Where(a => a.LeagueId == leagueId && a.isPlayoffs == isPlayoffs)
                .Select(a => a.Season)
                .Distinct()
                .OrderByDescending(a => a)
                .ToList();
        }

        private static void AddRanks(IEnumerable<PlayerTableRow> rows)
        {
            int i = 1;
            foreach (var row in rows)
                row.Rank = i++;
        }

        private List<TeamDto> GetTeamParameters(int leagueId)
        {
            return db.Franchises
                .Where(a => a.LeagueId == leagueId)
                .Select(a => TeamDto.Create(a.CurrentTeam))
                .OrderBy(a => a.Name)
                .ToList();
        }

        private static StatParametersDto CleanPlayerParameters(StatParametersDto paramDto)
        {
            StatParametersDto cleanParameters = new StatParametersDto();
            cleanParameters.Season = null;
            cleanParameters.Era = null;
            cleanParameters.Team = paramDto.Team ?? 0;
            cleanParameters.League = paramDto.League ?? 1;
            cleanParameters.SeasonType = paramDto.SeasonType ?? 1;
            cleanParameters.Col = paramDto.Col;
            cleanParameters.Page = paramDto.Page ?? 0;
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
            cleanParameters.Col = paramDto.Col;
            cleanParameters.Page = paramDto.Page ?? 0;

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
            cleanParameters.Col = paramDto.Col;
            cleanParameters.Page = paramDto.Page ?? 0;

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
