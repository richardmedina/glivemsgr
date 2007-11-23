
using System;
using System.Collections.Generic;

using RickiLib.Types;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpGroupCollection : RickiLib.Types.WatchedCollection <MsnpGroup>
	{
		
		public MsnpGroupCollection ()
		{
		}
		
		public new void Add (MsnpGroup group)
		{
			base.Add (group);
		}
		
		public new void Remove (MsnpGroup group)
		{
			base.Remove (group);
		}
		
		public MsnpGroup SearchById (int id)
		{
			foreach (MsnpGroup group in this)
				if (group.Id == id)
					return group;
			
			return null;
		}
		
		public MsnpGroup GetById (int id)
		{
			foreach (MsnpGroup g in this)
				if (g.Id == id)
					return g;
			
			return null;
		}
		
		// TODO: Find with predicates
		/*
		public MsnpGroupCollection Buscar (MsnpGroup group)
		{
			return Array.Find (this, Diff);
		}
		
		public bool Diff (MsnpGroup group)
		{
			return this.Contains (group); 
		}*/
	}
}
