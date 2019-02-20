using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using BojoBox.SthsDataCollector.Model;

namespace BojoBox.SthsDataCollector.Modern
{
    internal class TeamSection
    {
        public HtmlNode teamHeader;
        public SkaterGroup skaterGroup;
        public IEnumerable<HtmlNode> goalieRows;
    }

}
