
using System;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace System.Net.Protocols.Msnp
{

	public static class Utils
	{
		private static string [] states = {
			"NLN",
			"BSY",
			"BRB",
			"AWY",
			"PHN",
			"LUN",
			"HDN"
		};
		
		public static string ContactStateToString (MsnpContactState state)
		{
		/*
			switch (state) {
				case MsnpContactState.Offline:
					return "HDN";
				case MsnpContactState.Online:
					return "NLN";
				case MsnpContactState.Away:
					return "AWY";
				case MsnpContactState.Brb:
					return "BRB";
				case MsnpContactState.Bussy:
					return "BSY";
				case MsnpContactState.Lunch:
					return "LUN";
				case MsnpContactState.Phone:
					return "PHN";
			}
			return "HDN";
		*/
			return states [(int) state];
				
		
		}
		
		public static MsnpContactState StringToContactState (string state)
		{
			for (int i = 0; i < states.Length; i ++)
				if (state == states [i])
					return (MsnpContactState) i;
			
			Console.WriteLine ("{0} not found!", state);
			return MsnpContactState.Offline;
		}
		
		// TODO: This will be necesary
		public static string CommandTypeToString (MsnpCommandType command)
		{
			return string.Empty;
		}
		// TODO: too
		public static MsnpCommandType StringToCommandType (string command)
		{
			return (MsnpCommandType) 0; //MsnpCommandType.CVR;
		}
		
		public static string UrlEncode (string text)
		{
			return HttpUtility.UrlEncode (text);
		}
		
		public static string UrlDecode (string text)
		{
			return HttpUtility.UrlDecode (text);
		}

		public static string MD5Sum (string input)
		{
			MD5 md5 = MD5.Create ();
			byte [] bytes = Encoding.Default.GetBytes (input);
			byte [] result = md5.ComputeHash (bytes);
			
			string output = string.Empty;
			
			foreach (byte b in result)
				output += b.ToString ("x2");
			
			return output;
		}
	}
}
