// MsnpNotificationClient.cs
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
using System.Collections.Generic;
using System.Text;

namespace System.Net.Protocols.Msnp.Core
{
	public class MsnpNotificationClient : MsnpClient
	{
		private const string _hostname = "messenger.hotmail.com";
		private const int _port = 1863;

		private string _username;
        
		private int _trId = 1;
        
		private event NotificationSuccessHandler _success;        
		//private event EventHandler _error;	
        
		public MsnpNotificationClient () : this (string.Empty)
		{
		}
        
		public MsnpNotificationClient (string username) : base (MsnpClientType.Notification)
		{
			_success = onSuccess;
			_username = username;
		
			Hostname = _hostname;
			Port = _port;
		}
                
		protected override void OnConnected ()
		{
			base.OnConnected ();
			StartAsynchronousReading ();
		
			Send ("VER {0} MSNP8 MSNP9 CVR0", TrId++);
		}

/*
        protected override void OnDataArrived(string data)
        {
			Console.WriteLine("DataArrived");
			MsnpCommand command = MsnpCommand.Parse (ServerType, data);
			//processCommand (command);
			
		
			base.OnDataArrived(data);
        }
*/        
		protected override void OnCommandArrived (MsnpCommand command)
		{

		//	Console.WriteLine ("Notification({0}):{1}", 
			//	command.Type.ToString (),
				//command.RawString);
				
			switch (command.Type) {
				case MsnpCommandType.VER:
					Send ("CVR {0} 0x0C0A winnt 5.1 i386 MSNMSGR 6.0.0602 " +
						"MSMSGS {1}", TrId ++, _username);
				break;
			
				case MsnpCommandType.CVR:
					Send ("USR {0} TWN I {1}",
						TrId ++, _username);
				break;
			
				case MsnpCommandType.XFR:
					string [] pieces = command.Arguments [1].Split (":".ToCharArray ());
					int port;
					if (int.TryParse (pieces [1], out port))
						OnSuccess (pieces [0], port);
					else 
						throw new InvalidCastException (pieces [1]);
				break;
			}
		
		//	Console.WriteLine ("Notification: processCommand Ends");
			
        		base.OnCommandArrived (command);
		}

        
		protected virtual void OnSuccess (string hostname, int port)
		{
				_success (this, 
					new NotificationSuccessArgs (hostname, port));
      	}
                
       	private void onSuccess (object sender, NotificationSuccessArgs args)
        {
        }
                
		public string Username {
			get { return _username; }
			set { _username = value; }
		}
		
		public int TrId {
			get { return _trId; }
			set { _trId = value; }
		}
	
		public event NotificationSuccessHandler Success {
			add { _success += value; }
			remove { _success -= value; }
		}
    }
}
