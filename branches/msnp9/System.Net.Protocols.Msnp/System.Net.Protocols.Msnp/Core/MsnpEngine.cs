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
using System.Text;

namespace System.Net.Protocols.Msnp.Core
{

	public class MsnpEngine : IMsnpCommandable
	{
		private string _username;
		private string _password;
		
		private MsnpNotificationClient _notification;
		private MsnpDispatchClient _dispatch;
		
		private MsnpSwitchboard _switchboard;
		
		private event MsnpCommandArrivedHandler _commandArrived;
		private event LoginErrorHandler _loginError;
		private event MsnpMessageHandler _message_arrived;
	
		public MsnpEngine () : this (string.Empty, string.Empty)
		{
		}
	
		public MsnpEngine (string username, string password)
		{
			_username = username;
			_password = password;
			
			_commandArrived = onCommandArrived;
			_loginError = onLoginError;
			_message_arrived = onMessageArrived;
			
			_notification = new MsnpNotificationClient ();
			_dispatch = new MsnpDispatchClient ();
			
			_notification.CommandArrived += onAnyCommandArrived;
			_notification.Success += notificationSuccess;
			
		//	_notification.Disconnected += notificationDisconnected;
			
			_dispatch.CommandArrived += onAnyCommandArrived;
			_dispatch.MessageArrived += dispatchMessageArrived;
		//	_dispatch.Disconnected += dispatchDisconnected;
			_switchboard = new MsnpSwitchboard (this);
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
		
		protected virtual void OnMessageArrived (MsnpMessage message)
		{
			_message_arrived (this, new MsnpMessageArgs (message));
		}
		
		private void notificationSuccess (object sender,
			NotificationSuccessArgs args)
		{
			Console.WriteLine ("Attemp");
			_notification.Close ();
			Console.WriteLine ("Closed not");
			
			_dispatch.Username = _username;
			_dispatch.Password = _password;
			_dispatch.Hostname = args.Hostname;
			_dispatch.Port = args.Port;
			_dispatch.TrId = _notification.TrId;
			
			_dispatch.Open ();
		}
		
		private void dispatchMessageArrived (object sender,
			MsnpMessageArgs args)
		{
			OnMessageArrived (args.Message);
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
		
		private void onMessageArrived (object sender, MsnpMessageArgs args)
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
		
		public int ListVersion {
			get { return _dispatch.ListVersion; }
			protected set { _dispatch.ListVersion = value; }
		
		}

		public MsnpDispatchClient Dispatch {
			get { return _dispatch; }
		}
	
		public MsnpNotificationClient Notification {
			get { return _notification; }
		}
		
		public MsnpSwitchboard Switchboard {
				get { return _switchboard; }
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
		
		public event MsnpMessageHandler MessageArrived {
			add { _message_arrived += value; }
			remove { _message_arrived -= value; }
		}
	}
}
