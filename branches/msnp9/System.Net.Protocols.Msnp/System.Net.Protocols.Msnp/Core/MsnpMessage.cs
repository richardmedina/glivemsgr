// MsnpMessage.cs
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
	
	
	public class MsnpMessage
	{
		private MsnpCommand _command;
		private string _body;
		
		public MsnpMessage (MsnpCommand command, string body)
		{
			_command = command;
			_body = body;
		}
		
		public MsnpCommand Command {
			get { return _command; }
		}
		
		public string Body {
			get { return _body; }
		}
	}
}
