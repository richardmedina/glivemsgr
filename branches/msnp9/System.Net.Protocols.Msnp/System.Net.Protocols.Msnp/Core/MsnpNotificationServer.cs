using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Protocols.Msnp.Core
{
    public class MsnpNotificationServer : MsnpServer
    {
        private const string _hostname = "messenger.hotmail.com";
        private const int _port = 1863;

		private string _username;
        
        private int _trId = 1;
        
        private event NotificationSuccessHandler _success;
        //private event EventHandler _error;
        
        public MsnpNotificationServer () : this (string.Empty)
        {
        }
        
        public MsnpNotificationServer (string username) : base (MsnpServerType.Notification)
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
		
			Console.WriteLine ("Connected: Starting..");
		
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
		
			Console.WriteLine ("Notification: processCommand Ends");
			
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
