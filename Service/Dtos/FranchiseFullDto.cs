using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class FranchiseFullDto : FranchiseDto
    {
        public LeagueDto League { get; set; }
        public TeamDto CurrentTeam { get; set; }
        public IEnumerable<TeamDto> Teams { get; set; }

        public new static FranchiseFullDto Create(Franchise source) => 
            AutoMapper.Mapper.Map<FranchiseFullDto>(source);
    }
}
