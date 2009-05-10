// Program.cs
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
using System.Threading;
using System.Net.Protocols.Msnp;
using System.Net.Protocols.Msnp.Core;
namespace MsnpTesting
{
	public class Program
	{
		
		static void Main(string[] args)
		{
			Account account = new Account ();
			Console.Write ("Username: ");
			account.Username = Console.ReadLine ();
			Console.Write ("Password: ");
			account.Password = Console.ReadLine ();
			
			account.LoggedIn += accountLoggedIn;
			//account.CommandArrived += accountCommandArrived;
			//account.GroupsLoaded += accountGroupsLoaded;
			account.ContactsLoaded += accountContactsLoaded;
			account.StateChanged += accountStateChanged;
			
			
			account.Login ();
			Console.WriteLine ("Connecting ..");
			Console.ReadLine ();
		}
		/*
		private static void accountCommandArrived (object sender, MsnpCommandArrivedArgs args)
		{
			Console.WriteLine ("{0}({1}): {2}", 
				args.Command.ServerType, 
				args.Command.Type, 
				args.Command.RawString);
		}
		*/
		private static void accountLoggedIn (object sender, EventArgs args)
		{
			Console.WriteLine ("Logged!");
			((Account) sender).SetState (ContactState.Online);
		}
		
		private static void accountGroupsLoaded (object sender, EventArgs args)
		{
			Console.WriteLine ("{0} Groups Loaded..", ((Account) sender).Groups.Count);
			foreach (Group group in ((Account)sender).Groups)
				Console.WriteLine ("   Name:{0} id:{1}",
					group.Name, group.Id);
		}
		
		private static void accountContactsLoaded (object sender, EventArgs args)
		{
			Account account = (Account) sender; 
			foreach (Group group in account.Groups) {
				Console.WriteLine (group.Name);
				foreach (Contact contact in group)
					Console.WriteLine ("\t{0}",contact.Username);
			}
			//Console.WriteLine ("Contacts Loaded");
		}
		
		private static void accountStateChanged (object sender, EventArgs args)
		{
			Console.WriteLine ("Your state is now {0}", 
				Utils.ContactStateToString ((int) ((Account)sender).State));
		}
	}
}
