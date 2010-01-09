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

namespace System.Net.Protocols.Msnp
{
	
	public delegate void MessageEvent (object sender, MessageEventArgs args);
	
	public class MessageEventArgs
	{
		private Message _message;
		
		public MessageEventArgs (Message message)
		{
			_message = message;
		}
		
		public Message Message {
			get { return _message; }
			set { _message = value; }
		}
	}
}
