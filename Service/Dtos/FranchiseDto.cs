using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class FranchiseDto
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public int CurrentTeamId { get; set; }

        public static FranchiseDto Create(Franchise source) => 
            AutoMapper.Mapper.Map<FranchiseDto>(source);
    }
}
