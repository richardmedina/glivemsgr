// SBSession.cs
// 
// Copyright (C) 2008 Ricardo Medina <ricki@dana-ide.org>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net.Protocols.Msnp.Core
{
	
	
	public class MsnpSBSession : MsnpClient
	{
		private string _owner;
		
		private MsnpCommand _command;
		
		private event EventHandler _opened;
		private event EventHandler _closed;
		
		// Must be us username
		private string _username;
		
		private System.Collections.Generic.List<string> _members;
		
		private string id1;
		private string id2;
		
		public MsnpSBSession (MsnpCommand command) : base (MsnpClientType.Switchboard)
		{
				_command = command;
				// index 4 is the username who calling..
				Owner = command.Arguments [4];
				_opened = onOpened;
				_closed = onClosed;
				_members = new List<string> ();
		}
		
		public new void Open ()
		{
			//RNG 38695310 64.4.36.44:1863 CKI 20799108.88152118 karl113@hotmail.com Ricki.
			if (_command.Type == MsnpCommandType.RNG) {
					// first element at argument array. first coltrol identifier 
					id1 = _command.Arguments [0];
					
					// second element in argument array has address and port of SBsession ("hostname:port" like)
					string [] address = _command.Arguments [1].Split (":".ToCharArray ());
					string hostname = address [0];
					int port;
					if (!int.TryParse (address [1], out port))
						throw new ArgumentException ("MsnpSBSession.Open()");
						
					// fourth element at argument array. second control identifier
					id2 = _command.Arguments [3];
					
					Hostname = hostname;
					Port = port;
					
					base.Open ();
			}
		}
		
		protected virtual void OnOpened ()
		{
			_opened (this, EventArgs.Empty);
		}
		
		
		protected virtual void OnClosed ()
		{
			_closed (this, EventArgs.Empty);
		}
		
		protected override void OnConnected ()
		{
			base.OnConnected ();
			
			StartAsynchronousReading ();
			
			if (_command.Type == MsnpCommandType.RNG)
				Send ("ANS 1 {0} {1} {2}\r\n", Username, id2, id1);
		}
		
		protected override void OnCommandArrived (MsnpCommand command)
		{
			
			if (command.Type == MsnpCommandType.IRO) {
				// position 2 in command is username of member
				_members.Add (command.Arguments [2]);
				// This action must be performed in order to process next command, some bug here!
				Read ();
			}
			if (command.Type == MsnpCommandType.ANS)
				OnOpened ();
			
			base.OnCommandArrived (command);
		}
		
		protected override void OnDataArrived (string data)
		{
			base.OnDataArrived (data);
		}

		
		protected override void OnMessageArrived (MsnpMessage message)
		{
			Console.WriteLine ("{0}({1})>\n{2}",message.Command, message.Body.Length, message.Body);
			base.OnMessageArrived (message);
		}

		private void onOpened (object sender, EventArgs args)
		{
		}
		
		private void onClosed (object sender, EventArgs args)
		{
		}

		public string Username {
			get { return _username; }	
			set { _username = value; }
		}
		public string Owner {
			get { return _owner; }
			set { _owner = value; }
		}
		
		public event EventHandler Opened {
			add { _opened += value; }
			remove { _opened -= value; }
		}
		public event EventHandler Closed {
			add { _closed += value; }
			remove { _closed -= value; }
		}
	}
}
