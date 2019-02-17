using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class GoalieSeasonFullDto : GoalieSeasonDto
    {
        public GoalieDto Goalie { get; set; }
        public LeagueDto League { get; set; }
        public TeamDto Team { get; set; }
        public GoalieSeasonDto SubtotalFor { get; set; }
        public IEnumerable<GoalieSeasonDto> SubTotals { get; set; }

        public new static GoalieSeasonFullDto Create(GoalieSeason source) => 
            AutoMapper.Mapper.Map<GoalieSeasonFullDto>(source);
    }
}
