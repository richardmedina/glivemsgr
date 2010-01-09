// MsnpAccount.cs
// 
// Copyright (C) 2008 Ricardo Medina <ricki@dana-ide.org>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Net.Protocols.Msnp.Core;

namespace System.Net.Protocols.Msnp
{
	
	public class Account : MsnpEngine
	{
		
		private event EventHandler _loggedIn;
		private event EventHandler _loggedOut;
		
		//private bool _list_loaded;
		private GroupCollection _groups;
		private ContactState _contactState;
		
		private bool _logged = false;
		
		private int _contactListVersion = 0;
		private int _totalGroups = 0;
		private int _totalContacts = 0;
		private int _currentContacts = 0;
		
		private event EventHandler _groupsLoaded;
		private event EventHandler _contactsLoaded;
		private event EventHandler _stateChanged;
		
		private ConversationCollection _conversations;
		
		
		
		public Account () : this (string.Empty, string.Empty)
		{
		}
		
		public Account (string username, string password) : 
			base (username, password)
		{
			_loggedIn = onLoggedIn;
			_loggedOut = onLoggedOut;
			_groups = new GroupCollection ();
			_contactState = ContactState.Offline;
			
			_groupsLoaded += onGroupsLoaded;
			_contactsLoaded += onContactsLoaded;
			_stateChanged = onStateChanged;
			
			_conversations = new ConversationCollection ();
			
			Switchboard.Added += SwitchboardAdded;
			Switchboard.Activated += SwitchboardActivated;
		}
		
		public void SetState (ContactState state)
		{
			
			if (Logged) {
				Dispatch.Send ("CHG {0} {1}",
					Dispatch.TrId ++,
					Utils.ContactStateToString ((int) state));
					return;
			}
			
			State = state;
		}
		
		protected override void OnCommandArrived (MsnpCommand command)
		{
			if (command.ServerType == MsnpClientType.Dispatch) {
				if (command.Type == MsnpCommandType.USR) {
					if (command.Arguments [0] == "OK")
						OnLoggedIn ();
				}
					
				if (command.Type == MsnpCommandType.RNG)
					processRNG (command);
				
				else if (command.Type == MsnpCommandType.SYN)
					processSYN (command);
				
				else if (command.Type == MsnpCommandType.LSG)
					processLSG (command);
				
				else if (command.Type == MsnpCommandType.LST)
					processLST (command);
					
				else if (command.Type == MsnpCommandType.CHG)
					processCHG (command);
				else if (command.Type == MsnpCommandType.CHL)
					processCHL (command);
				else if (command.Type == MsnpCommandType.QRY)
					Console.WriteLine (command.RawString);
				//else if (command.Type == MsnpCommandType
				//if (command.Type == MsnpCommandType.MSG)
				//	processMSG (command);
				
				//if (command.Type == MsnpCommandType.MSG)
				//	processMSG (command);
			}
			
			base.OnCommandArrived (command);
		}
		
		protected virtual void OnContactsLoaded ()
		{
			_contactsLoaded (this, EventArgs.Empty);
		}
		
		protected virtual void OnGroupsLoaded ()
		{
			_groupsLoaded (this, EventArgs.Empty);
		}
		
		protected virtual void OnLoggedIn ()
		{
			_loggedIn (this, EventArgs.Empty);
			//personal_msg ("Ricki hola");
		}
		/*
		private void personal_msg (string msg)
		{
			
			string message = "<Data><PSM>" + 
				msg +
				"</PSM><CurrentMedia></CurrentMedia></Data>";
			
			string final_msg = string.Format ("UUX {0} {1}\r\n{2}",
				Dispatch.TrId ++, message.Length, message);
			Console.WriteLine ("Setting message: {0}", final_msg);
			Dispatch.Send (final_msg);
			
		}
		*/
		protected virtual void OnLoggedOut ()
		{
			_loggedOut (this, EventArgs.Empty);
		}
		
		protected virtual void OnStateChanged ()
		{
			_stateChanged (this, EventArgs.Empty);
		}
		
		private void processCHG (MsnpCommand command)
		{
			
			int state = Utils.StringToContactState 
				(command.Arguments [0]);
			
			_contactState = (ContactState) state;
			
			OnStateChanged ();
		}
		
		private void processCHL (MsnpCommand command)
		{
			string static_key = "Q1P7W2E4J9R8U3S5";
			
			// zero position in array contains the hex number generated by server
			string hex_num = command.Arguments [0];
			
			string encoded = Utils.MD5Sum (string.Format ("{0}{1}", hex_num, static_key));
			
			Console.WriteLine ("Received CHL.");
			Console.WriteLine ("QRY {0} msmsgs@msnmsgr.com {1}\r\n{2}", Dispatch.TrId, encoded.Length, encoded);
			//Console.WriteLine ("QRY {0} msmsgs@msnmsgr.com {1}", Dispatch.TrId ++, encoded.Length);
			
			Dispatch.RawSend ("QRY {0} msmsgs@msnmsgr.com {1}\r\n{2}", Dispatch.TrId ++, encoded.Length, encoded);
			//Dispatch.Read ();
			//CHL 0 35233191528463256281
		}
		
