// ErrorEventHandler.cs
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

namespace System.Net.Protocols.Msnp.Core
{
	
	public delegate void ErrorEventHandler (object sender, ErrorEventArgs args);
	
	public class ErrorEventArgs : System.EventArgs
	{
		
		private int _code;
		private string _message;
		
		public ErrorEventArgs (int code, string message)
		{
			_code = code;
			_message = message;
		}
		
		public int Code {
			get { return _code; }
		}
		
		public string Message {
			get { return _message; }
		}
	}
}
