namespace QLess.Core.Helpers
{
	public static class DateTimeHelperExtensions
	{
		public static DateTime GetDayStartDateTime(this DateTime targetDate)
		{
			return new DateTime(targetDate.Year, targetDate.Month,
				targetDate.Day, 0, 0, 0);
		}

		public static DateTime GetDayEndDateTime(this DateTime targetDate)
		{
			return new DateTime(targetDate.Year, targetDate.Month,
				targetDate.Day, 23, 59, 59);
		}
	}
}
