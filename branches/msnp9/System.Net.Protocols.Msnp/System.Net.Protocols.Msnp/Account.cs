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
		
		
		private GroupCollection _groups;
		
		private int _contactListVersion = 0;
		private int _totalGroups = 0;
		private int _totalContacts = 0;
		private int _currentContacts = 0;
		
		private event EventHandler _groupsLoaded;
		private event EventHandler _contactsLoaded;
		
		public Account () : this (string.Empty, string.Empty)
		{
		}
		
		public Account (string username, string password) : 
			base (username, password)
		{	
			_loggedIn = onLoggedIn;
			_loggedOut = onLoggedOut;
			_groups = new GroupCollection ();
			
			_groupsLoaded += onGroupsLoaded;
			_contactsLoaded += onContactsLoaded;
		}
		
		public void Login ()
		{
			Connect ();
		}
		
		public void SetState (ContactState state)
		{
			Dispatch.Send ("CHG {0} {1}",
				Dispatch.TrId ++,
				Utils.ContactStateToString ((int) state));
		}
		
		protected override void OnCommandArrived (MsnpCommand command)
		{
			if (command.ServerType == MsnpServerType.Dispatch) {
				if (command.Type == MsnpCommandType.USR) {
					if (command.Arguments [0] == "OK")
						OnLoggedIn ();
				}
				
				if (command.Type == MsnpCommandType.RNG) {
					Console.WriteLine 
						("Someone wants to chat with you..");
					processRNG ();
				}
				
				if (command.Type == MsnpCommandType.SYN)
					processSYN (command);
				
				if (command.Type == MsnpCommandType.LSG)
					processLSG (command);
				
				if (command.Type == MsnpCommandType.LST)
					processLST (command);
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
		}
		
		protected virtual void OnLoggedOut ()
		{
			_loggedOut (this, EventArgs.Empty);
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
			if (command.Type != MsnpCommandType.LST)
				throw new ArgumentException ("processLST");
						
			Contact contact = new Contact (
				Dispatch,
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
			
			if (_currentContacts == _totalContacts)
				OnContactsLoaded ();
		}
		
		private void processRNG ()
		{
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
				
		// Handlers

		private void onContactsLoaded (object sender, EventArgs args)
		{
		}
		
		private void onGroupsLoaded (object sender, EventArgs args)
		{
		}
				
		private void onLoggedIn (object sender, EventArgs args)
		{
		}
		
		private void onLoggedOut (object sender, EventArgs args)
		{
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
		
		// Events
		
		public event EventHandler ContactsLoaded {
			add { _contactsLoaded += value; }
			remove { _contactsLoaded -= value; }
		}
		
		public event EventHandler GroupsLoaded {
			add { _groupsLoaded += value; }
			remove { _groupsLoaded -= value; }
		}
	}
}
