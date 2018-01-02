using System;
using System.Collections.Generic;

namespace badmintoncourtmanager.Services
{
    public static class Util
    {
		public static string AsShortRelativeDate(this DateTime date, bool dateIsAlreadyInUtc = false)
		{
			DateTime utcDate = dateIsAlreadyInUtc ? date : date.ToUniversalTime();
			var ts = new TimeSpan(Math.Max(0, DateTime.UtcNow.Ticks - utcDate.Ticks));
			double delta = Math.Abs(ts.TotalSeconds);

			int SECOND = 1;
			int MINUTE = 60 * SECOND;
			int HOUR = 60 * MINUTE;
			int DAY = 24 * HOUR;
			int MONTH = 30 * DAY;

			if (delta < 1)
			{
				return "Just now";
			}
			if (delta < 1 * MINUTE)
			{
				return ts.Seconds + "s";
			}
			if (delta < 60 * MINUTE)
			{
				return ts.Minutes + "m";
			}
			if (delta < 24 * HOUR)
			{
				return ts.Hours + "h";
			}
			if (delta < 48 * HOUR)
			{
				return "Yesterday";
			}
			if (delta < 30 * DAY)
			{
				return ts.Days + "d";
			}
			if (delta < 12 * MONTH)
			{
				int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
				return months <= 1 ? "One month ago" : months + " months ago";
			}
			else
			{
				int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
				return years <= 1 ? "One year ago" : years + " years ago";
			}
		}


		
    }
}
