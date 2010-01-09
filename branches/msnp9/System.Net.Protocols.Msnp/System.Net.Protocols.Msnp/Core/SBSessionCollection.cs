// SBSessionCollection.cs
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
using System.Collections.Generic;

namespace System.Net.Protocols.Msnp.Core
{
	
	
	public class SBSessionCollection : 
		System.Collections.Generic.List <MsnpSBSession>
	{
		
		private event SBSessionCollectionEventHandler _added;
		private event SBSessionCollectionEventHandler _removed;
		// When session was clossed and require rebuild, commonly MsnpCommand is replaced
		private event SBSessionCollectionEventHandler _activated;
		
		public SBSessionCollection ()
		{
			_added = onAdded;
			_removed = onRemoved;
			_activated = onActivated;
		}
		
		public new void Add (MsnpSBSession session)
		{
			base.Add (session);
			OnAdded (session);
		}
		
		public new void Remove (MsnpSBSession session)
		{
			base.Remove (session);
		}
		
		public void Activate (MsnpSBSession session, MsnpCommand command)
		{
			session.Command = command;
			OnActivated (session);
		}
		
		protected void OnActivated (MsnpSBSession session)
		{
			_activated (this, new SBSessionCollectionEventArgs (session));
		}
		protected virtual void OnAdded (MsnpSBSession session)
		{
			_added (this, new SBSessionCollectionEventArgs (session));
		}
		
		protected virtual void OnRemoved (MsnpSBSession session)
		{
			_removed (this, new SBSessionCollectionEventArgs (session));
		}
		
		
		private void onAdded (object sender, SBSessionCollectionEventArgs args)
		{
		}
		
		private void onRemoved (object sender, SBSessionCollectionEventArgs args)
		{
		}
		
		private void onActivated (object sender, SBSessionCollectionEventArgs args)
		{
		}
		
		public event SBSessionCollectionEventHandler Added {
			add { _added += value; }
			remove { _added -= value; }
		}
		
		public event SBSessionCollectionEventHandler Removed {
			add { _removed += value; }
			remove { _removed -= value; }
		}
		
		public event SBSessionCollectionEventHandler Activated {
			add { _activated += value; }
			remove { _activated -= value; }
		}
	}
}
