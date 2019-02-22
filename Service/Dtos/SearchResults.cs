using BojoBox.EntityFramework.Entities;
using System.Collections.Generic;

namespace BojoBox.Service.Dtos
{
    public class SearchResults
    {
        public string SearchInput { get; set; }
        public PlayerDto[] Results { get; set; }
    }
}
