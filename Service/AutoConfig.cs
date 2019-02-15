using AutoMapper;
using BojoBox.EntityFramework.Entities;
using BojoBox.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BojoBox.Service
{
    public static class AutoConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
                cfg.CreateMap<League, LeagueDto>();
                cfg.CreateMap<Franchise, FranchiseDto>();
                cfg.CreateMap<Team, TeamDto>();
                cfg.CreateMap<Skater, SkaterDto>();
                cfg.CreateMap<Goalie, GoalieDto>();
                cfg.CreateMap<SkaterSeason, SkaterSeasonDto>();
                cfg.CreateMap<GoalieSeason, GoalieSeasonDto>();
            });
        }
    }
}
