using System;
using System.Collections.Generic;
using System.Text;

namespace BojoBox.SthsDataCollector.Model
{
    public abstract class PlayerRow
    {
        public PlayerRow()
        {
            Stats = new List<int>();
        }

        public string Name { get; set; }
        public string TeamAcronym { get; set; }
        public bool IsSubTotal { get; set; }
        public List<int> Stats { get; set; }
    }
}
