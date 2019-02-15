using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BojoBox.SthsDataCollector.Moderno
{
    public static class Helper
    {
        public static int GetPercentageAmount(double percent, int total)
        {
            double factor = percent / 100;
            return (int)Math.Round(total * factor);
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

    }
}
