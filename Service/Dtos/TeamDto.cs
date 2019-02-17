using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }
        public int? FranchiseId { get; set; }

        public string Name { get; set; }
        public string Acronym { get; set; }

        public static TeamDto Create(Team source) => 
            AutoMapper.Mapper.Map<TeamDto>(source);
    }
}
