
using System;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace System.Net.Protocols.Msnp.Core
{

	public static class Utils
	{
		private static string[] states = {
			"NLN",
			"BSY",
			"BRB",
			"AWY",
			"PHN",
			"LUN",
			"HDN"
		};

		public static string ContactStateToString(int state)
		{
			return states [state];
		}

		public static int StringToContactState(string state)
		{
			for (int i = 0; i < states.Length; i++)
				if (state == states[i])
					return i;

			Console.WriteLine("{0} not found!", state);
			return states.Length -1;
		}

		// TODO: This will be necesary
		public static string CommandTypeToString(MsnpCommandType command)
		{
			return string.Empty;
		}
		// TODO: too
		public static MsnpCommandType StringToCommandType(string command)
		{
			return (MsnpCommandType)0; //MsnpCommandType.CVR;
		}

		public static string UrlEncode(string text)
		{
			return HttpUtility.UrlPathEncode(text);
		}

		public static string UrlDecode(string text)
		{
			return HttpUtility.UrlDecode(text);
		}

		public static string MD5Sum(string input)
		{
			MD5 md5 = new MD5CryptoServiceProvider(); //MD5.Create ();

			byte[] bytes = Encoding.Default.GetBytes(input);
			byte[] result = md5.ComputeHash(bytes);

			string output = string.Empty;

			foreach (byte b in result)
				output += b.ToString("x2");

			return output;
		}

		public static byte[] SHA1(byte[] input)
		{
			SHA1 sha1 = new SHA1CryptoServiceProvider();

			return sha1.ComputeHash(input);
		}

		public static string Base64(byte[] input)
		{
			return System.Convert.ToBase64String(input);
		}
	}
}
