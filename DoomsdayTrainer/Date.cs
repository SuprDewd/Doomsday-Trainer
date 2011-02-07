using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomsdayTrainer
{
    public class Date : IFormattable
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public string ToString(string format = "d. M y", IFormatProvider provider = null)
        {
            return format.Replace("d", this.Day.ToString())
                         .Replace("y", this.Year.ToString())
                         .Replace("m", this.Month.ToString())
                         .Replace("M", MonthNames[this.Month - 1]);
        }

        private static string[] MonthNames = new[]
        {
            "january",  "february", "march",  "april",
            "may",    "june",   "july",   "august",
            "september", "october", "november", "december"
        };
    }
}