
using System;
using RickiLib.Widgets; 
using Gtk;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ContactStateCombo : Gtk.ComboBox
	{

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
		
		private Gtk.CellRendererPixbuf pixbufRenderer;
		private Gtk.CellRendererText textRenderer; 
		
		private Gtk.ListStore model;
		
		public ContactStateCombo ()
		{
			model = new Gtk.ListStore (
				typeof (Gdk.Pixbuf),
				typeof (string)
			);
			
			base.Model = model;
			
			populateModel ();
			
			pixbufRenderer = new CellRendererPixbuf ();
			textRenderer = new CellRendererText ();
			
			base.PackStart (pixbufRenderer, false);
			base.PackStart (textRenderer, false);
			
			base.AddAttribute (pixbufRenderer, "pixbuf", 0);
			base.AddAttribute (textRenderer, "text", 1);
			
			base.Active = 0;
			
			base.ModifyBg (StateType.Normal,
				Theme.GetGdkColor (Theme.LogonBackground));
			base.ModifyText (StateType.Normal,
				Theme.GetGdkColor (Theme.LogonEntryForeground));
		}
		
		private void populateModel ()
		{
			Gtk.ListStore model = (Gtk.ListStore) base.Model;
			
			for ( int i = 0; i < menu_labels.Length; i ++)
				model.AppendValues (
					Gdk.Pixbuf.LoadFromResource (image_names [i]),
					menu_labels [i]);
			
		}
	
		public MsnpContactState ContactState {
			get {
				return (MsnpContactState) this.Active;
			}
		}

	}
}
