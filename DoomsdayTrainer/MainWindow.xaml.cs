using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoomsdayTrainer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string QuestionString = "Which weekday is {0:d. M y}?";
		private const string ErrorString = "Sorry. {0:d. M y} is on a {1}.";

		private string WeekDayToString(DayOfWeek d)
		{
			switch (d)
			{
				case DayOfWeek.Sunday: return "sunday";
				case DayOfWeek.Monday: return "monday";
				case DayOfWeek.Tuesday: return "tuesday";
				case DayOfWeek.Wednesday: return "wednesday";
				case DayOfWeek.Thursday: return "thursday";
				case DayOfWeek.Friday: return "friday";
				case DayOfWeek.Saturday: return "saturday";
				default: return null;
			}
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		private Random Randomizr = new Random();

		public Date RandomDate(DoomsdayDifficulty difficulty)
		{
			bool janOrFeb = (difficulty & DoomsdayDifficulty.WithJanOrFeb) == DoomsdayDifficulty.WithJanOrFeb;
			difficulty ^= DoomsdayDifficulty.WithJanOrFeb;
			int month = this.RandomMonth(janOrFeb), year;

			switch (difficulty)
			{
				case DoomsdayDifficulty.YearRange1: year = Randomizr.Next(DateTime.Now.Year - 1, DateTime.Now.Year + 2); break;
				case DoomsdayDifficulty.YearRange2: year = Randomizr.Next(DateTime.Now.Year - 2, DateTime.Now.Year + 3); break;
				case DoomsdayDifficulty.YearRange3: year = Randomizr.Next(DateTime.Now.Year - 3, DateTime.Now.Year + 4); break;
				case DoomsdayDifficulty.YearRange4: year = Randomizr.Next(DateTime.Now.Year - 4, DateTime.Now.Year + 5); break;
				case DoomsdayDifficulty.CurrentCentury: year = Randomizr.Next((DateTime.Now.Year / 100) * 100, ((DateTime.Now.Year / 100) * 100) + 100); break;
				case DoomsdayDifficulty.AnyCentury: year = Randomizr.Next(1, 100001); break;
				case DoomsdayDifficulty.CurrentYear:
				default: year = DateTime.Now.Year; break;
			}

			return new Date { Year = year, Month = month, Day = this.RandomDay(month, this.IsLeapYear(year)) };
		}

		public bool IsLeapYear(int year)
		{
			return year % 400 == 0 || (year % 100 != 0 && year % 4 == 0);
		}

		public int RandomMonth(bool janOrFeb = true)
		{
			return Randomizr.Next(janOrFeb ? 1 : 3, 13);
		}

		public int RandomDay(int month, bool leapYear)
		{
			switch (month)
			{
				case 4:
				case 6:
				case 9:
				case 11: return Randomizr.Next(1, 31);
				case 2: return Randomizr.Next(1, leapYear ? 30 : 29);
				default: return Randomizr.Next(1, 32);
			}
		}

		private void AnswerSubmitted(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				this.txtAnswer.Foreground = Brushes.Black;
				this.Error.Text = this.txtAnswer.Text = "";
			}

			if (e.Key != Key.Enter) return;
			string answer;
			if (this.txtAnswer.Text.ToLower() == (answer = WeekDayToString(Doomsday.DayOfWeekFor(this.CurrentDate.Year, this.CurrentDate.Month, this.CurrentDate.Day)))) Environment.Exit(0);
			else
			{
				Date dt = this.CurrentDate;
				this.txtAnswer.Text = "";
				this.DisplayNewDate();
				this.Error.Text = String.Format(ErrorString, dt, answer);
			}
		}

		private Date CurrentDate { get; set; }

		private void DisplayNewDate()
		{
			this.CurrentDate = this.RandomDate((DoomsdayDifficulty)this.Difficulties.SelectedValue);
			this.lblQuestion.Content = String.Format(QuestionString, this.CurrentDate);
			this.txtAnswer.Text = "";
			this.Error.Text = "";
		}

		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			this.txtAnswer.Focus();

			Dictionary<string, DoomsdayDifficulty> diffs = new Dictionary<string, DoomsdayDifficulty>
			{
				{"Difficulty 1", DoomsdayDifficulty.CurrentYear},
				{"Difficulty 2", DoomsdayDifficulty.CurrentYear | DoomsdayDifficulty.WithJanOrFeb },
				{"Difficulty 3", DoomsdayDifficulty.YearRange1 | DoomsdayDifficulty.WithJanOrFeb },
				{"Difficulty 4", DoomsdayDifficulty.YearRange2 | DoomsdayDifficulty.WithJanOrFeb },
				{"Difficulty 5", DoomsdayDifficulty.YearRange3 | DoomsdayDifficulty.WithJanOrFeb },
				{"Difficulty 6", DoomsdayDifficulty.YearRange4 | DoomsdayDifficulty.WithJanOrFeb },
				{"Difficulty 7", DoomsdayDifficulty.CurrentCentury | DoomsdayDifficulty.WithJanOrFeb },
				{"Difficulty 8", DoomsdayDifficulty.AnyCentury | DoomsdayDifficulty.WithJanOrFeb }
			};

			foreach (var i in diffs)
			{
				this.Difficulties.Items.Add(i);
			}

			this.Difficulties.SelectedIndex = 2;

			this.DisplayNewDate();
			this.Activate();
		}

		private void NewDateClicked(object sender, RoutedEventArgs e)
		{
			this.DisplayNewDate();
		}
	}
}