
using System;
using System.Drawing;

namespace GLiveMsgr.Gui
{
	
	
	public class Emoticon
	{
		private string filename;
		private string trigger;
		
		private bool isStock;
		
		
		public Emoticon (string trigger, string filename) :
			this (trigger, filename, false)
		{
		}
		
		public Emoticon (string trigger, string filename, bool isStock)
		{
			this.filename = filename;
			this.trigger = trigger;
			this.IsStock = isStock;
		}
		
		public string Trigger {
			get {
				return trigger;
			}
		}
		
		public string Filename {
			get {
				return string.Format ("smileys/{0}", filename);
			}
		}
		
		public bool IsStock {
			get { return isStock; }
			internal set { isStock = value; }
		}
	}
}
