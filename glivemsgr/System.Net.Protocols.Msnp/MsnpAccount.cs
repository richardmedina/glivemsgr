// project created on 04/07/2007 at 00:38
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using System.Web;

using System.IO;

namespace System.Net.Protocols.Msnp
{

	public class MsnpAccount : System.Net.Protocols.Account
	{
		private static int trId = 1;
		
		private Connection notificationServer;
		private Connection dispatchServer = null;
		
		private BuddyCollection buddies;

		private MsnpGroupCollection groups;
		private MsnpConversationCollection conversations;
		
		private MsnpPassportInfo passport;
		
		public event ConversationRequestHandler ConversationRequest;
		
		private readonly string [] loginCommands = {
			"VER {0} MSNP8 MSNP9 CVR0",
			"CVR {0} 0x0C0A winnt 5.1 i386 MSNMSGR 6.0.0602 " +
				"MSMSGS {1}",
			"USR {0} TWN I {1}",
			"USR {0} TWN S {1}"
		};
		
		public MsnpAccount () : 
			this (string.Empty, string.Empty)
		{
		}
		
		public MsnpAccount (string username, string password) : 
			base (username, password)
		{
			passport = new MsnpPassportInfo ();
			buddies = new BuddyCollection ();
			conversations = new MsnpConversationCollection ();

			State = MsnpContactState.Offline;			
			groups = new MsnpGroupCollection ();
			
			ConversationRequest = onConversationRequest;
		}
		
		//Return values
		// 	0	Connection Successfully
		// 	> 0	Can't open connection
		
		
		public override int Login ()
		{
			//Clear current data
			MsnpAccount.trId = 0;
			Debug.Enable = true;
			
			buddies.Clear ();
			groups.Clear ();
			conversations.Clear ();
			
			Debug.WriteLine ("Debug enable");
			// Notification server
			notificationServer = new Connection (
				"messenger.hotmail.com", 1863);
			
			Debug.Write ("Connecting to {0}:{1}...", 
				notificationServer.Hostname, 
				notificationServer.Port);
			
			try {
				notificationServer.Open ();
			} catch (Exception e) {
				Debug.WriteLine ("Error Connecting to {0}:{1} : {2}",
					notificationServer.Hostname,
					notificationServer.Port,
					e.Message);
				return 1;
			}
			
			Debug.WriteLine ("Done");	
			
			notificationServer.Send (loginCommands [0], TrId);
			Debug.WriteLine ("response: {0}", 
				notificationServer.Read ());
			
			notificationServer.Send (loginCommands [1], 
				TrId,
				this.Username);
			Debug.WriteLine ("response: {0}", 
				notificationServer.Read ());
			
			
			notificationServer.Send (loginCommands [2], 
				TrId,
				this.Username);
			
			string response = notificationServer.Read ();
			
			Debug.WriteLine ("response: {0}", 
				response);
			
			if (response.StartsWith ("911"))
				return -1;
			
			string [] response_formatted = response.Split (
				" ".ToCharArray ());
			
			string [] authserver_info = response_formatted [3].Split (
				":".ToCharArray ());
			
			Debug.WriteLine ("Connecting to authentification server");
			
			notificationServer.Close ();
			
			// Dispatch Server
			dispatchServer = new Connection (
				authserver_info [0],
				int.Parse (authserver_info [1]));

			dispatchServer.DataArrived += 
				dispatchServer_DataArrived;
			dispatchServer.Disconnected += 
				dispatchServer_Disconnected;

			try {
				dispatchServer.Open ();
			} catch (Exception) {
				return 2;
			}
			
			dispatchServer.Send (loginCommands [0], TrId);
			
			Debug.WriteLine (dispatchServer.Read ());
			
			dispatchServer.Send (loginCommands [1],
				TrId, this.Username);
			
			Debug.WriteLine (dispatchServer.Read ());
			dispatchServer.Send (loginCommands [2], 
				TrId, this.Username);
				
			response = dispatchServer.Read ();
			Debug.WriteLine (response);
			string cockie = formatCockie (response);
			
			ServicePointManager.CertificatePolicy = new 
				MsnpCertificatePolicy ();
			
			
			// Authentication server
			string ticket = nexusLogin (cockie);
			
			// if error return
			if (ticket == string.Empty) {
				notificationServer.Close ();
				dispatchServer.Close ();
				return 2;
			}
			
			dispatchServer.Send (
				loginCommands [3],
				TrId,
				ticket
			);
			
			
			dispatchServer.StartAsynchronousReading ();
			
			return 0;	
		}
		
