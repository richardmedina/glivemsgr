// MsnpCommand.cs
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

namespace System.Net.Protocols.Msnp.Core
{
	
	public class MsnpCommand
	{
		private MsnpCommandType _type;
		private int _trId;
		private string [] _arguments;
		private MsnpClientType _serverType;

		private string _rawString;

		private MsnpCommand() :
			this(MsnpCommandType.Unknown,
			       0,
			       string.Empty,
			       new string[0])
		{
		}

		private MsnpCommand(
			MsnpCommandType type,
			int trId,
			string rawString,
			params string[] arguments)
		{
			_type = type;
			_trId = trId;
			_arguments = arguments;
			_rawString = rawString;
		}
		
		public static MsnpCommand Parse (string command)
		{
			return Parse (MsnpClientType.Dispatch, command);
		}

		public static MsnpCommand Parse (MsnpClientType server_type, string commandString)
		{
			MsnpCommand command = new MsnpCommand();
			command._serverType = server_type;
			command.RawString = commandString;

			string[] pieces =
				commandString.Split(" ".ToCharArray());

			// arguments start index
			int startIndex = 2;

			if (pieces[0] == "CVR")
				command.Type = MsnpCommandType.CVR;
			else if (pieces [0] == "VER")
				command.Type = MsnpCommandType.VER;
			else if (pieces[0] == "USR")
				command.Type = MsnpCommandType.USR;
			else if (pieces[0] == "SYN")
				command.Type = MsnpCommandType.SYN;
			else if (pieces[0] == "LSG") {
				command.Type = MsnpCommandType.LSG;
				startIndex = 1;
			}
			else if (pieces[0] == "MSG") {
				command.Type = MsnpCommandType.MSG;
				startIndex = 1;
			}
			else if (pieces[0] == "LST") {
				command.Type = MsnpCommandType.LST;
				startIndex = 1;
			}
			else if (pieces[0] == "PNG") {
				command.Type = MsnpCommandType.PNG;
				startIndex = 1;
			}
			else if (pieces[0] == "CHG")
				command.Type = MsnpCommandType.CHG;
			else if (pieces[0] == "FLN")
				command.Type = MsnpCommandType.FLN;
			else if (pieces[0] == "ILN")
				command.Type = MsnpCommandType.ILN;
			else if (pieces[0] == "NLN")
				command.Type = MsnpCommandType.NLN;
			else if (pieces[0] == "CHL")
				command.Type = MsnpCommandType.CHL;
			else if (pieces[0] == "RNG") {
				command.Type = MsnpCommandType.RNG;
				startIndex = 1;
			}
			else if (pieces[0] == "REA")
				command.Type = MsnpCommandType.REA;
			else if (pieces[0] == "XFR") {
				command.Type = MsnpCommandType.XFR;
				int id;
				if (int.TryParse(pieces[1], out id))
					command.TrId = id;
			}


			if (command.Type != MsnpCommandType.Unknown)
			{
				command.Arguments = new string[pieces.Length - startIndex];
				for (int i = startIndex; i < pieces.Length; i++)
					command.Arguments[i - startIndex] = pieces[i];
			}

			return command;
		}

		public override string ToString()
		{
			return RawString;
		}


		public MsnpCommandType Type
		{
			get { return _type; }
			private set { _type = value; }
		}

		public int TrId
		{
			get { return _trId; }
			private set { _trId = value; }
		}

		public string[] Arguments
		{
			get { return _arguments; }
			private set { _arguments = value; }
		}

		public string RawString
		{
			get { return _rawString; }
			private set { _rawString = value; }
		}
		
		public MsnpClientType ServerType {
			get { return _serverType; }
		}
	}
}
