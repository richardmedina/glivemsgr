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
			//for (int i = 0; i < 10000; i ++)
			//Test ();
			
			//NetTest ();
			//Console.WriteLine ("Main Thread: {0}", Thread.CurrentThread.Name);
		/*	
			MsnpEngine engine = new MsnpEngine (
				"ricardo@innovaciontecnologica.com", 
				"09b9085aa");
			engine.CommandArrived += engineCommandArrived;
			engine.Connect ();
			*/
			// This is a responsive application?
			/*
			while (true) {
				Console.WriteLine ("App Active..");
				System.Threading.Thread.Sleep (1000);
			}
			*/
			
			
			Account account = new Account ();
			Console.Write ("Username: ");
			account.Username = Console.ReadLine ();
			Console.Write ("Password: ");
			account.Password = Console.ReadLine ();
			
			account.LoggedIn += accountLoggedIn;
			//account.CommandArrived += accountCommandArrived;
			account.GroupsLoaded += accountGroupsLoaded;
			account.ContactsLoaded += accountContactsLoaded;
			
			account.Login ();
			Console.WriteLine ("Connecting ..");
			Console.ReadLine ();
		}
		
		private static void accountCommandArrived (object sender, MsnpCommandArrivedArgs args)
		{
			Console.WriteLine ("{0}({1}): {2}", 
				args.Command.ServerType, 
				args.Command.Type, 
				args.Command.RawString);
		}
		
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
			Console.WriteLine ("Contacts Loaded");
		}
	}
}
