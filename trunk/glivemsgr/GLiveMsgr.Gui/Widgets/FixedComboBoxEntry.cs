
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class FixedComboBoxEntry : FixedEntry
	{
		private string [] emails;
		
		public FixedComboBoxEntry (string [] emails)
		{
			this.emails = emails;
		}
		
		public string [] Emails {
			get {
				return emails;
			}
		}
	}
}
