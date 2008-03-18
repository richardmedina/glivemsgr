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
		private int trId = 1;
		
		private Connection notificationServer;
		private Connection dispatchServer = null;
		
		private BuddyCollection buddies;

		private MsnpGroupCollection groups;
		private MsnpConversationCollection conversations;
		
		private MsnpPassportInfo passport;
		
		private MsnpConversation _nextConversation;
		
		private static readonly string group_nogroup_name = "No group";
		
		private static readonly MsnpGroup noGroup = new MsnpGroup (group_nogroup_name, -1);
		
		public event ConversationRequestHandler ConversationRequest;
		
		private event MsnpCommandHandler command;
		
		private MsnpObject _msnpObject;
		
		private readonly string [] loginCommands = {
			"VER {0} MSNP8 MSNP9 CVR0",
			"CVR {0} 0x0C0A winnt 5.1 i386 MSNMSGR 6.0.0602 " +
				"MSMSGS {1}",
			"USR {0} TWN I {1}",
			"USR {0} TWN S {1}"
		};
		
		private System.IO.StreamWriter writer;
		
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
			command = onMsnpCommand;
			createNextConversation ();
			//writer = new StreamWriter ("/home/ricki/Desktop/object.txt");
		}
		
		//Return values
		// 	0	Connection Successfully
		// 	> 0	Can't open connection
		
		
		public override int Login ()
		{
			//Clear current data
			trId = 0;
			Debug.Enable = false;
			
			buddies.Clear ();
			groups.Clear ();
			groups.Add (noGroup);
			conversations.Clear ();
			
			_msnpObject = MsnpObject.Create (this.Username, "/home/ricki/me.png");
			
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
				Username);
			Debug.WriteLine ("response: {0}", 
				notificationServer.Read ());
			
			
			notificationServer.Send (loginCommands [2], 
				TrId,
				Username);
			
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
				TrId, Username);
			
			Debug.WriteLine (dispatchServer.Read ());
			dispatchServer.Send (loginCommands [2], 
				TrId, Username);
				
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
			//writer.Close ();
			foreach (Conversation conv in conversations)
				conv.Close ();
			
			if (dispatchServer != null)
				if (dispatchServer.Connected)
					dispatchServer.Close ();
			
			base.Logout ();
		}
		
		public void OpenConversation (MsnpContact contact)
		{
			
		}
		
		public void StartConversation (MsnpContact contact)
		{	
			
			MsnpConversation conv;
			
			if (!FindConversation (contact, out conv)) {
				conv = _nextConversation;
				createNextConversation ();
				conversations.Add (conv);
			}
						
			if (!conv.Connected)
				conv.Open (contact);
		}
		
		protected virtual void OnMsnpCommand (string msnpcommand)
		{
			MsnpCommand cmd = 
				MsnpCommand.CreateFromString (msnpcommand);
			
			command (this, 
				new MsnpCommandArgs (cmd));
		}
		
		protected virtual void OnConversationRequest (MsnpConversation conv)
		{
			ConversationRequest (this, new ConversationRequestArgs (conv));
		}
		
		public void ChangeAlias (string newalias)
		{
			dispatchServer.Send ("REA {0} {1} 805306468 {2}",
				TrId,
				Username,
				Utils.UrlEncode (newalias));
		}
		
		public void ChangeState (MsnpContactState state)
		{
			dispatchServer.Send ("CHG {0} {1} {2}",
				TrId,
				Utils.ContactStateToString (state),
				_msnpObject.ToString ());
		}
		
		internal void SendCommand (string command, params object [] objs)
		{
			dispatchServer.Send (command, objs);
		}
		
		private bool FindConversation (MsnpContact contact, 
			out MsnpConversation conversation)
		{
			conversation = null;
			
			foreach (MsnpConversation c in conversations) {
				if ( c.Buddies.Count == 1 &&
					c.Buddies [0].Username == contact.Username) {
					conversation = c;
					return true;
				} else if (c.Buddies.Count == 0 && 
					c.RemoteContact.Username == contact.Username) {
					conversation = c;
					return true;
				}
			}
			
			return false;
		}
		
		private void createNextConversation ()
		{
			_nextConversation = new MsnpConversation (this);
			_nextConversation.Closed += nextConversation_Closed;
		}
		
		private string formatCockie (string cockie)
		{
			cockie  = cockie.Replace ("USR 6 TWN S ", "");
			string format = string.Format (
				"Passport1.4 OrgVerb=GET," +
				"OrgURL=http%3A%2F%2Fmessenger%2Emsn%2Ecom," +
				"sign-in={0},pwd={1},{2}",
				Username,
				Password,
				cockie);
			
			
			return format;

		}
		
		private void nextConversation_Closed (object sender, EventArgs args)
		{
			Console.WriteLine ("Removing Conversation");
			Conversations.Remove ((MsnpConversation) sender);
		}
		
		private string nexusLogin (string cockie)
		{
			WebRequest req;
			WebResponse res;
			try {
				 req = WebRequest.Create ("https:nexus.passport.com/rdr/pprdr.asp");
				 res = req.GetResponse ();
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return string.Empty;
			}
			
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

		private void processCommand (MsnpCommand cmd)
		{
			string msnpcommand = cmd.RawString;
			Console.WriteLine ("MsnpAccount.processCommand>{0}<=", msnpcommand);
			//Console.WriteLine ("Type: {0}", cmd.Type.ToString ());
			
			if (msnpcommand == string.Empty) {
				return;
			}
			
			string [] command = msnpcommand.Split (
				" ".ToCharArray ());
			
			if (command [0].EndsWith (":")) {
				setPassportAttr (command [0], command [1]);
				return;
			}
			
			switch (cmd.Type) {
				case MsnpCommandType.USR: { // Getting Alias
					
					Alias = Utils.UrlDecode (
						command [4]);
					OnAliasChanged ();
					Debug.WriteLine ("Setting Alias: {0}", 
						Alias);
					SendCommand ("SYN {0} 0", TrId);
				}
				break;
				
				case MsnpCommandType.SYN: { // Getting contact and group lists
					//int groups = int.Parse (command [4]);
					//int contacts = int.Parse (command [3]);
					
					OnStarted ();
					
					string c =  string.Format ("CHG {0} {1}",
							TrId,
							Utils.ContactStateToString (
								State));
					dispatchServer.Send (c);
				}
				break;
				
				case MsnpCommandType.LSG: { // List Groups
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
				
				case MsnpCommandType.LST: { // Save Contact (list) and groups for each contact
					MsnpContact contact = new MsnpContact (
						command [1],
						Utils.UrlDecode (command [2])
					);
					contact.Account = this;
					
					//Console.WriteLine ("LST nick : {0}",Utils.UrlDecode (command [2]));
					
					if (command.Length == 5) {
						string [] ids = command  [4].Split (
							",".ToCharArray ());
						
						Debug.WriteLine ("Belong to {0} group mask", command [3]);
						if (ids.Length == 0) {
							//Debug.WriteLine ("Adding {0} to no group", contact.Username);
							contact.Groups.Add (noGroup);
						}
						
						foreach (string id in ids) {
							Debug.WriteLine ("\tid>{0}", id);
							MsnpGroup mGroup = Groups.GetById (int.Parse (id));
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
				
				case MsnpCommandType.CHG: {
					State = Utils.StringToContactState (
						command [2]);
					OnStateChanged ();
					//SendStateChanged ();
					Debug.WriteLine ("Changed ContactState to {0}..", 
						State.ToString ());
				}
				break;
				
				case MsnpCommandType.FLN: {
					MsnpContact c = (MsnpContact) 
						Buddies.GetByUsername (command [1]);
					
					if (c != null) {
						Debug.WriteLine ("User {0} found for Status changing", c.Alias);
						c.State = MsnpContactState.Offline;
					}
				}
				break;
				
				case MsnpCommandType.ILN:
				case MsnpCommandType.NLN: {
					int state_index = command [0] == "ILN"?2:1;
					int username_index = state_index + 1;
					int alias_index = state_index + 2;
					
					//writer.WriteLine (Utils.UrlDecode (msnpcommand));
					
					if (command [alias_index] == Username) {
						Alias =  Utils.UrlDecode (
							command [alias_index]);
						
						Debug.WriteLine ("Now Online with {0} contacts", Buddies.Count);
					} else{
						MsnpContact c = (MsnpContact) Buddies.GetByUsername (
							command[username_index]);
						
						if (c != null) {
							Console.WriteLine ("{0}({1}) is {2}",
								c.Username,
								command [alias_index],
								command [state_index]);
							c.Alias = Utils.UrlDecode (command [alias_index]);
							c.State = Utils.StringToContactState (command [state_index]);
							
							Debug.WriteLine ("State is {0}", c.State);
						} else
							Debug.WriteLine ("Contact not found");
					}
				}	
				break;
				
				case MsnpCommandType.CHL: { // send md5 hash
				
					Console.Write ("Ping? ");
					string static_key = "Q1P7W2E4J9R8U3S5";
					
					string md5sum = Utils.MD5Sum (command [2] + static_key);
					
					dispatchServer.Send ("QRY {0} msmsgs@msnmsgr.com 32", TrId);
					dispatchServer.RawSend (md5sum);
					Console.WriteLine ("..Pong!");
					
					//Debug.WriteLine ("MD5Sum: {0}", md5sum);
				}
				break;
				
				case MsnpCommandType.RNG:
					Console.WriteLine ("Ready for create conversation");
					
					MsnpContact c = (MsnpContact) Buddies.GetByUsername (cmd.Arguments [4]);
					
					if (c != null) {
						foreach (MsnpConversation conv in Conversations)
							if (conv.Buddies.Count == 1 &&	conv.RemoteContact == c)
							return;
					
						_nextConversation.RemoteContact = (MsnpContact) c;
						Conversations.Add (_nextConversation);
						createNextConversation ();
					}
					//_nextConversation._account_Command (this, new MsnpCommandArgs (cmd));
				break;
				
				case MsnpCommandType.PNG:
					Console.WriteLine ("Receiving PNG Command");
				break;
				/*
				// FIXME: Find a nice way to perform that..
				case MsnpCommandType.RNG: { // Conversation request
					string r1 = command [1];
					string [] conn_info = command [2].Split (":".ToCharArray ());
					string r2 = command [4];
					string email = command [5];
					
					Console.WriteLine (msnpcommand);
					
					//connect to switchboard server
					
					
					MsnpContact contact = 
						(MsnpContact) Buddies.GetByUsername (email);

					MsnpConversation conversation = new MsnpConversation (this);
					conversation.Hostname = conn_info [0];
					conversation.Port = int.Parse (conn_info [1]);
					conversation.Contact = contact;
					conversation.Create = false;
					conversation.Id = r1;
					conversation.Id2 = r2;
					conversation.Connect ();
					
					//	new MsnpConversation (
					//		this,
					//		conn_info [0],
					//		int.Parse (conn_info [1]));
										
					Debug.WriteLine ("Conversation from {0} ({1})", contact.Alias, contact.State);
					
					//conversation.Join (r1, r2);
					
					ConversationRequest (this, 
						new ConversationRequestArgs (conversation));
					
				}
				break;
				*/
				case MsnpCommandType.REA:
					Alias = Utils.UrlDecode (command [4]);
					OnAliasChanged ();
				break;

			}
		
		}
		
		private void setPassportAttr (string attr, string val)
		{
		
			switch (attr) {
				case "MIME-Version:":
					Passport.MimeVersion = 
						float.Parse (val);
				break;
				
				case "Content-Type:":
					Passport.ContentType = val;
				break;
				
				case "LoginTime:":
					Passport.LoginTime = 
					long.Parse (val);
				break;
				
				case "EmailEnabled:":
					Passport.EmailEnabled = val;
				break;
				
				case "MemberIdHigh:":
					Passport.MemberIdHight = 
						int.Parse (val);
				break;
				
				case "MemberIdLow:":
					Passport.MemberIdLow = 
						int.Parse (val);
				break;
				
				case "lang_preference:":
					Passport.LangPreference = 
						int.Parse (val);
				break;
				
				case "preferredEmail:":
					Passport.PreferredEmail = val;
				break;
				
				case "country:":
					Passport.Country = val;
				break;
				
				case "PostalCode:":
					Passport.PostalCode = val;
				break;
				
				case "Gender:":
					Passport.Gender = val;
				break;
				
				case "Kid:":
					Passport.Kid = int.Parse (val);
				break;
				
				case "Age:":
					Passport.Age = val;
				break;
				
				case "BDayPre:":
					Passport.BDayPre = val;
				break;
				
				case "Birthday:":
					Passport.Birthday = val;
				break;
				
				case "Wallet:":
					Passport.Wallet = val;
				break;
				
				case "Flags:":
					Passport.Flags = long.Parse (val);
				break;
				
				case "sid:":
					Passport.SId = int.Parse (val);
				break;
				
				case "MSPAuth:":
					Passport.MspAuth = val;
				break;
				
				case "ClientIP:":
					Passport.ClientIP = val;
				break;
				
				case "ClientPort:":
					Passport.ClientPort = 
						int.Parse (val);
				break;
				default:
					Console.WriteLine ("Passport <{0}> attr not found.", attr);
				break;
			}
		}
		
		private void dispatchServer_DataArrived (object sender,
			DataArrivedArgs args)
		{
			//Debug.WriteLine ("DataArrived: {0}", args.Data);
			//processCommand (args.Data);
			OnMsnpCommand (args.Data);
		}
		
		private void dispatchServer_Disconnected (object sender,
			EventArgs args)
		{
			Debug.WriteLine ("Connection was interrupted");
		}
		
		private void onConversationRequest (object sender, ConversationRequestArgs args)
		{
			Console.WriteLine ("Adding Conversation");
			args.Conversation.Closed += conversation_Closed;
			conversations.Add (args.Conversation);
		}
		
		private void conversation_Closed (object sender, EventArgs args)
		{
			Console.WriteLine ("Removing Conversation");
			MsnpConversation conv = (MsnpConversation) sender;
			conversations.Remove (conv);
		}
		
		private void onMsnpCommand (object sender, MsnpCommandArgs args)
		{
			processCommand (args.Command);
		}
		
		public int TrId {
			get { return trId ++; }
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
		
		public event MsnpCommandHandler Command {
			add { command += value; }
			remove { command -= value; }
		}
	}
}