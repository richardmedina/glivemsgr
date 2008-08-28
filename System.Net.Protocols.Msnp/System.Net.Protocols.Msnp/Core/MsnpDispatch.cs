using System;
using System.Net;
using System.Net.Security;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net.Protocols.Msnp.Core
{
	public class MsnpDispatch : Connection
	{
		private string _username;
		private string _password;
		private int _trId;
		
		private event PassportArrivedHandler _passportArrived;
		private event MsnpCommandArrivedHandler _commandArrived;
		
		public MsnpDispatch () : 
			this (string.Empty, 
				0, 
				string.Empty, 
				string.Empty, 
				0)
		{
		}

		public MsnpDispatch (string hostname, int port, string username, string password, int trId)
			: base(hostname, port)
		{
			_username = username;
			_password = password;
			_trId = trId;
			_passportArrived = onPassportArrived;
			_commandArrived = onCommandArrived;
			
			AllowReconnect = false;
			//ServicePointManager.CertificatePolicy = new 
			//	MsnpCertificatePolicy ();
			
			
			ServicePointManager.CertificatePolicy = new MsnpCertificatePolicy ();
 
		//	ServicePointManager.ServerCertificateValidationCallback += 
		//	remoteCertificateValidationCallback;
		}

//	FIXME. This is the callback validation method. The compiler throws an 
//	exception because the feature is not implemented yet. ¿? 
/*	
		private bool remoteCertificateValidationCallback (
			object sender, 
			System.Security.Cryptography.X509Certificates.X509Certificate certificate, 
			System.Security.Cryptography.X509Certificates.X509Chain chain, 
			SslPolicyErrors sslPolicyErrors)
		{
		
			return false;
		}
	*/	
		//private certificateValidation (
		
		protected virtual void OnCommandArrived (MsnpCommand command)
		{
			processCommand (command);
			_commandArrived (this, new MsnpCommandArrivedArgs (command));
		}
		
		protected override void OnConnected ()
		{
			base.OnConnected ();
			StartAsynchronousReading ();
			
			Console.WriteLine ("Dispatch. Connected!");
			
			Send ("VER {0} MSNP8 MSNP9 CVR0", TrId ++);
		}

		
		protected override void OnDataArrived (string data)
		{
			MsnpCommand command = MsnpCommand.Parse (data);
			OnCommandArrived (command);
			
			base.OnDataArrived (data);
		}
		
		protected virtual void OnPassportArrived (string content)
		{
			_passportArrived (this, new PassportArrivedArgs (content));
		}
		
		private void processCommand (MsnpCommand command)
        {
			Console.WriteLine ("Dispatch({0}):{1}", 
				command.Type.ToString (),
				command.RawString);
			switch (command.Type) {
				case MsnpCommandType.VER:
					Send ("CVR {0} 0x0C0A winnt 5.1 i386 MSNMSGR 6.0.0602 " +
					"MSMSGS {1}", TrId ++, _username);
				break;
			
				case MsnpCommandType.CVR:
					Send ("USR {0} TWN I {1}",
						TrId ++, _username);
				break;
				/*
				case MsnpCommandType.XFR:
					Close ();
					string [] pieces = command.Arguments [1].Split (":".ToCharArray ());
					int port;
					if (int.TryParse (pieces [1], out port)) {
						//OnSuccess (pieces [0], port);
					} else throw new Exception ("Error casting type");
				break;
				*/
				case MsnpCommandType.USR:
					//Console.WriteLine ("Dispatch. Ticket: {0}",
					//	command.Arguments [2]);
					
					if (command.Arguments [0] == "TWN") {
						string cookie = formatCookie (command.Arguments [2]);
						string ticket = nexusLogin (cookie);
						Send ("USR {0} TWN S {1}", TrId ++, ticket);
					} else if (command.Arguments [0] == "OK") {
						//Console.WriteLine ("USR OK detected ..Lets read a line");
						Read ();
						/*
						MsnpCommand c = MsnpCommand.Parse (line);
						int lenght;
						
						if (int.TryParse (c.Arguments [2], out lenght)) {
							string passportcontent = Read (lenght);
							OnPassportArrived (passportcontent);
						} else throw new InvalidCastException ();
						*/
					}
				break;
				
				case MsnpCommandType.MSG:
					if (command.Arguments [0] == "Hotmail") {
						int length;
						if (int.TryParse (command.Arguments [2], out length))
							OnPassportArrived (Read (length));
							Send ("SYN {0} 0", TrId ++);
					}
				break;
			}
		
			Console.WriteLine ("Dispatch: processCommand Ends");
        	}
        	
		private string formatCookie (string cookie)
		{
			cookie  = cookie.Replace ("USR 6 TWN S ", "");
			string format = string.Format (
				"Passport1.4 OrgVerb=GET," +
				"OrgURL=http%3A%2F%2Fmessenger%2Emsn%2Ecom," +
				"sign-in={0},pwd={1},{2}",
				Username,
				Password,
				cookie);
			
			
			return format;

		}
		
		private string nexusLogin (string cookie)
		{
			Console.WriteLine ("Nexus");
			WebRequest req;
			WebResponse res;
			try {
				 req = WebRequest.Create ("https://nexus.passport.com/rdr/pprdr.asp");
				 Console.Write ("WebConnecting to {0}...", req.RequestUri);
				 res = req.GetResponse ();
				 Console.WriteLine ("DONE");
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return string.Empty;
			}
			
			Regex regex = new Regex ("dalogin=([^,]+)", RegexOptions.IgnoreCase);
			
			GroupCollection group = regex.Match (res.Headers ["PassportURLs"]).Groups;
			
			req = WebRequest.Create ("https://" + group [1].Value.TrimEnd ());
			req.Headers.Add ("Authorization", cookie);
			
			for (int retry = 0; retry < 1; retry ++) {
				try {
					Console.Write ("WebConnecting to {0}...", req.RequestUri);
					res = req.GetResponse ();
					Console.WriteLine ("DONE");
				} catch (Exception e) {
					Console.WriteLine (e);
					return string.Empty;
				}
				
				regex = new Regex ("da-status=([^,]+)", RegexOptions.IgnoreCase);
				string info = string.Empty;
				
				if (res.Headers ["WWW-Authenticate"] != null)
					info = res.Headers ["WWW-Authenticate"];
				else if (res.Headers ["Authentication-Info"] != null)
					info = res.Headers ["Authentication-Info"];
				Console.WriteLine ("Info : {0}", info);
				group = regex.Match (info).Groups;
				
				switch (group [1].Value) {
					case "redir":
						req = WebRequest.Create(res.Headers ["Location"]);
						req.Headers.Add ("Authentication", cookie);
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
		
		private void onCommandArrived (object sender, MsnpCommandArrivedArgs args)
		{
		}
		private void onPassportArrived (object sender, PassportArrivedArgs args)
		{
		}
		
		public int TrId {
			get { return _trId; }
			set { _trId = value; }
		}
		
		public new string Hostname {
			get { return base.Hostname; }
			set { base.Hostname = value; }
		}
		
		public new int Port {
			get { return base.Port; }
			set { base.Port = value; }
		}
		
		public string Username {
			get { return _username; }
			set { _username = value; }
		}
		
		public string Password {
			get { return _password; }
			set { _password = value; }
		}
		
		// Events
		
		public event MsnpCommandArrivedHandler CommandArrived {
			add { _commandArrived += value; }
			remove { _commandArrived -= value; }
		}
		
		public event PassportArrivedHandler PassportArrived {
			add { _passportArrived += value; }
			remove { _passportArrived -= value; }
		}
	}
}
