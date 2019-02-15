﻿using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class GoalieDto
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }

        public string Name { get; set; }

        public LeagueDto League { get; set; }
        public IEnumerable<GoalieSeasonDto> Seasons { get; set; }

        public static GoalieDto Create(Goalie source) => 
            AutoMapper.Mapper.Map<GoalieDto>(source);
    }
}
