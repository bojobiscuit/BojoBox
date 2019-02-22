using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class PlayerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Acronym { get; set; }

        public static PlayerDto Create(Skater source) =>
            AutoMapper.Mapper.Map<PlayerDto>(source);

        public static PlayerDto Create(Goalie source) =>
            AutoMapper.Mapper.Map<PlayerDto>(source);
    }
}
