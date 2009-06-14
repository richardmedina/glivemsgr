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
	
	
	public class TextMessage : Message
	{
		private string _font_name;
		private FontStyle _font_style;
		private Color _color;
		
		private string _text;
		
		public TextMessage () : base (MessageType.Text)
		{
		}
		
		public bool TryParse (string messagestring, out TextMessage message)
		{
			message = null;
			
			return false;
		}
		
		public string FontName {
			get { return _font_name; }
			set { _font_name = value; }
		}
		
		public FontStyle FontStyle {
			get { return _font_style; }
			set { _font_style = value; }
		}
		
		public Color Color {
			get { return _color; }
			set { _color = value; }
		}
		
		public string Text {
			get { return _text; }
			set { _text = value; }
		}
	}
}
