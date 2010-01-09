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
using REGroup = System.Text.RegularExpressions.Group;
using System.Net.Protocols.Msnp.Core;

namespace System.Net.Protocols.Msnp
{
	
	
	public class TextMessage : Message
	{
		private string _mime_version;
		private string _content_type;
		private string _charset;
		private string _font_name;
		private FontStyle _font_style;
		private Color _color;
		
		private string _text;
		
		private MsnpMessage _msnpmsg;
		
		
		public TextMessage (MsnpMessage msnpmsg) : base (MessageType.Text)
		{
			_msnpmsg = msnpmsg;
		}
		
		public static bool TryParse (MsnpMessage msnpmsg, out TextMessage message)
		{
			message = null;
			string input = msnpmsg.Body; //"MIME-Version: 1.0\r\nContent-Type: text/plain; charset=UTF-8\r\nX-MMS-IM-Format: FN=Lucida%20Console; EF=B; CO=db000; CS=0; PF=31\r\njuguijugui\r\nMundo\r\nComo\r\nEstas\r\n\r\n";
			
			//Regex regex = new Regex (@"MIME-Version:\s(<MIMEVersion>\d*.\d*)\r\nContent-Type:\s(<ContentType>\w*/\w*);");//\r\n[\w\s]i+");
			Regex regex = new Regex (@"MIME-Version:(?<MIMEVersion>\s\d*.\d*)\r\n" +
				@"Content-Type:\s(?<ContentType>\w*/\w*);\scharset=(?<charset>[\w-]*)\r\n" +
				@"X-MMS-IM-Format: FN=(?<FontName>[\w%-]+);\sEF=(?<FontStyle>\w*);\sCO=(?<FontColor>\w*);\sCS=\w*;\sPF=\w*\r\n" +
				@"(?<Text>[\w\r\n-/]*)\r\n"
				);
			
			
			if (regex.IsMatch (input)) {
				Console.WriteLine ("Message Match");
				foreach (Match match in regex.Matches (input)) {
					//REGroup group = match.Groups ["MIME-Version"];
					
					message = new TextMessage (msnpmsg);
					message.MIMEVersion = match.Groups ["MIMEVersion"].ToString ();
					message.ContentType = match.Groups ["ContentType"].ToString ();
					message.Charset = match.Groups ["charset"].ToString ();
					message.FontName = Utils.UrlDecode (match.Groups ["FontName"].ToString ());
					Color color;
					if (Color.TryParse (match.Groups ["MIMEVersion"].ToString (), out color)) {
						message.Color = color;
					}
					
					message.Text = match.Groups ["Text"].ToString ();
					return true;
				}
			}
			
			
			return false;
		}
		
		public string MIMEVersion {
			get { return _mime_version; }
			set { _mime_version = value; }
		}
		
		public string ContentType {
			get { return _content_type; }
			set { _content_type = value; }
		}
		
		public string Charset {
			get { return _charset; }
			set { _charset = value; }
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
		
		public MsnpMessage RawMessage {
			get { return _msnpmsg; }
		}
	}
}
