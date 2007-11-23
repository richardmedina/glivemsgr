
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class RitchAnchorEmoticon : IRitchAnchor
	{
		private Emoticon emoticon;
		private Gtk.EventBox eventbox;
		private Gtk.Image image;
		private Gtk.TextChildAnchor anchor;
		private Gtk.TextIter iter;
		private Gtk.TextMark mark;
		
		private Gdk.Cursor overCursor;
		
		private Gtk.Menu menu;
		
		public RitchAnchorEmoticon (Emoticon emoticon)
		{
			this.iter = TextIter.Zero;
			this.emoticon = emoticon;
			this.eventbox = new EventBox ();
			this.menu = createMenu ();
			
			if (!System.IO.File.Exists (emoticon.Filename))
				this.image = new Image (Stock.MissingImage, 
					IconSize.Menu);
			else
				this.image = new Image (emoticon.Filename);
			
			this.overCursor = new Gdk.Cursor (Gdk.CursorType.Hand2);
			
			eventbox.Add (image);
			
			if (!this.emoticon.IsStock) {
				eventbox.EnterNotifyEvent += delegate {
					eventbox.GdkWindow.Cursor = overCursor;
				};
				eventbox.ButtonReleaseEvent += 
					delegate (object sender, 
						ButtonReleaseEventArgs args){
						if (args.Event.Button == 3) {
							menu.Popup ();
						}
					};
			}
			
			eventbox.ModifyBg (StateType.Normal,
				new Gdk.Color (0xFF, 0xFF, 0xFF));
			
			eventbox.ShowAll ();
			//image.Pixbuf = new Gdk.Pixbuf (emoticon.Filename);
		}
		
		private Gtk.Menu createMenu ()
		{
			Gtk.Menu men = new Menu ();
			ImageMenuItem item = new ImageMenuItem ();
			
			//Gtk.Image img = new Image (Stock.Save,
			//	IconSize.Menu);
			
			item = createMenuItemFromStock (Stock.Save, 
				"Save");
			men.Append (item);
			
			item = createMenuItemFromStock (Stock.SaveAs,
				"Save as...");
			item.Activated += delegate {
				FileChooserDialog dialog = new FileChooserDialog (
					"",null, FileChooserAction.Save);
					
				dialog.Title = "Save emoticon as...";
				dialog.AddButton (Stock.Cancel, 
					ResponseType.Cancel);
				dialog.AddButton (Stock.Save,
					ResponseType.Ok);
				dialog.DoOverwriteConfirmation = true;
				
				dialog.Run ();
				dialog.Destroy ();
			};
			
			men.Append (item);
			men.ShowAll ();
			return men;
		}
		
		private Gtk.ImageMenuItem createMenuItemFromStock (
			string stock_id,
			string label)
		{
			ImageMenuItem item = new ImageMenuItem ();
			Gtk.Image img = new Image (stock_id, IconSize.Menu);
			item.Image = img;
			Gtk.HBox hbox = new HBox (false, 0);
			
			hbox.PackStart (RickiLib.Widgets.Factory.Label (label),
				false, false, 0);
				
			
			item.Child = hbox;
			
			return item;
		}
		
		public Emoticon Emoticon {
			get { return emoticon; }
		}
		
		public Gtk.TextChildAnchor Anchor { 
			get { return anchor; }
			set { anchor = value; }
		}
		
		public Gtk.TextIter Iter {
			get { return iter; }
			set { iter = value; }
		}
		
		public Gtk.TextMark Mark {
			get { return mark; }
			set { mark = value; }
		}
		
		public Gtk.Widget Widget {
			get { return eventbox; }
			set { eventbox = (Gtk.EventBox) value; }
		}
		
		public RitchAnchorType Type {
			get { return RitchAnchorType.Emoticon; }
		}
	}
}
