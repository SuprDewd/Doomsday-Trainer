using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoomsdayTrainer
{
	public static class Doomsday
	{
		public static int CenturyAnchorFor(int year)
		{
			int c = Century(year);
			return (((5 * c + ((c - 1) / 4)) % 7) + 4) % 7;
		}

		public static int DoomsdayFor(int year)
		{
			return DoomsdayFor(year, CenturyAnchorFor(year));
		}

		public static int DoomsdayFor(int year, int anchor)
		{
			int y = year - (year / 100) * 100;
			int ym12 = y % 12;
			return ((((y / 12) + ym12 + (ym12 / 4)) % 7) + anchor) % 7;
		}

		public static DayOfWeek DayOfWeekFor(int year, int month, int date)
		{
			int doomsday = DoomsdayFor(year), sameDay = SameDayFor(year, month);

			// This can be optimized...
			if (date < sameDay) while (date < sameDay) date += 7;
			else if (date > sameDay) while (date - 7 >= sameDay) date -= 7;

			while (sameDay < date)
			{
				sameDay++;
				doomsday = (doomsday + 1) % 7;
			}

			return ToWeekday(doomsday);
		}

		public static int SameDayFor(int year, int month)
		{
			bool l = year % 400 == 0 || (year % 100 != 0 && year % 4 == 0);
			switch (month)
			{
				case 1: return l ? 11 : 10;
				case 2: return l ? 29 : 28;
				case 5: return 9;
				case 7: return 11;
				case 9: return 5;
				case 3:
				case 11: return 7;
				default: return month;
			}
		}

		public static int Century(int year)
		{
			return (year / 100) + 1;
		}

		public static DayOfWeek ToWeekday(int weekday)
		{
			switch (weekday)
			{
				case 0: return DayOfWeek.Sunday;
				case 1: return DayOfWeek.Monday;
				case 2: return DayOfWeek.Tuesday;
				case 3: return DayOfWeek.Wednesday;
				case 4: return DayOfWeek.Thursday;
				case 5: return DayOfWeek.Friday;
				case 6: return DayOfWeek.Saturday;
				default: return 0;
			}
		}
	}
}