		public override void Logout ()
		{
			Console.WriteLine ("Preparing to close {0} conversations",
				conversations.Count);
			foreach (Conversation conv in conversations)
				conv.Close ();
			
			if (dispatchServer != null)
				if (dispatchServer.Connected)
					dispatchServer.Close ();
			
			base.Logout ();
		}
		
		public void StartConversation (MsnpContact contact)
		{
			dispatchServer.Send ("XFR {0} SB", TrId);
			
			string recv = string.Empty;
			bool gotit = false;
			
			do {
				
				if (!gotit) {
					this.processCommand (recv);
					recv = dispatchServer.Read ();
					gotit = recv.StartsWith ("XFR");
				}
			} while (!gotit);
			
			Console.WriteLine ("COMMAND>{0}", recv);
			string [] command = recv.Split (" ".ToCharArray ());
			
			string []  server_info = command [3].Split (":".ToCharArray ());
			
			MsnpConversation conv = new MsnpConversation (
				this,
				server_info [0],
				int.Parse (server_info [1]));
			
			Console.WriteLine ("Starting conversation:\n\tHost: {0}\n\tPort: {1}\n\tRandom: {2}",
				server_info [0], server_info [1], command [5]);
			
			conv.Start (contact, command [5]);
			
			this.ConversationRequest (this, 
				new ConversationRequestArgs (conv));
			
		}

		
		private string formatCockie (string cockie)
		{
			cockie  = cockie.Replace ("USR 6 TWN S ", "");
			string format = string.Format (
				"Passport1.4 OrgVerb=GET," +
				"OrgURL=http%3A%2F%2Fmessenger%2Emsn%2Ecom," +
				"sign-in={0},pwd={1},{2}",
				this.Username,
				this.Password,
				cockie);
			
			
			return format;

		}
				
		internal void SendCommand (string command, params object [] objs)
		{
			Thread thread = new Thread (delegate () {
					dispatchServer.Send (string.Format (
						command, objs));
			});
			
			thread.Start ();
		}

		private string nexusLogin (string cockie)
		{
			WebRequest req = WebRequest.Create ("https:nexus.passport.com/rdr/pprdr.asp");
			WebResponse res = req.GetResponse ();
			
			Regex regex = new Regex ("dalogin=([^,]+)", RegexOptions.IgnoreCase);
			
			GroupCollection group = regex.Match (res.Headers ["PassportURLs"]).Groups;
			
			req = WebRequest.Create ("https://" + group [1].Value.TrimEnd ());
			req.Headers.Add ("Authorization", cockie);
			
			for (int retry = 0; retry < 1; retry ++) {
				try {
					res = req.GetResponse ();
				} catch (Exception) {
					return string.Empty;
				}
				
				regex = new Regex ("da-status=([^,]+)", RegexOptions.IgnoreCase);
				string info = string.Empty;
				
				if (res.Headers ["WWW-Authenticate"] != null)
					info = res.Headers ["WWW-Authenticate"];
				else if (res.Headers ["Authentication-Info"] != null)
					info = res.Headers ["Authentication-Info"];
				Debug.WriteLine ("Info : {0}", info);
				group = regex.Match (info).Groups;
				
				switch (group [1].Value) {
					case "redir":
						req = WebRequest.Create(res.Headers ["Location"]);
						req.Headers.Add ("Authentication", cockie);
					break;
					
					case "success":
						regex = new Regex("from-pp='([^']+)", 
							RegexOptions.IgnoreCase);
						group = regex.Match (res.Headers ["Authentication-Info"]).Groups;
						return group [1].Value;
					
					case "failed":
						return string.Empty;
				}
			}
			
			return string.Empty;
		}

