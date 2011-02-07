using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomsdayTrainer
{
    public enum DoomsdayDifficulty
    {
        WithJanOrFeb = 1,
        CurrentYear = 2,
        YearRange1 = 4,
        YearRange2 = 8,
        YearRange3 = 16,
        YearRange4 = 32,
        CurrentCentury = 64,
        AnyCentury = 128
    }
}