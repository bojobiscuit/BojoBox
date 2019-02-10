using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SthsDataCollector.Models
{
    public abstract class Player
    {
        public string Name { get; set; }
        public bool IsRookie { get; set; }
        public bool IsCaptain { get; set; }
        public bool IsAlternate { get; set; }
    }
}