		private void processCommand (string msnpcommand)
		{
			Debug.WriteLine ("MsnpAccount.processCommand>{0}<=", msnpcommand);
			
			if (msnpcommand == string.Empty) {
				return;
			}
			
			string [] command = msnpcommand.Split (
				" ".ToCharArray ());
			
			if (command [0].EndsWith (":")) {
				setPassportAttr (command [0], command [1]);
				return;
			}
			// FIXME: this would be improved comparing with
			// enumeration values
			switch (command [0]) {
				case "USR": { // Getting Alias
					
					this.Alias = Utils.UrlDecode (
						command [4]);
					Debug.WriteLine ("Setting Alias: {0}", 
						this.Alias);
					SendCommand ("SYN {0} 0", TrId);
				}
				break;
				
				case "SYN": { // Getting contact and group lists
					int groups = int.Parse (command [4]);
					int contacts = int.Parse (command [3]);
				
					for (int i = 0; i < 4; i ++)
					Debug.WriteLine ("//{0}",
						dispatchServer.Read ());
					
					for (int i = 1; i < groups; i ++) {
						string str = string.Empty;
						do {
							str = dispatchServer.Read ();
							processCommand (str);
						} while (!str.StartsWith ("LSG"));
							//processCommand (str);
					}
					
					for (int i = 0; i < contacts; i ++) {
						string str = string.Empty;
						do {
							str = dispatchServer.Read ();
							processCommand (str);
						}while (!str.StartsWith ("LST"));
						//processCommand (str);
					}
					
					base.OnStarted ();
					
					string c =  string.Format ("CHG {0} {1}",
							TrId,
							Utils.ContactStateToString (
								this.State));
					dispatchServer.Send (c);
				}
				break;
				
				case "LSG": { // List Groups
					int group_id = int.Parse (command [1]);
					
					Debug.WriteLine ("Adding Group..{0} with id {1}", 
						Utils.UrlDecode (command [2]),
						group_id);
					
					MsnpGroup mGroup = new MsnpGroup (
						Utils.UrlDecode (command [2]),
						group_id);
					
					Groups.Add (mGroup);
				}
				break;
				
				case "LST": { // Save Contact (list) and groups for each contact
					MsnpContact contact = new MsnpContact (
						command [1],
						Utils.UrlDecode (command [2])
					);
					contact.Account = this;
					
					if (command.Length == 5) {
						string [] ids = command  [4].Split (
							",".ToCharArray ());
						
						Debug.WriteLine ("Belog to {0} group mask", command [3]);
						foreach (string id in ids) {
							Debug.WriteLine ("\tid>{0}", id);
							MsnpGroup mGroup = this.Groups.GetById (int.Parse (id));
							if (mGroup != null) {
								Debug.WriteLine ("Adding group {0} to {1}", mGroup.Name, contact.Username);
								contact.Groups.Add (mGroup);
							} else Debug.WriteLine ("I can't get group id from plain id for {0}..", contact.Username);
						}
					}

					contact.ListsMask = int.Parse (command [3]);
					Debug.WriteLine ("Adding LST: {0} groups : {1} == {2}", contact.Username, contact.ListsMask.ToString (), command [3]);			
					Buddies.Add (contact);
				}
				break;
				
				case "CHG": {
					this.State = Utils.StringToContactState (
						command [2]);
					//this.SendStateChanged ();
					Debug.WriteLine ("Changed ContactState to {0}..", 
						this.State.ToString ());
				}
				break;
				
				case "FLN": {
					MsnpContact c = (MsnpContact) 
						Buddies.GetByUsername (command [1]);
					
					if (c != null) {
						Debug.WriteLine ("User {0} found for Status changing", c.Alias);
						c.State = MsnpContactState.Offline;
					}
				}
				break;
				
				case "ILN":
					
				case "NLN": {
					Console.WriteLine (msnpcommand);
					
					int state_index = command [0] == "ILN"?2:1;
					
					
					if (command [3] == this.Username) {
						Debug.WriteLine ("Setting Alias");
						this.Alias =  Utils.UrlDecode (
							command [state_index + 2]);
						
						Debug.WriteLine ("Now Online with {0} contacts", Buddies.Count);
					} else{
						MsnpContact c = (MsnpContact) this.Buddies.GetByUsername (
							command[state_index + 1]);
						
						if (c != null) {
							Debug.WriteLine ("{0} is {1}",
								c.Username,
								command [state_index]);
							c.Alias = Utils.UrlDecode (command [state_index + 2]);
							c.State = Utils.StringToContactState (command [state_index]);
							Console.WriteLine ("Setting state to {0}", Utils.StringToContactState (command [state_index]));
							
							Debug.WriteLine ("State is {0}", c.State);
						} else
							Debug.WriteLine ("Contact not found");
					}
				}	
				break;
				
				case "CHL": { // send md5 hash
					string static_key = "Q1P7W2E4J9R8U3S5";
					
					string md5sum = Utils.MD5Sum (command [2] + static_key);
					
					dispatchServer.Send ("QRY {0} msmsgs@msnmsgr.com 32", TrId);
					dispatchServer.RawSend (md5sum);
					
					//Debug.WriteLine ("MD5Sum: {0}", md5sum);
				}
				break;
				
				// FIXME: Find a nice way to perform that..
				case "RNG": { // Conversation request
					string r1 = command [1];
					string [] conn_info = command [2].Split (":".ToCharArray ());
					string r2 = command [4];
					string email = command [5];
					
					//connect to switchboard server
					
					
					MsnpContact contact = 
						(MsnpContact) Buddies.GetByUsername (email);

					MsnpConversation conversation =
						new MsnpConversation (
							this,
							conn_info [0],
							int.Parse (conn_info [1]));
										
					Debug.WriteLine ("Conversation from {0} ({1})", contact.Alias, contact.State);
					
					conversation.Join (r1, r2);
					
					//conversation.DataReceived += 
					//	delegate (object sender, 
					//		DataReceivedArgs args) {
					//	Debug.WriteLine ("Rec: {0}", args.Data);
						//conversation.SendText ("Que paso?");
					//};
					
					
					this.ConversationRequest (this, 
						new ConversationRequestArgs (conversation));
					
				}
				break;

			}
		
		}
		
