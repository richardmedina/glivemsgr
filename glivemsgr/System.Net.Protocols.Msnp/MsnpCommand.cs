// MsnpCommand.cs created with MonoDevelop
// User: ricki at 01:28 01/27/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpCommand
	{
		private MsnpCommandType _type;
		private int _trId;
		private string [] _arguments;
		
		private string _rawString;
		
		private MsnpCommand () : 
			this (MsnpCommandType.Unknown, 
				0, 
				string.Empty, 
				new string [0])
		{
		}
		
		private MsnpCommand (
			MsnpCommandType type,
			int trId,
			string rawString,
			params string [] arguments)
		{
			_type = type;
			_trId = trId;
			_arguments = arguments;
			_rawString = rawString; 
		}
		
		public static MsnpCommand CreateFromString (string commandString)
		{
			MsnpCommand command = new MsnpCommand ();
			command.RawString = commandString;
		
			string [] pieces = 
			 	commandString.Split (" ".ToCharArray ());
			 
			 // arguments start index
			 int startIndex = 2; 
			 
			 if (pieces [0] == "CVR")
			 	command.Type = MsnpCommandType.CVR;
			 else if (pieces [0] == "USR")
			 	command.Type = MsnpCommandType.USR;
			 else if (pieces [0] == "SYN")
			 	command.Type = MsnpCommandType.SYN;
			 else if (pieces [0] == "LSG") {
			 	command.Type = MsnpCommandType.LSG;
			 	startIndex = 1;
			 }
			 else if (pieces [0] == "LST") {
			 	command.Type = MsnpCommandType.LST;
			 	startIndex = 1; 
			 }
			 else if (pieces [0] == "CHG")
			 	command.Type = MsnpCommandType.CHG;
			 else if (pieces [0] == "FLN")
			 	command.Type = MsnpCommandType.FLN;
			 else if (pieces [0] == "ILN")
			 	command.Type = MsnpCommandType.ILN;
			 else if (pieces [0] == "NLN")
			 	command.Type = MsnpCommandType.NLN;
			 else if (pieces [0] == "CHL")
			 	command.Type = MsnpCommandType.CHL;
			 else if (pieces [0] == "RNG") {
			 	command.Type = MsnpCommandType.RNG;
			 	startIndex = 1;
			 }
			 else if (pieces [0] == "REA")
			 	command.Type = MsnpCommandType.REA;
			 else if (pieces [0] == "XFR") {
			 	command.Type = MsnpCommandType.XFR;
			 	int id;
			 	if (int.TryParse (pieces [1], out id))
			 		command.TrId = id;
			 }
			 
			 
			 if (command.Type != MsnpCommandType.Unknown) {
			 	command.Arguments = new string [pieces.Length - startIndex];
			 	for (int i = startIndex; i < pieces.Length; i ++)
			 		command.Arguments [i - startIndex] = pieces [i];
			 }
			 
			 return command;
		}
		
		public override string ToString ()
		{
			return RawString;
		}

		
		public MsnpCommandType Type {
			get { return _type; }
			private set { _type = value; }
		}
		
		public int TrId {
			get { return _trId; }
			private  set { _trId = value; }
		}
		
		public string [] Arguments {
			get { return _arguments; }
			private set { _arguments = value; }
		}
		
		public string RawString {
			get { return _rawString; }
			private set { _rawString = value; }
		}
	}
}
