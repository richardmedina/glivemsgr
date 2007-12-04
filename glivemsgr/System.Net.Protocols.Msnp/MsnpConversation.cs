
using System;
using System.Threading;
using System.Net.Protocols;
using System.Text.RegularExpressions;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpConversation : Conversation
	{
		private string hostname;
		private int port;
		
		private Connection connection;
		private MsnpAccount account;
		
		private static string msg_header = "MIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nX-MMS-IM-Format: FN=Verdana; EF=; CO=800000; CS=0; PF=22\r\n\r\n{0}";
		
		private event EventHandler started;
		
		public MsnpConversation (MsnpAccount account, 
			string hostname, 
			int port)
			//string random1,
			//string random2)
		{
			this.account = account;
			this.hostname = hostname;
			this.port = port;
			this.started = onStarted;
		}
		
		public void Join (string random1, string random2)
		{
			connection = new Connection (hostname, port);
			
			connection.Open ();
			Debug.WriteLine ("Connected to new conversation");
			connection.RawSend ("ANS 1 {0} {1} {2}\r\n",
				account.Username, random2, random1);
						
			connection.Disconnected += delegate {
				this.Close ();
			};
			
			connection.DataArrived += delegate (object sender, 
				DataArrivedArgs args) {
				this.processCommand (args.Data);
			};
			
			connection.StartAsynchronousReading ();
		}
		
		//FIXME: perform in separated thread and throw Started event
		public void Start (MsnpContact contact, string random)
		{
			connection = new Connection (hostname, port);
			
			//connection.Disconnected += delegate { this.Close (); };
			System.Threading.Thread thread = new System.Threading.Thread ((ThreadStart) delegate {
			try {
				connection.Open ();
			}catch (Exception e) {
				Console.WriteLine ("Error connecting : {0}",e.Message);
				return;
			}
			
			Console.WriteLine ("USR 1 {0} {1}",account.Username, random);
			
			connection.RawSend ("USR 1 {0} {1}\r\n",account.Username, random);
			
			string recv = connection.Read ();
			Debug.WriteLine (recv);
			
			Console.WriteLine ("CAL 1 {0}", contact.Username);
			
			connection.RawSend ("CAL 1 {0}\r\n", contact.Username);
			recv = connection.Read ();
			Debug.WriteLine (recv);
			
			connection.DataArrived += delegate (object sender,
			DataArrivedArgs args) {
				this.processCommand (args.Data);
			};
			connection.StartAsynchronousReading ();
			
			Console.WriteLine ("OnStarted");
			OnStarted ();
			
			});
			
			thread.Start ();
		}
		
		private bool processCommand (string msnpcommand)
		{				
			string [] command = msnpcommand.Split (" ".ToCharArray ());
			
			Debug.WriteLine (">{0}<", msnpcommand);
			
			switch (command [0]) {
			
				case "IRO": {
					Buddy buddy = 
						account.Buddies.GetByUsername (
							command [4]);
					if (buddy != null) {
						this.Buddies.Add (buddy);
						Debug.WriteLine ("Added {0} to conversation", buddy.Username);
					}
					else
						Debug.WriteLine ("{0} is not here", command [4]);
				} break;
				
				case "JOI": {
					Buddy buddy = account.Buddies.GetByUsername (command [1]);
					
					if (buddy != null) {
						this.Buddies.Add (buddy);
						Debug.WriteLine ("{0} Joins to conversation", buddy.Username);
					}
				
				} break;
				
				case "MSG": {
					int length = int.Parse (
						command [command.Length -1]);
					
					string message = connection.Read (length);
/*
					string mv = "MIME-Version: ";
					int mime_index = mv.Length;
					
					int end_index = message.IndexOf ("\r", 
						mime_index);
					
					string mime = message.Substring (
						mime_index ,
						end_index
					);
					
					Debug.WriteLine ("MiME :{0}", mime);
					
					
*/
					//formatMessage (message);
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
							
							base.SendDataGet (
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
						
						Buddy b = this.Buddies.GetByUsername (
							username);
						
						if (b != null)
							this.SendTyping (b);
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
						Debug.WriteLine ("{0} Leaves the conversation", bud.Alias);
						Buddies.Remove (bud);
					}
				} break;
				
				default:
				break;
			}
			
			return true;
		}
		//static int trid = 1;
		public override void SendText (string text)
		{
			string data = string.Format (msg_header, text);
			
			string d = string.Format ("MSG {0} N {1}\r\n{2}", 1, data.Length, data);
			Debug.WriteLine ("Debug:{0}",d);
			connection.RawSend (d);
			
			base.SendDataSent (text);
		}
				
		protected override void OnDataReceived (string data)
		{
			Debug.WriteLine ("MsnpConversation.Received>{0}", data);
		}
		
		protected override void OnDataGet (Buddy buddy, string data)
		{
			base.OnDataGet (buddy, data);
			Debug.WriteLine ("MsnpConversation.Received>{0}", data);
		}
		
		protected override void OnTyping (Buddy buddy)
		{
		}
		
		protected override void OnClosed ()
		{
			connection.RawSend ("OUT");
			connection.Close ();
			base.OnClosed ();
		}
		
		protected virtual void OnStarted ()
		{
			started ( this, EventArgs.Empty);
		}
		
		private void onStarted (object sender,EventArgs args)
		{
		}
		
		/*
		private void formatMessage (string message)
		{
			Regex regex = new Regex (
				@"\w*Content-Type: (?<ContentType>[\w/-]+);" + // ContentType
				@"[^\w]*(charset=(?<charset>[\w-]+)){,1}" + // charset
				@"[^\w]*(X-MMS-IM-Format: FN=(?<FontName>[\w-]+); ){,1}" + // FontName
				@"[^\w]*(EF=(?<Efects>\w*); ){,1}" + // Efects
				@"[^\w]*(CO=(?<Color>\d{6}); ){,1}" +
				@"[^\w]*(CS=(?<CharCode>\w+); ){,1}" +
				@"[^\w]*(PF=(?<PF>\w+)){,1}" +
				@"[^\w]*(TypingUser: (?<TypingUser>w+)){,1}"
				
				);
			
			MatchCollection mc = regex.Matches (message);
			string [] gn = regex.GetGroupNames ();
		
			Debug.WriteLine ("Matches: {0}", mc.Count);
			
			foreach (Match match in mc) {
				for (int i = 0; i < gn.Length; i ++)
				Debug.WriteLine ("{0}=>{1}",
					gn [i],
					match.Groups [gn [i]]);
			}
		}
		*/
		public MsnpAccount Account {
			get { return account; }
		}
		
		public new BuddyCollection Buddies {
			get { return base.Buddies; }
		}
		
		public event EventHandler Started {
			add { started += value; }
			remove { started -= value; }
		}
	}
}
