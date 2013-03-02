using System;
using org.tamrah.islamic.hijri;

namespace HijraCalendar
{
	public class dummy
	{

		public static void Main ()
		{
		
			String[] weekDays = {"SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"};

			Calendar calendar = Calendar.getInstance ();

			DateTime dt = new DateTime (calendar.get(Calendar.YEAR),calendar.get(Calendar.MONTH)+1,calendar.get(Calendar.DATE));



			Console.WriteLine ( weekDays[calendar.get (Calendar.DAY_OF_WEEK)] + ", " +calendar.get (Calendar.YEAR) + "-" + calendar.get (Calendar.MONTH) + "-" + calendar.get (Calendar.DATE));

			calendar.add(Calendar.DATE,52);
			Console.WriteLine ( weekDays[calendar.get (Calendar.DAY_OF_WEEK)] + ", " +calendar.get (Calendar.YEAR) + "-" + calendar.get (Calendar.MONTH) + "-" + calendar.get (Calendar.DATE));

			Console.WriteLine ("DateTime");
			Console.WriteLine (dt.Year + "-" + dt.Month + "-" + dt.Day);

			dt = dt.AddDays (52);

			Console.WriteLine (dt.Year + "-" + dt.Month + "-" + dt.Day);

			Console.WriteLine ("Hijra Calendar");

			org.tamrah.islamic.hijri.HijraCalendar hCalendar = org.tamrah.islamic.hijri.HijraCalendar.getInstance();

			Console.WriteLine ( weekDays[hCalendar.get (Calendar.DAY_OF_WEEK)] + ", " + hCalendar.get (Calendar.YEAR) + "-" + hCalendar.get (Calendar.MONTH) + "-" + hCalendar.get (Calendar.DATE));
			hCalendar.add(Calendar.DATE,-18);
			Console.WriteLine ( weekDays[hCalendar.get (Calendar.DAY_OF_WEEK)] + ", " +hCalendar.get (Calendar.YEAR) + "-" + hCalendar.get (Calendar.MONTH) + "-" + hCalendar.get (Calendar.DATE));



		}
	}
}

