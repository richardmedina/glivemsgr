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
	
	
	public class MsnpAccount 
	{
		private MsnpEngine _engine;
		
		private string _alias;
		
		private EventHandler _connected;
		private EventHandler _disconnected;
		
		public MsnpAccount (string username, string password)
		{
			_engine = new MsnpEngine (username, password);
		}
		
		public void Connect ()
		{
			_engine.Connect ();
			
		}
		
		public MsnpEngine Engine {
			get { return _engine; }
		}
		
		// Events
				
		public event EventHandler Connected {
			add { }
			remove { }
		}
	}
}
