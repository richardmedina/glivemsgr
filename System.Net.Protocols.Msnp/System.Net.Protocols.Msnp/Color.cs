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
using System.Text.RegularExpressions;
using System.Globalization;

namespace System.Net.Protocols.Msnp
{
	
	
	public class Color
	{
		private int _red;
		private int _green;
		private int _blue;
		
		public static bool TryParse (string colorstr, out Color color)
		{
			Regex regex = new Regex ("[0-9a-fA-F]{6}");
			color = null;
			
			if (regex.IsMatch (colorstr)) {
				color = new Color ();
				
				color.Red = Convert.ToInt32 (colorstr.Substring (0, 2), 16);
				color.Green = Convert.ToInt32 (colorstr.Substring (2, 2), 16);
				color.Blue = Convert.ToInt32 (colorstr.Substring (4, 2), 16);
				return true;
			}
			
			return false;
		}

		public string ToHexString ()
		{
			return string.Format ("{0:X2}{1:X2}{2:X2}", Red, Green, Blue);
		}
						
		public override string ToString ()
		{
			return string.Format ("[Color: Red={0}, Green={1}, Blue={2}]", Red, Green, Blue);
		}
		
		public int Red {
			get { return _red; }
			set { _red = value; }
		}
		
		public int Green {
			get { return _green; }
			set { _green = value; }
		}
		
		public int Blue {
			get { return _blue; }
			set { _blue = value; }
		}
	}
}
