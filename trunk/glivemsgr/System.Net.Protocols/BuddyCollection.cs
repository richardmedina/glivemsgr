
using System;
using System.Collections;
using System.Collections.Generic;

using RickiLib.Types; 

namespace System.Net.Protocols
{
	
	
	public class BuddyCollection : RickiLib.Types.WatchedCollection <Buddy>
	{
		
		public BuddyCollection()
		{
		}
		
		public new void Add (Buddy buddy)
		{
			base.Add (buddy);
		}
		
		public new void Remove (Buddy buddy)
		{
			base.Remove (buddy);
		}
		
		public new void RemoveAt (int index)
		{
			base.RemoveAt (index);
		}
		
		public Buddy GetByUsername (string username)
		{
			foreach (Buddy buddy in this)
				if (buddy.Username == username)
					return buddy;
			
			return null;
		}
		
		public Buddy FindBuddy (string username)
		{
			foreach (Buddy buddy in this)
				if (buddy.Username == username)
					return buddy;
			
			return null;
		}
	}
	
}
