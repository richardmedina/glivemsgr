// Conversation.cs
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
using System.Net.Protocols.Msnp.Core;

namespace System.Net.Protocols.Msnp
{
	
	
	public class Conversation
	{
		private string _hostname;
		private int _port;
		private Contact _requester;
		
		private string _auth_id1;
		private string _auth_id2;
		
		private Account _account;
		private MsnpSBSession _session;
		
		public Conversation (Account account, MsnpSBSession session)
		{
			_account = account;
			_session = session;
			_session.Username = _account.Username;
			_session.CommandArrived += sessionCommandArrived;
		}
		
		public void Open ()
		{
			_session.Open ();
		}
		
		private void sessionCommandArrived (object sender, MsnpCommandArrivedArgs args)
		{
			
		}
		
		public string Hostname {
			get { return _hostname; }
			set { _hostname = value; }
		}
		
		public int Port {
			get { return _port; }
			set { _port = value; }
		}
		
		public Contact Requester {
			get { return _requester; }
			set { _requester = value; }
		}
		
		public string AuthId1 {
			get { return _auth_id1; }
			set { _auth_id1 = value; }
		}
		
		public string AuthId2 {
			get { return _auth_id2; }
			set { _auth_id2 = value; }
		}
		
		public Account Account {
			get { return _account; }
		}
	}
}
