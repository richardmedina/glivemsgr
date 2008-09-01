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
	
	public class MsnpAccount : MsnpEngine
	{
		
		private event EventHandler _loggedIn;
		private event EventHandler _loggedOut;
		
		public MsnpAccount () : this (string.Empty, string.Empty)
		{
		}
		
		public MsnpAccount (string username, string password) : 
			base (username, password)
		{	
			_loggedIn = onLoggedIn;
			_loggedOut = onLoggedOut;
		}
		
		public void Login ()
		{
			Connect ();
		}
		
		protected override void OnCommandArrived (MsnpCommand command)
		{
			if (command.ServerType == MsnpServerType.Dispatch) {
				if (command.Type == MsnpCommandType.USR) {
					if (command.Arguments [0] == "OK")
						OnLoggedIn ();
				}
			}
			
			base.OnCommandArrived (command);
		}
		
		protected virtual void OnLoggedIn ()
		{
			_loggedIn (this, EventArgs.Empty);
		}
		
		protected virtual void OnLoggedout ()
		{
			_loggedOut (this, EventArgs.Empty);
		}

		private void onLoggedIn (object sender, EventArgs args)
		{
		}
		
		private void onLoggedOut (object sender, EventArgs args)
		{
		}
				
		// Events
		
		public event EventHandler LoggedIn {
			add { _loggedIn += value; }
			remove { _loggedIn -= value; }
		}
	}
}
