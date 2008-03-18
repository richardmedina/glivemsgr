
using System;
using Gtk;
using Cairo;

using RickiLib.Widgets;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ContactListHeader : Gtk.VBox
	{
		private Gtk.Image displayPic;
		
		private Gtk.HBox hbox;
		
		
		private EditableLabelButton elbMsg;
		private AliasChangeButton _aliasButton;
		
		private MsnpAccount account;
		
		public ContactListHeader (MsnpAccount account)
		{
			this.account = account;
			
			Gdk.Pixbuf pixbuf = Gdk.Pixbuf.LoadFromResource 
				("user_display_picture_default.png");
			
			pixbuf = pixbuf.ScaleSimple (60, 60, Gdk.InterpType.Nearest);
			
			displayPic = new Gtk.Image (pixbuf);
			
			_aliasButton = new AliasChangeButton ();
			
			elbMsg = new EditableLabelButton ();
			
			elbMsg.HBox.PackStart (
				new Arrow (ArrowType.Down, ShadowType.None), 
				false, false, 0);
			
			hbox = new HBox ();
			
			Gtk.VBox vbox = new VBox ();
			vbox.BorderWidth = 10;
			
			Gtk.Viewport viewport1 = new Viewport ();
			viewport1.ShadowType = ShadowType.EtchedOut;
			viewport1.Add (displayPic);
			
			vbox.PackStart (viewport1, false, false, 0);
			
			hbox.PackStart (vbox, false, false, 5);
			
			vbox = new VBox ();
			vbox.BorderWidth = 10;
			vbox.PackStart (_aliasButton, false, false, 10);
			vbox.PackEnd (elbMsg, false, false, 0);

			hbox.PackStart (vbox);
			
			vbox.HeightRequest = 85;
			
			PackStart (hbox);
		}
		
		public AliasChangeButton AliasButton {
			get { return _aliasButton; }
		}
		
		public EditableLabelButton CustomMessage {
			get { return elbMsg; }
		}
		
		public MsnpAccount Account {
			get { return account; }
		}
	}
}
