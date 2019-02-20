using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using BojoBox.SthsDataCollector.Model;

namespace BojoBox.SthsDataCollector.Modern
{
    internal class SkaterGroup
    {
        public IEnumerable<HtmlNode> sectionA;
        public IEnumerable<HtmlNode> sectionB;
    }
}
