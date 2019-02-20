using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BojoBox.SthsDataCollector.Model;

namespace BojoBox.SthsDataCollector
{
    internal static class Helper
    {
        public static string GetName(string nameValue)
        {
            var name = nameValue;
            name = name.RemoveAcronyms();
            name = name.Replace("_", "");
            name = name.Trim();
            return name;
        }

        public static void GetPlayerTeam(string teamAcronym, IEnumerable<string> nameAcronyms, PlayerRow playerRow)
        {
            if (playerRow.TeamAcronym == null)
                playerRow.TeamAcronym = teamAcronym;

            if (nameAcronyms.Contains("TOT"))
                playerRow.TeamAcronym = null;

            if (PreviousName == playerRow.Name)
                playerRow.IsSubTotal = true;

            PreviousName = playerRow.Name;
        }

        public static int GetPercentageAmount(double percent, int total)
        {
            double factor = percent / 100;
            return (int)Math.Round(total * factor);
        }

        public static int GetPenaltyShotsSaved(string shotPercentage, int penaltyShotsTotal)
        {
            var shotsValue = shotPercentage.Replace("%", "");
            var shotsDouble = double.Parse(shotsValue);
            var shotsStopped = (int)Math.Round(shotsDouble * penaltyShotsTotal);
            return shotsStopped;
        }

        public static int GetFaceoffsWon(string faceoffPercentage, int faceoffTotal)
        {
            var faceoffValue = faceoffPercentage.Replace("%", "");
            var faceoffDouble = double.Parse(faceoffValue);
            var faceoffsWon = Helper.GetPercentageAmount(faceoffDouble, faceoffTotal);
            return faceoffsWon;
        }

        public static List<int> GetStats(IEnumerable<string> rowValues, int[] skipColumns)
        {
            List<int> stats = new List<int>();
            for (int i = 0; i < rowValues.Count(); i++)
            {
                if (skipColumns.Contains(i))
                    continue;

                stats.Add(int.Parse(rowValues.ElementAt(i)));
            }
            return stats;
        }

        public static string RemoveAcronyms(this string text)
        {
            List<string> acronymList = new List<string>();
            int leftIndex = 0, rightIndex = 0;
            while (true)
            {
                leftIndex = text.IndexOf('(', rightIndex);
                if (leftIndex <= 0) break;

                rightIndex = text.IndexOf(')', leftIndex) + 1;
                int length = rightIndex - leftIndex;
                acronymList.Add(text.Substring(leftIndex, length));
            }

            foreach (string acronym in acronymList)
                text = text.Replace(acronym, "");

            return text.Trim();
        }

        public static string SplitCamelCase(this string text)
        {
            text = text.Replace(".", "");

            string strRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";
            Regex myRegex = new Regex(strRegex, RegexOptions.None);

            string strReplace = @" $1$2";
            return myRegex.Replace(text, strReplace).Trim();
        }

        public static string GetTeamAcronym(IEnumerable<string> acronyms)
        {
            return acronyms.Where(acro => acro.Length == 3 && acro != "TOT").FirstOrDefault();
        }

        public static IEnumerable<string> GetAcronymns(this string text)
        {
            List<string> acronymList = new List<string>();
            int leftIndex = 0, rightIndex = 0;
            while (true)
            {
                leftIndex = text.IndexOf('(', rightIndex) + 1;
                if (leftIndex <= 0) break;

                rightIndex = text.IndexOf(')', leftIndex);
                int length = rightIndex - leftIndex;
                acronymList.Add(text.Substring(leftIndex, length));
            }

            return acronymList;
        }

        public static string PreviousName = "";
    }
}
