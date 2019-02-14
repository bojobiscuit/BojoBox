using System;
using System.Collections.Generic;
using System.Text;

namespace BojoBox.SthsDataCollector.Moderno
{
    public class SkaterRow
    {
        public string Name { get; set; }
        public string TeamAcronym { get; set; }
        public bool IsSubtotal { get; set; }
        public bool IsForward { get; set; }
        public bool IsDefense { get; set; }
        public List<int> Stats { get; set; }
    }
}
