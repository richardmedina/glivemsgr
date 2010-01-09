//  
//  Copyright (C) 2009 Ricardo Medina
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Collections.Generic;

namespace System.Net.Protocols.Msnp
{
	
	
	public class ConversationCollection : 
		System.Collections.Generic.List <Conversation>
	{
		private event ConversationEventHandler _added;
		private event ConversationEventHandler _removed;
		
		public ConversationCollection ()
		{
			_added = onAdded;
			_removed = onRemoved;
		}
		
		public virtual void OnAdded (Conversation conversation)
		{
			_added (this, new ConversationEventArgs (conversation));
		}
		
		public virtual void OnRemoved (Conversation conversation)
		{
			_removed (this, new ConversationEventArgs (conversation));
		}
		
		private void onAdded (object sender, ConversationEventArgs args)
		{
		}
		
		private void onRemoved (object sender, ConversationEventArgs args)
		{
		}
		
		public event ConversationEventHandler Added {
			add { _added += value; }
			remove { _removed -= value; }
		}
		
		public event ConversationEventHandler Removed {
			add { _removed = value; }
			remove { _removed = value; }
		}
	}
}
