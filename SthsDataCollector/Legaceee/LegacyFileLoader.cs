using BojoBox.SthsDataCollector.Modern;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BojoBox.SthsDataCollector.Legaceee
{
    public class LegacyFileLoader
    {
        public LegacyFileLoader(SeasonData season)
        {
            this.season = season;
        }

        public HtmlDocument LoadFile()
        {
            HtmlDocument htmlDocument;

            var nameFormat = (season.IsPlayoffs) ?
                "{1} - PLF - Pro Team Scoring {0}.html" :
                "{1} - Pro Team Scoring {0}.html";

            var folderName = @"LegacyFiles";

            string seasonNumberText = (season.SeasonNumber < 10 ? "0" : "") + season.SeasonNumber.ToString();
            string fileName = string.Format(nameFormat, seasonNumberText, season.LeagueAcronym);
            string directory = Path.Combine(Environment.CurrentDirectory, folderName);
            string filePath = Path.Combine(directory, fileName);

            Directory.CreateDirectory(directory);
            if (!File.Exists(filePath))
            {
                throw new Exception("Page does not exist");
            }

            htmlDocument = new HtmlDocument();
            htmlDocument.Load(filePath);

            return htmlDocument;
        }

        private SeasonData season;
    }
}
