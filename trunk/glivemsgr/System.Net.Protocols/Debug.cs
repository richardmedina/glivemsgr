using System;

namespace System.Net.Protocols
{
	
	
	public static class Debug
	{
		public static bool Enable = false;
		
		public static void WriteLine (object format, params object [] objs)
		{
			if (Enable)
				Console.WriteLine (format.ToString (), objs);
		}
		
		public static void Write (object format, params object [] objs)
		{
			if (Enable)
				Console.Write (format.ToString (), objs);
		}
	}
}
