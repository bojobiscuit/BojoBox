using System;
using System.Collections.Generic;
using System.Text;

namespace BojoBox.SthsDataCollector.Modern
{
    public class SkaterRow
    {
        public SkaterRow()
        {
            Stats = new List<int>();
        }

        public string Name { get; set; }
        public string TeamAcronym { get; set; }
        public bool IsSubTotal { get; set; }
        public List<int> Stats { get; set; }

        public bool IsForward { get; set; }
        public bool IsDefense { get; set; }
    }
}
