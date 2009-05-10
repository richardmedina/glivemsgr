// MsnpMessageHandler.cs
// 
// Copyright (C) 2009 Ricardo Medina López
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

namespace System.Net.Protocols.Msnp.Core
{
	
	public delegate void MsnpMessageHandler (object sender, 
		MsnpMessageArgs args);
	
	public class MsnpMessageArgs
	{
		private MsnpMessage _message;
		
		public MsnpMessageArgs (MsnpMessage message)
		{
			_message = message;
		}
		
		public MsnpMessage Message {
			get { return _message; }
		}
	}
}
