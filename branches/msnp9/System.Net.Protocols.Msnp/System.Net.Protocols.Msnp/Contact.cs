// Contact.cs
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
	
	
	public class Contact
	{
		private string _username;
		private string _alias;
		
		private Account _account;
		private ListType _lists;
		
		
		private event EventHandler _stateChanged; 
		
		private ConversationArrivedHandler _conversationArrived;
		
		public Contact (Account account, string username) :
			this (account, username, string.Empty)
		{
		}
		
		public Contact (
			Account dispatch,
			string username, 
			string alias)
		{
			_username = username;
			_alias = alias;
			_account = dispatch;
			_account.CommandArrived += dispatchCommandArrived;
		}

		private void dispatchCommandArrived (object sender, 
			MsnpCommandArrivedArgs args)
		{
			if (args.Command.ServerType == MsnpClientType.Dispatch)
				if (args.Command.Type == MsnpCommandType.RNG) {
					//Console.WriteLine (args.Command.RawString);
				/*	Console.WriteLine ("{0} == {1}",
						args.Command.Arguments [4],
						Username);
				*/		
					/*if (args.Command.Arguments [4] == Username) {
						Console.WriteLine (
							"Me ({0}) Wants to chat with you",
							Username);
						Console.WriteLine (args.Command.RawString);
						
					}*/
				}
		}
		
		public string Username {
			get { return _username; }
		}
		
		public string Alias {
			get { return _alias; }
		}
		
		public Account Account {
			get { return _account; }
		}
		
		public ListType Lists {
			get { return _lists; }
			set { _lists = value; }
		}
		
		public event ConversationArrivedHandler ConversationArrived {
			add { _conversationArrived += value; }
			remove { _conversationArrived -= value; }
		}
		
		public event EventHandler StateChanged {
			add { _stateChanged += value; }
			remove { _stateChanged -= value; }
		}
	}
}
