// MsnpEngine.cs
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

namespace System.Net.Protocols.Msnp.Core
{
	
	
	public class MsnpEngine
	{
		private MsnpNotification _notification;
		private MsnpDispatch _dispatch;
		
		private string _username;
		private string _password;
		
		private event EventHandler _success;
		private event MsnpCommandArrivedHandler _commandArrived;
//		private event EventHandler _error;
		
		public MsnpEngine (string username, string password)
		{
			_username = username;
			_password = password;
			
			_notification = new MsnpNotification ();
			_notification.Success += notification_Success;
			
			_dispatch = new MsnpDispatch ();
			_dispatch.CommandArrived += dispatchCommandArrived;
		}
		
		public void Connect ()
		{
			_notification.Username = _username;
			
			Console.WriteLine ("Starting with {0}:{1}",
				_username,
				_password);
				
			_notification.Open ();
		}
		
		private void notification_Success (object sender,
			NotificationSuccessArgs args)
		{
			_notification.Close ();
			
			_dispatch.Username = _username;
			_dispatch.Password = _password;
			_dispatch.Hostname = args.Hostname;
			_dispatch.Port = args.Port;
			_dispatch.TrId = _notification.TrId;
			
			_dispatch.Open ();
		}
		
		// Handlers 
		private void dispatchCommandArrived (object sender,
			MsnpCommandArrivedArgs args)
		{
			
			//if (args.Command.Type == MsnpCommandType.
		}
		
		// Events
		
		public event MsnpCommandArrivedHandler CommandArrived {
			add { _commandArrived += value; }
			remove { _commandArrived -= value; }
		}
		
		public event EventHandler Success {
			add { _success += value; }
			remove { _success -= value; }
		}
	}
}
