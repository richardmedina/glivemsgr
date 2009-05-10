// MsnpDispatchServer.cs
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
using System.Net;
using System.Net.Security;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net.Protocols.Msnp.Core
{


	public class MsnpDispatchServer : MsnpClient
	{
		private string _username;
		private string _password;
		private int _trId;
		private int _list_version = 0;
		
		private event PassportArrivedHandler _passportArrived;
		private event MsnpMessageHandler _message_arrived;
		
		public MsnpDispatchServer () : 
			this (string.Empty, 
				0, 
				string.Empty, 
				string.Empty, 
				0)
		{
		}

		public MsnpDispatchServer (string hostname, int port, string username, string password, int trId)
			: base(MsnpClientType.Dispatch)
		{
			_username = username;
			_password = password;
			_trId = trId;
			_passportArrived = onPassportArrived;
			_message_arrived = onMessageArrived;
			
			Hostname = hostname;
			Port = port;
			
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
		
		protected override void OnCommandArrived (MsnpCommand command)
		{
//			Console.WriteLine ("Dispatch({0}):{1}", 
	//			command.Type.ToString (),
		//		command.RawString);
			switch (command.Type) {
				case MsnpCommandType.VER:
					processVER (command);
				break;
			
				case MsnpCommandType.CVR:
					processCVR (command);
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
					processUSR (command);
				break;
				case MsnpCommandType.MSG:
					processMSG (command);
				break;
			}
			
			base.OnCommandArrived (command);
			
		//	Console.WriteLine ("Dispatch: processCommand Ends");
		}
		
		protected override void OnConnected ()
		{
			base.OnConnected ();
			StartAsynchronousReading ();
			
			//Console.WriteLine ("Dispatch. Connected!");
			
			Send ("VER {0} MSNP8 MSNP9 CVR0", TrId ++);
		}

		/*
		protected override void OnDataArrived (string data)
		{
			MsnpCommand command = MsnpCommand.Parse (data);
			OnCommandArrived (command);
			
			base.OnDataArrived (data);
		}
		*/
		
		protected virtual void OnMessageArrived (MsnpMessage message)
		{
			if (message.Command.Arguments [0] == "Hotmail") {
				Send ("SYN {0} {1}", TrId ++, _list_version);
			}
			_message_arrived (this, new MsnpMessageArgs (message));
		}
		protected virtual void OnPassportArrived (string content)
		{
			_passportArrived (this, new PassportArrivedArgs (content));
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
		//	Console.WriteLine ("Nexus");
			WebRequest req;
			WebResponse res;
			
			req = WebRequest.Create ("https://nexus.passport.com/rdr/pprdr.asp");
			
			try {
				 //Console.Write ("WebConnecting to {0}...", req.RequestUri);
				 res = req.GetResponse ();
				 //Console.WriteLine ("DONE");
			} catch (Exception e) {
				//Console.WriteLine (e.ToString ());
				Console.WriteLine ("Nexus.Error Connecting to {0}.{1}", 
					req.RequestUri, e.Message);
				return string.Empty;
			}
			
			Regex regex = new Regex ("dalogin=([^,]+)", RegexOptions.IgnoreCase);
			
			System.Text.RegularExpressions.GroupCollection group = 
				regex.Match (res.Headers ["PassportURLs"]).Groups;
			
			req = WebRequest.Create ("https://" + group [1].Value.TrimEnd ());
			req.Headers.Add ("Authorization", cookie);
			
			for (int retry = 0; retry < 1; retry ++) {
				try {
					//Console.Write ("WebConnecting to {0}...", req.RequestUri);
					res = req.GetResponse ();
					//Console.WriteLine ("DONE");
				} catch (Exception e) {
					//Console.WriteLine (e);
					Console.WriteLine ("Nexus.Error connecting to: {0}.{1}", 
						req.RequestUri, e.Message);
					return string.Empty;
				}
				
				regex = new Regex ("da-status=([^,]+)", RegexOptions.IgnoreCase);
				string info = string.Empty;
				
				if (res.Headers ["WWW-Authenticate"] != null)
					info = res.Headers ["WWW-Authenticate"];
				else if (res.Headers ["Authentication-Info"] != null)
					info = res.Headers ["Authentication-Info"];
			//	Console.WriteLine ("Info : {0}", info);
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
		
		private void processMSG (MsnpCommand command)
		{
			int size;
			
			if (int.TryParse (command.Arguments [2], out size)) {
				string buffer = Read (size);
				MsnpMessage msg = new MsnpMessage (command, buffer);
				
				OnMessageArrived (msg);
			}
		}
		
		private void processUSR (MsnpCommand command)
		{
			if (command.Arguments [0] == "TWN") {
				string cookie = formatCookie (command.Arguments [2]);
				string ticket = nexusLogin (cookie);
				if (ticket == string.Empty) {
					base.OnCommandArrived (command);
						Close ();
						return;
				}
				Send ("USR {0} TWN S {1}", TrId ++, ticket);
			}
		}
		
		private void processVER (MsnpCommand command)
		{
			Send ("CVR {0} 0x0C0A winnt 5.1 i386 MSNMSGR 6.0.0602 " +
				"MSMSGS {1}", TrId ++, _username);
		}
		
		private void processCVR (MsnpCommand command)
		{
			Send ("USR {0} TWN I {1}",
				TrId ++, _username);
		}
		
		private void onMessageArrived (object sender, MsnpMessageArgs args)
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
		
		public int ListVersion {
			get { return _list_version; }
			set { _list_version = value; }
		}
		
		// Events
		
		public event MsnpMessageHandler MessageArrived {
			add { _message_arrived += value; }
			remove { _message_arrived -= value; }
		}
		public event PassportArrivedHandler PassportArrived {
			add { _passportArrived += value; }
			remove { _passportArrived -= value; }
		}
	}
}
