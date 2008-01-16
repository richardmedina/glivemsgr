
using System;
using Gtk;
using RickiLib.Widgets;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ContactStateMenu : Gtk.Menu
	{
		private MsnpContactState state;
		private ExtendedMenuItem [] menus;
		
		public event EventHandler Changed;
		
		private string [] menu_labels = {
			"Online",
			"Busy",
			"Be right back",
			"Away",
			"Phone",
			"Gone to lunch",
			"Invisible"
		};
		
		private string [] image_names = {
			"online.png",
			"busy.png",
			"brb.png",
			"away.png",
			"phone.png",
			"lunch.png",
			"offline.png",
		};
		
		public ContactStateMenu () : this (MsnpContactState.Offline)
		{
		}
		
		//FIXME: Find better way to change state 
		// app crashes when try to change internal value
		// but throw Changed event and the account is not initialized
		public ContactStateMenu (MsnpContactState state)
		{
			Changed = onChanged;
			
			menus = new ExtendedMenuItem [menu_labels.Length];
			
			for (int i = 0; i < menu_labels.Length; i ++) {
				menus [i] = createImageMenuItem (i);
				base.Append (menus [i]);
			}
			//State = state;
			
			base.ShowAll ();
		}
		
		public ExtendedMenuItem GetMenuItem (MsnpContactState state)
		{
			return menus [(int) state];
		}
		
		public ExtendedMenuItem GetSelectedMenuItem ()
		{
			return GetMenuItem (State);
		}
		
		protected virtual void OnChanged ()
		{
			Changed (this, EventArgs.Empty);
		}
		
		private void onChanged (object sender, EventArgs args)
		{
			int stat = (int) State;
			for (int i = 0; i < menus.Length; i ++) {
				ExtendedMenuItem item = (ExtendedMenuItem) menus [i];
				if (i == stat)
					item.Text = string.Format ("<b>{0}</b>",
						menu_labels [i]);
				else
					item.Text = menu_labels [i];
				
			}
				
			//item.Text = string.Format ("<b>{0}</b>", 
			//	menu_labels [(int) State]);
		}

		private ExtendedMenuItem createImageMenuItem (int index)
		{			
			ExtendedMenuItem item = new ExtendedMenuItem (
				Gtk.Image.LoadFromResource (image_names [index]),
				menu_labels [index],
				true);
			
			item.Activated += menu_Activate;
			
			return item;
		}

		private void menu_Activate (object sender, EventArgs args)
		{
			Gtk.ImageMenuItem item = (Gtk.ImageMenuItem) sender;
			for (int i = 0; i < menus.Length; i ++)
				if (menus [i] == item) {
					State = (MsnpContactState) i;
					return;
				}
		}
		
		public new MsnpContactState State {
			get { return state; }
			set { 
				state = value;
				OnChanged ();
			}
		}
	}
}