		private void processLSG (MsnpCommand command)
		{
			if (command.Type != MsnpCommandType.LSG)
				throw new ArgumentException ("processLSG");
			
			int groupid;
			
			if (!int.TryParse (command.Arguments [0], out groupid))
				throw new ArgumentException ("processLSG");
			
			Group group = new Group (groupid, 
				command.Arguments [1]);
			
			Groups.Add (group);
			
			if (Groups.Count == _totalGroups)
				OnGroupsLoaded ();
		}
		
		private void processLST (MsnpCommand command)
		{
			//Console.WriteLine ("processLST");
			if (command.Type != MsnpCommandType.LST)
				throw new ArgumentException ("processLST");
						
			Contact contact = new Contact (
				this,
				command.Arguments [0], 
				command.Arguments [1]);
			
			int list_mask;
			bool contact_added = false;
			
			if (!int.TryParse (command.Arguments [2], out list_mask))
				throw new ArgumentException ("processLST");
			
			contact.Lists = (ListType) list_mask;
			
			if (command.Arguments.Length == 4)
				foreach (string strid in 
					command.Arguments [3].Split 
						(",".ToCharArray ())) {
	
					int id;
					if (!int.TryParse (strid, out id))
						throw new ArgumentException 
							("processLST");

					Group group;
					if (Groups.GetById (id, out group)) {
						group.Add (contact);
						contact_added = true;
					}
				}
			
			if (!contact_added) {
				Group gothers;
				
				if (!Groups.GetById (-1, out gothers)) {
					gothers = new Group (-1, "Other Contacts");
					Groups.Add (gothers);
				}
				
				gothers.Add (contact);
			}
			
			_currentContacts ++;
			//Console.WriteLine ("Loaded Contacts {0}/{1}",
			//	_currentContacts, _totalContacts);
			if (_currentContacts == _totalContacts)
				OnContactsLoaded ();
		}
				
		private void processRNG (MsnpCommand command)
		{
			// an incomming message
			//printCommandArgs (command);
			
		}
		
		private void processSYN (MsnpCommand command)
		{
			if (command.Type == MsnpCommandType.SYN) {
				if (!int.TryParse (command.Arguments [0], out _contactListVersion))
					throw new ArgumentException ("processSYN");
				if (!int.TryParse (command.Arguments [1], out _totalContacts))
					throw new ArgumentException ("processSYN");
				if (!int.TryParse (command.Arguments [2], out _totalGroups))
					throw new ArgumentException ("processSYN");
			} else
				throw new ArgumentException ("processSYN");
		}
/*		
		private void printCommandArgs (MsnpCommand command)
		{
			Console.Write ("{0}({1}", command.Type, command.TrId);
			foreach (string arg in command.Arguments)
				Console.Write (" {0}", arg);
			Console.WriteLine ();
		}
*/				
		// Handlers

		private void onContactsLoaded (object sender, EventArgs args)
		{
		}
		
		private void onGroupsLoaded (object sender, EventArgs args)
		{
		}
				
		private void onLoggedIn (object sender, EventArgs args)
		{
				_logged = true;
		}
		
		private void onLoggedOut (object sender, EventArgs args)
		{
				_logged = false;
		}
		
		private void onStateChanged (object sender, EventArgs args)
		{
		}
		
		private void SwitchboardAdded (object sender, SBSessionCollectionEventArgs args)
		{
			Console.WriteLine ("Adding conversation to pool");
			Conversation conversation = new Conversation (this, args.Session);
			Conversations.Add (conversation);
			conversation.Open ();
		}
		
		private void SwitchboardActivated (object sender, SBSessionCollectionEventArgs args)
		{
			Console.WriteLine ("Activating conversation.. DONE");
			foreach (Conversation conversation in Conversations)
				if (conversation.Session == args.Session)
					conversation.Open ();
		}
		
		public event EventHandler LoggedIn {
			add { _loggedIn += value; }
			remove { _loggedIn -= value; }
		}
		
		public event EventHandler LoggedOut {
			add { _loggedOut += value; }
			remove { _loggedOut -= value; }
		}
		
		public int ContactListVersion {
			get { return _contactListVersion; }
		}
		
		public GroupCollection Groups {
			get { return _groups; }
		}
		/*
		public bool ListLoaded {
			get { return _list_loaded; }
		}
		*/
		public bool Logged {
			get { return _logged; }
		}
		
		public ContactState State {
			get { return _contactState; }
			protected set { _contactState = value; }
		}
		
		public ConversationCollection Conversations {
			get { return _conversations; }
		}
		
		// Events
		
		public event EventHandler ContactsLoaded {
			add { _contactsLoaded += value; }
			remove { _contactsLoaded -= value; }
		}
		
		public event EventHandler GroupsLoaded {
			add { _groupsLoaded += value; }
			remove { _groupsLoaded -= value; }
		}
		
		public event EventHandler StateChanged {
			add { _stateChanged += value; }
			remove { _stateChanged -= value; }
		}
	}
}
