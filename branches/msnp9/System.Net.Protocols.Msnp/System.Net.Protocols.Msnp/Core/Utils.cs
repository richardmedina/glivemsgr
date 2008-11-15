// Utils.cs
//
// Copyright (c) 2008 Ricardo Medina <ricki@dana-ide.org>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

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