		private void setPassportAttr (string attr, string val)
		{
		
			switch (attr) {
				case "MIME-Version:":
					this.Passport.MimeVersion = 
						float.Parse (val);
				break;
				
				case "Content-Type:":
					this.Passport.ContentType = val;
				break;
				
				case "LoginTime:":
					this.Passport.LoginTime = 
					long.Parse (val);
				break;
				
				case "EmailEnabled:":
					this.Passport.EmailEnabled = val;
				break;
				
				case "MemberIdHigh:":
					this.Passport.MemberIdHight = 
						int.Parse (val);
				break;
				
				case "MemberIdLow:":
					this.Passport.MemberIdLow = 
						int.Parse (val);
				break;
				
				case "lang_preference:":
					this.Passport.LangPreference = 
						int.Parse (val);
				break;
				
				case "preferredEmail:":
					this.Passport.PreferredEmail = val;
				break;
				
				case "country:":
					this.Passport.Country = val;
				break;
				
				case "PostalCode:":
					this.Passport.PostalCode = val;
				break;
				
				case "Gender:":
					this.Passport.Gender = val;
				break;
				
				case "Kid:":
					this.Passport.Kid = int.Parse (val);
				break;
				
				case "Age:":
					this.Passport.Age = val;
				break;
				
				case "BDayPre:":
					this.Passport.BDayPre = val;
				break;
				
				case "Birthday:":
					this.Passport.Birthday = val;
				break;
				
				case "Wallet:":
					this.Passport.Wallet = val;
				break;
				
				case "Flags:":
					this.Passport.Flags = long.Parse (val);
				break;
				
				case "sid:":
					this.Passport.SId = int.Parse (val);
				break;
				
				case "MSPAuth:":
					this.Passport.MspAuth = val;
				break;
				
				case "ClientIP:":
					this.Passport.ClientIP = val;
				break;
				
				case "ClientPort:":
					this.Passport.ClientPort = 
						int.Parse (val);
				break;
			}
		}
		
		private void dispatchServer_DataArrived (object sender,
			DataArrivedArgs args)
		{
			Debug.WriteLine ("DataArrived: {0}", args.Data);
			processCommand (args.Data);
		}
		
		private void dispatchServer_Disconnected (object sender,
			EventArgs args)
		{
			Debug.WriteLine ("Connection was interrupted");
		}
		
		private void onConversationRequest (object sender, ConversationRequestArgs args)
		{
			Console.WriteLine ("Saving Conversation");
			conversations.Add (args.Conversation);
		}
		
		/*
		private void doLogin ()
		{
			Debug.WriteLine ("Reconnecting");
			Login ();
		}
		*/
		public static int TrId {
			get {
				return trId ++;
			}
		}
		
		public MsnpPassportInfo Passport {
			get {
				return passport;
			}
		}
		
		public MsnpGroupCollection Groups {
			get { return groups; }
		}
		
		public MsnpConversationCollection Conversations {
			get { return conversations; }
		}
		
		public BuddyCollection Buddies {
			get { return buddies; }
		}
		
		public new MsnpContactState State {
			get {
				return (MsnpContactState) base.State;
			}
			set {
				base.State = (int) value;
			}
		}
		
		public override ProtocolType Protocol {
			get { return ProtocolType.Msnp; }
		}
		
		public new string Username {
			get { return base.Username; }
			set { base.Username = value; }
		}
		
		public new string Password {
			get { return base.Password; }
			set { base.Password = value; }
		}
		
//		public event ConversationRequestHandler ConversationRequest {
//			add { conversationRequest += value; }
//			remove { conversationRequest -= value; }
//		}
		/*public Connection Connection {
			get { return connection; }
			set { connection = value; }
		}*/
	
	}
}