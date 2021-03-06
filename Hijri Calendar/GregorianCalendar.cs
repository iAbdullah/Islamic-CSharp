using System;
using System.Collections.Generic;
using System.Globalization;

namespace org.tamrah.islamic.hijri
{
	public class GregorianCalendar : Calendar
	{
		private static readonly int EPOCH_OFFSET   = 719163; // Fixed date of January 1, 1970 (Gregorian)
//		private static readonly int EPOCH_YEAR     = 1970;

		static readonly int BCE = 0;
		static readonly int CE = 1;
		/*
		 static readonly int[] MONTH_Length
		= {31,28,31,30,31,30,31,31,30,31,30,31}; // 0-based
		 static readonly int[] LEAP_MONTH_Length
		= {31,29,31,30,31,30,31,31,30,31,30,31}; // 0-based
       */
		// Useful millisecond constants.  Although ONE_DAY and ONE_WEEK can fit
		// into ints, they must be longs in order to prevent arithmetic overflow
		// when performing (bug 4173516).
		private static readonly int  ONE_SECOND = 1000;
		private static readonly int  ONE_MINUTE = 60*ONE_SECOND;
		private static readonly int  ONE_HOUR   = 60*ONE_MINUTE;
		private static readonly long ONE_DAY    = 24*ONE_HOUR;
//		private static readonly long ONE_WEEK   = 7*ONE_DAY;

		static readonly int[] MIN_VALUES = {
			BCE,            // ERA
			1,              // YEAR
			JANUARY,        // MONTH
			1,              // WEEK_OF_YEAR
			0,              // WEEK_OF_MONTH
			1,              // DAY_OF_MONTH
			1,              // DAY_OF_YEAR
			SUNDAY,         // DAY_OF_WEEK
			1,              // DAY_OF_WEEK_IN_MONTH
			AM,             // AM_PM
			0,              // HOUR
			0,              // HOUR_OF_DAY
			0,              // MINUTE
			0,              // SECOND
			0,              // MILLISECOND
			-13*ONE_HOUR,   // ZONE_OFFSET (UNIX compatibility)
			0               // DST_OFFSET
		};
		static readonly int[] LEAST_MAX_VALUES = {
			CE,             // ERA
			292269054,      // YEAR
			DECEMBER,       // MONTH
			52,             // WEEK_OF_YEAR
			4,              // WEEK_OF_MONTH
			28,             // DAY_OF_MONTH
			365,            // DAY_OF_YEAR
			SATURDAY,       // DAY_OF_WEEK
			4,              // DAY_OF_WEEK_IN
			PM,             // AM_PM
			11,             // HOUR
			23,             // HOUR_OF_DAY
			59,             // MINUTE										
			59,             // SECOND
			999,            // MILLISECOND
			14*ONE_HOUR,    // ZONE_OFFSET
			20*ONE_MINUTE   // DST_OFFSET (historical least maximum)
		};
		static readonly int[] MAX_VALUES = {
			CE,             // ERA
			292278994,      // YEAR
			DECEMBER,       // MONTH
			53,             // WEEK_OF_YEAR
			6,              // WEEK_OF_MONTH
			31,             // DAY_OF_MONTH
			366,            // DAY_OF_YEAR
			SATURDAY,       // DAY_OF_WEEK
			6,              // DAY_OF_WEEK_IN
			PM,             // AM_PM
			11,             // HOUR
			23,             // HOUR_OF_DAY
			59,             // MINUTE
			59,             // SECOND
			999,            // MILLISECOND
			14*ONE_HOUR,    // ZONE_OFFSET
			2*ONE_HOUR      // DST_OFFSET (double summer time)
		};

		// The default value of gregorianCutover.
		static readonly long DEFAULT_GREGORIAN_CUTOVER = -12219292800000L;
		/**
     * The fixed date of the gregorianCutover.
     */
		[NonSerialized]
		private long gregorianCutoverDate =
			(((DEFAULT_GREGORIAN_CUTOVER + 1)/ONE_DAY) - 1) + EPOCH_OFFSET; // == 577736
		
		/**
     * The normalized year of the gregorianCutover in Gregorian, with
     * 0 representing 1 BCE, -1 representing 2 BCE, etc.
     */
		[NonSerialized]
		private int gregorianCutoverYear = 1582;
		
		/**
     * The normalized year of the gregorianCutover in Julian, with 0
     * representing 1 BCE, -1 representing 2 BCE, etc.
     */
		[NonSerialized]
		private int gregorianCutoverYearJulian = 1582;

		public Boolean isLeapYear(int year) {
			if ((year & 3) != 0) {
				return false;
			}
			
			if (year > gregorianCutoverYear) {
				return (year%100 != 0) || (year%400 == 0); // Gregorian
			}
			if (year < gregorianCutoverYearJulian) {
				return true; // Julian
			}
			Boolean gregorian;
			// If the given year is the Gregorian cutover year, we need to
			// determine which calendar system to be applied to February in the year.
			if (gregorianCutoverYear == gregorianCutoverYearJulian) {
				DateTime d =Convert.ToDateTime(gregorianCutoverDate); // Gregorian
				gregorian = d.Month < MARCH;
			} else {
				gregorian = year == gregorianCutoverYear;
			}
			return gregorian ? (year%100 != 0) || (year%400 == 0) : true;
		}

		public GregorianCalendar ()
		{
			DateTime dateTime = DateTime.Now;
			setFromDateTime (dateTime);
		}

		public override void add(int field, int amount)
		{
			if(field >= FIELD_COUNT)
				throw new Exception();
			DateTime dt = new DateTime (get(YEAR),get(MONTH)+1,get(DATE),get(HOUR),get(MINUTE),get(SECOND));

			switch(field){
			case YEAR:
				dt = dt.AddYears(amount);
				break;
			case MONTH:
				dt = dt.AddMonths(amount);
				break;
			case DATE:
				dt = dt.AddDays(amount);
				break;
			case DAY_OF_YEAR:
				dt = dt.AddDays(amount);
				break;
			case HOUR:
				dt = dt.AddHours(amount);
				break;
			case MINUTE:
				dt = dt.AddMinutes(amount);
				break;
			case SECOND:
				dt = dt.AddSeconds(amount);
				break;
			}
			setFromDateTime (dt);
		}

		private void setFromDateTime(DateTime dateTime)
		{
			//
			if(dateTime.Year > 0)
				set(ERA, AD);
			else
				set(ERA, BC);
			//
			set(YEAR, dateTime.Year);
			//
			set(MONTH, dateTime.Month - 1);
			//
			set(DATE, dateTime.Day);
			//
			set(DAY_OF_MONTH, dateTime.Day);
			//DAY_OF_YEAR
			set(DAY_OF_YEAR, dateTime.DayOfYear);
			//
			set(DAY_OF_WEEK, getDayOfWeek(dateTime.DayOfWeek));
			//
			set(HOUR, dateTime.Hour);
			set(MINUTE, dateTime.Minute);
			set(SECOND, dateTime.Second);
			set(MILLISECOND, dateTime.Millisecond);
		}
		public override int getMaximum(int field)
		{
			return isLeapYear(get(YEAR))?LEAST_MAX_VALUES[field]:MAX_VALUES[field];
		}
		
		public override int getMinimum(int field)
		{
			return MIN_VALUES [field];
		}
		
		public override int getActualMaximum(int field)
		{
			return getMaximum(field);
		}


	}
}

