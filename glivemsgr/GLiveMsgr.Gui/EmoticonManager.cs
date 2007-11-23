
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using System.Net.Protocols;

namespace GLiveMsgr.Gui
{
	
	
	public class EmoticonManager : System.Collections.Generic.List <Emoticon>, IXmlSerializable
	{		
		public EmoticonManager ()
		{
			//CloseSession ();
		}
		
		public new void Add (Emoticon emoticon)
		{
			base.Add (emoticon);
		}
		
		public new void Remove (Emoticon emoticon)
		{
			base.Remove (emoticon);
		}
		
		public void LoadSession (string session)
		{
			CloseSession ();
		}
		
		public void SaveSession ()
		{
			//foreach (Emoticon in this
			
		}
		
		public void CloseSession ()
		{
			base.Clear ();
			loadStock ();
		}
		
		private void loadStock ()
		{
			//for (int x =0; x < 7; x ++)
			for (int i = 0; i < stock_launchers.Length; i ++) {
				this.Add (
					new Emoticon (
						string.Format ("{0}", stock_launchers [i]),
						stock_images [i],
						true));
			}
			
			this.Add (new Emoticon ("login", "loganim.gif"));
		}
		
		private static string [] stock_launchers = {
			":)",
			";)",
			":(",
			":d",
			":@",
			":p",
			":'(",
			":$",
			":s",
		};
		
		private static string [] stock_images = {
			"Smiley.png",
			"Winking_smiley.png",
			"Sad_smiley.png",
			"Open_mouthed_smiley.png",
			"Angry_smiley.png",
			"Smiley_with_tonque_out.png",
			"Crying_face.png",
			"Embarrassed_smiley.png",
			"Confused_smiley.png",
			"Hot_smiley.png"
		};
		
		// Serialization
		
		public void ReadXml (XmlReader reader)
		{
			
		}
		
		public void WriteXml (XmlWriter writer)
		{
		
		}
		
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}
		
	}
}
