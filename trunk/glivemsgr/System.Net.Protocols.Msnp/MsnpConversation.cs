// MsnpBetaConversation.cs created with MonoDevelop
// User: ricki at 19:14 01/20/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Net.Protocols;


namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpConversation : Conversation
	{
		
		private Connection _connection;
		
		private MsnpAccount _account;
		
		private MsnpContact _remoteContact = null;
		
		private event EventHandler _activated;
		
		private int _id;
		
		private bool _isChat;
		
		private MessageQueue messages;
		
		//private void Buddies 
		
		private static string _msg_header = "MIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nX-MMS-IM-Format: FN=Verdana; EF=; CO=800000; CS=0; PF=22\r\n\r\n{0}";
		
		public MsnpConversation (MsnpAccount account)
		{
			_id = -1;
			_account = account;
			_connection = null;
			_account.Command += _account_Command;
			_activated += onActivated;
			_isChat = false;
			messages = new MessageQueue ();
		}
		
		public void Open (MsnpContact contact)
		{
			OnActivated ();
			_remoteContact = contact;
			_id = _account.TrId;
			_account.SendCommand ("XFR {0} SB", _id);
		}
		
		public override void Close ()
		{
			if (_connection.Connected)
				_connection.Send ("BYE");
			
			base.Close ();
		}

		
		public void Invite (MsnpContact contact)
		{
			if (_connection.Connected)
				_connection.RawSend ("CAL 1 {0}\r\n", 
					contact.Username);
			else if (!IsChat) {
				Open (_remoteContact);
			}
		}
		
		private Connection createConnection (string hostname, int port)
		{
			Connection connection = new Connection (hostname, port);
			connection.DataArrived += _connection_DataArrived;
			connection.Open ();
			
			return connection;
		}
		
		public void Join (string hostname, int port, string id1, string id2)
		{
			if (_connection != null && _connection.Connected)
				_connection.Close ();
				
			_connection = createConnection (hostname, port);
			
			//_connection.Open ();
			
			_connection.StartAsynchronousReading ();
			
			_connection.RawSend (
				"ANS 1 {0} {1} {2}\r\n",
				_account.Username, 
				id2, 
				id1);
			//OnStarted();
		}
		
		public void RawSend (string data)
		{
			_connection.RawSend (data);
		}
		
		public override void SendText (string text)
		{
			/*
			if (!Connected) {
				Console.WriteLine ("Not Connected, saving stack");
				_messages.Push (text);
				return;
			}
			
			if (Buddies.Count == 0 && lastContact != null) {
				
			}
			*/
			
			if (Buddies.Count == 0 && !IsChat) {
				Console.WriteLine ("Queued: {0}", text);
				messages.Enqueue (text);
				Invite (RemoteContact);
				return;
			}
			string data = string.Format (_msg_header, text);
			
			string d = string.Format ("MSG {0} N {1}\r\n{2}", 1, data.Length, data);
			Debug.WriteLine ("Debug:{0}",d);
			_connection.RawSend (d);
			
			base.SendDataSent (text);
		}
		
		protected virtual void OnActivated ()
		{
			_activated (this, EventArgs.Empty);
		}
		
		protected override void OnDataGet (Buddy buddy, string data)
		{
			base.OnDataGet (buddy, data);
			OnActivated ();
		}
		
		protected override void OnClosed ()
		{
			if (_connection.Connected)
				_connection.Close ();
			
			base.OnClosed ();
		}
		
		private void _connection_DataArrived (
			object sender,
			DataArrivedArgs args)
		{
			processCommand (args.Data);
		}
		
		private bool processCommand (string msnpcommand)
		{				
			string [] command = msnpcommand.Split (" ".ToCharArray ());
			
			Console.WriteLine (">{0}<", msnpcommand);
			
			switch (command [0]) {
			
				case "IRO": {
					Buddy buddy = 
						_account.Buddies.GetByUsername (
							command [4]);
					if (buddy != null) {
						Buddies.Add (buddy);
						Debug.WriteLine ("Added {0} to conversation", buddy.Username);
						sendQueuedMessages ();
						if (Buddies.Count >= 2)
							_isChat = true;
					}
					else
						Debug.WriteLine ("{0} is not here", command [4]);
				} break;
				
				case "JOI": {
					Buddy buddy = _account.Buddies.GetByUsername (command [1]);
						
					if (buddy != null) {
						//lastContact = (MsnpContact) buddy;
						Buddies.Add (buddy);
						Debug.WriteLine ("{0} Joins to conversation", buddy.Username);
						if (Buddies.Count > 1)
							_isChat = true;
						sendQueuedMessages ();
						OnActivated ();
					}
				} break;
				
				case "MSG": {
					int length = int.Parse (
						command [command.Length -1]);
					
					string message = _connection.Read (length);
					
					Console.WriteLine ("MSG-DATA: ~{0}~\n",
						message);
					
					string str = "\r\n\r\n";
					int index = message.IndexOf (str) + str.Length;
					Debug.WriteLine ("\"{0}\"", message);
					
					string ct = "Content-Type: ";
					
					int cti = message.IndexOf (ct) + ct.Length;
					
					string content = 
						message.Substring (
							cti,
							message.IndexOf ("\r", cti));
					
					string [] content_array = content.Split (" ".ToCharArray ());
					
					if (content_array.Length > 1) {
						if (content_array [0] == "text/plain;") {
							Debug.WriteLine ("Charset: {0}", content_array [1]);
							Buddy buddy = Buddies.GetByUsername (command [1]);
							
							SendDataGet (
								buddy,
								message.Substring (index));
						}
					}
					
					string tus = "TypingUser: ";
					
					int tu = message.IndexOf (tus);
					
					if (tu > 0) {
						tu += tus.Length;
						string username = 
							message.Substring (tu).Trim ();
						
						Buddy b = Buddies.GetByUsername (
							username);
						
						if (b != null)
							SendTyping (b);
						else
							Debug.WriteLine ("Is not here");
					}
					
					//Debug.WriteLine ("ContentType :{0}", content_type);
					
					
					//Debug.WriteLine ("->{0}<-",
					//	message.Substring (index));
					
				} break;
				
				case "BYE": {
					Buddy bud = Buddies.GetByUsername (command [1]);
					
					if (bud != null) {
						Console.WriteLine ("{0} Leaves the conversation", bud.Alias);
						Buddies.Remove (bud);
					}
					if (Buddies.Count == 0) {
						OnClosed ();
					}
					//lastContact = (MsnpContact) bud;
				} break;
								
				default:
				break;
			}
			
			return true;
		}
		
		
		private void _account_Command (object sender, MsnpCommandArgs args)
		{	
			if (args.Command.Type == MsnpCommandType.RNG) {
				MsnpContact c = (MsnpContact) _account.Buddies.GetByUsername (
					args.Command.Arguments [4]);
				
				if (c == null || RemoteContact == null || c != RemoteContact)
					return;
				
				string [] arguments = args.Command.Arguments;
				
				string [] peer = arguments [1].Split (":".ToCharArray ());
				
				string hostname = peer [0];
				int port;
				
				if (!int.TryParse (peer [1], out port))
					return;
								
				Join (hostname, port, arguments [0], arguments [3]);
			} else if (args.Command.Type == MsnpCommandType.XFR) {				
				if (args.Command.TrId == _id) {
					createConversation (args.Command);
				}
			}
		}
		
		// TODO: Implement createConversation (string, int)..
		
		private void createConversation (MsnpCommand cmd)
		{
			string [] args = cmd.Arguments;
			
			string [] pieces = args [1].Split (":".ToCharArray ());
			string hostname = pieces [0];
			int port;
				
			if (!int.TryParse (pieces[1], out port))
				return;
			
			_connection = createConnection (hostname, port);
			_connection.StartAsynchronousReading ();
			
			_connection.RawSend ("USR 1 {0} {1}\r\n", 
				_account.Username,
				args [3]);
			
			Invite (_remoteContact);
		}
		
		private void sendQueuedMessages ()
		{
			while (messages.Count > 0) {
				SendText (messages.Dequeue ());
			}
		}
		
		private void onActivated (object sender, EventArgs args)
		{
		}
		
		public MsnpAccount Account {
			get { return _account; }
		}
		
		public event EventHandler Activated {
			add { _activated += value; }
			remove { _activated -= value; }
		}
		
		public bool IsChat {
			get { return _isChat; }
		}
		
		internal MsnpContact RemoteContact {
			get { return _remoteContact; }
			set { _remoteContact = value; }
		}
		
	}
}
