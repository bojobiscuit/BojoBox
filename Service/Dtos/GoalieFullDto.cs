using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class GoalieFullDto : PlayerDto
    {
        public IEnumerable<GoalieSeasonDto> Seasons { get; set; }

        public new static GoalieFullDto Create(Goalie source) => 
            AutoMapper.Mapper.Map<GoalieFullDto>(source);
    }
}
