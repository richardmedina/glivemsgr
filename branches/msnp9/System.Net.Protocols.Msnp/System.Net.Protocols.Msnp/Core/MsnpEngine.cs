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

	public class MsnpEngine : IMsnpCommandable
	{
		private string _username;
		private string _password;
		
		private MsnpNotificationServer _notification;
		private MsnpDispatchServer _dispatch;
		
		private Switchboard _switchboard;
		
		private event MsnpCommandArrivedHandler _commandArrived;
		private event LoginErrorHandler _loginError;
	
		public MsnpEngine () : this (string.Empty, string.Empty)
		{
		}
	
		public MsnpEngine (string username, string password)
		{
			_username = username;
			_password = password;
			
			_commandArrived = onCommandArrived;
			_loginError = onLoginError;
			
			_notification = new MsnpNotificationServer ();
			_dispatch = new MsnpDispatchServer ();
			
			_notification.CommandArrived += onAnyCommandArrived;
			_notification.Success += notificationSuccess;
			
		//	_notification.Disconnected += notificationDisconnected;
			
			_dispatch.CommandArrived += onAnyCommandArrived;
		//	_dispatch.Disconnected += dispatchDisconnected;
			_switchboard = new Switchboard (this);
		}
		
		public void Connect ()
		{
			_notification.Username = Username;
			_notification.Open ();
		}
		
		protected virtual void OnCommandArrived (MsnpCommand command)
		{
			_commandArrived (this, new MsnpCommandArrivedArgs (command));
		}
		
		protected virtual void OnLoginError (int error_code)
		{
			_loginError (this, new LoginErrorArgs (error_code));
		}
		
		private void notificationSuccess (object sender,
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
		
		private void onAnyCommandArrived (object sender,
			MsnpCommandArrivedArgs args)
		{
			OnCommandArrived (args.Command);
		}
		
		private void onCommandArrived (object sender, 
			MsnpCommandArrivedArgs args)
		{
		}
		
		private void onLoginError (object sender, LoginErrorArgs args)
		{
		}

		public string Username {
			get { return _username; }
			set { _username = value; }
		}
		
		public string Password {
			get { return _password; }
			set { _password = value; }
		}
		
		// Signals
		
		public event MsnpCommandArrivedHandler CommandArrived {
			add { _commandArrived += value; }
			remove { _commandArrived -= value; }
		}
		
		public event LoginErrorHandler LoginError {
			add { _loginError += value; }
			remove { _loginError -= value; }
		}

		public MsnpDispatchServer Dispatch {
			get { return _dispatch; }
		}
	
		protected MsnpNotificationServer Notification {
			get { return _notification; }
		}
	}
}
