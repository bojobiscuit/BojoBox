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
        public StatTableDto GetSkaterTable(int skaterId, StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanPlayerParameters(paramDto);

            using (db = new BojoBoxContext())
            {
                LeagueDto leagueDto = GetLeague(cleanParameters.League.Value);
                TeamDto teamDto = GetTeam(cleanParameters.Team.Value);
                SkaterDto skater = GetSkater(skaterId);

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
                    row.Skater = dto.Skater;
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

        public StatTableDto GetSeasonTable(StatParametersDto paramDto)
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

                cleanParameters.SelectedColumnIndex = 3;
                var idStatPairs = skaterSeasonQuery.Select(a => new IdStatPair(a.Id, a.Points));
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
                    row.Skater = dto.Skater;
                    row.Season = dto.Season;
                    row.Team = dto.Team;
                    row.Stats = GetSkaterStats(dto);
                    rows.Add(row);
                }
                statTableDto.PlayerRows = rows.OrderByDescending(a => a.Stats.ElementAt(3));
                AddRanks(statTableDto.PlayerRows);
            }

            statTableDto.DisplayType = "season";
            statTableDto.PlayerType = "skater";
            statTableDto.StatParameters = cleanParameters;
            return statTableDto;
        }

        public StatTableDto GetCareerTable(StatParametersDto paramDto)
        {
            StatTableDto statTableDto = new StatTableDto();
            StatParametersDto cleanParameters = CleanSeasonParameters(paramDto);

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
                    Skater = SkaterDto.Create(skater),
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

        private static bool CheckIfPlayoffs(int seasonType)
        {
            bool isPlayoffs = seasonType == 2;
            if (seasonType != 1 && seasonType != 2)
                throw new Exception("season type not valid");
            return isPlayoffs;
        }

        private SkaterDto GetSkater(int skaterId)
        {
            var skater = db.Skaters.Select(a => SkaterDto.Create(a)).FirstOrDefault(a => a.Id == skaterId);
            if (skater == null)
                throw new Exception("skater not found");
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
            // TODO: cleanParameters.SelectedColumnIndex = paramDto.SelectedColumnIndex ?? 0;
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
            // TODO: cleanParameters.SelectedColumnIndex = paramDto.SelectedColumnIndex ?? 0;

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
            // TODO: cleanParameters.SelectedColumnIndex = paramDto.SelectedColumnIndex ?? 0;
            return cleanParameters;
        }

        public SkaterFullDto GetSkater(int id, StatParametersDto statParametersDto)
        {
            SkaterFullDto skater = null;
            using (var db = new BojoBoxContext())
            {
                skater = db.Skaters
                    .Include(a => a.Seasons)
                    .Select(a => SkaterFullDto.Create(a))
                    .FirstOrDefault(a => a.Id == id);
            }

            RemoveSubtotalsFromMainSeasonList(skater);

            return skater;
        }

        private static void RemoveSubtotalsFromMainSeasonList(SkaterFullDto skater)
        {
            var seasons = skater.Seasons.ToList();
            seasons.RemoveAll(a => a.SubtotalForId != null);
            skater.Seasons = seasons.OrderByDescending(a => a.Season);
        }

        public IEnumerable<SkaterDto> GetSkaters()
        {
            IEnumerable<SkaterDto> skaters = null;
            using (var db = new BojoBoxContext())
            {
                skaters = db.Skaters
                    //.Include(a => a.Seasons)
                    //.Include(a => a.League)
                    .Select(a => SkaterDto.Create(a))
                    .ToList();
            }
            return skaters;
        }

        public IEnumerable<SkaterSeasonDto> GetSkaterSeasons()
        {
            IEnumerable<SkaterSeasonDto> skaterSeasons = null;
            using (var db = new BojoBoxContext())
            {
                skaterSeasons = db.SkaterSeasons
                    //.Include(a => a.Seasons)
                    //.Include(a => a.League)
                    .Select(a => SkaterSeasonDto.Create(a))
                    .ToList();
            }
            return skaterSeasons;
        }

        BojoBoxContext db;

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
