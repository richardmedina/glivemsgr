
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class DisplayPictureWidget : Gtk.EventBox
	{
		private Gtk.Image image;
		private bool _editable;
		
		private Gdk.Cursor _cursor;
		
		public DisplayPictureWidget () : this (null)
		{
		}
		
		public DisplayPictureWidget (Gdk.Pixbuf pixbuf)
		{
			//base.ShadowType = ShadowType.EtchedOut;
			_editable = false;
			_cursor = new Gdk.Cursor (Gdk.CursorType.Hand2);;
			
			if (pixbuf == null)
				pixbuf = Gdk.Pixbuf.LoadFromResource (
					"user_display_picture_default.png"
				);
			
			image = new Gtk.Image (pixbuf);
			//ConversationBackground
			//ModifyBg (StateType.Normal,
			//	Theme.GdkColorFromCairo (Theme.BgColor));
			
			Add (image);
		}
		
		protected override void OnRealized ()
		{
			base.OnRealized ();
			
			GdkWindow.Cursor = null;
			
			if (Editable)
				GdkWindow.Cursor = _cursor;
		}
		
		protected override bool OnButtonPressEvent (Gdk.EventButton evnt)
		{
			//DisplayPictureDialog dlg = new DisplayPictureDialog ();
			//if (dlg.Run () == ResponseType.Ok) {
				
			//}
			if (Editable) {
				Gtk.FileChooserDialog dlg = new FileChooserDialog (
					"Select your display picture", 
					null, 
				FileChooserAction.Open);
			
				dlg.AddButton (Stock.Cancel, ResponseType.Cancel);
				dlg.AddButton (Stock.Open, ResponseType.Ok);
			
				ResponseType r = (ResponseType) dlg.Run ();
				string filename = dlg.Filename;
				dlg.Destroy ();
				if (r == ResponseType.Ok) {
					Gdk.Pixbuf pixbuf = new Gdk.Pixbuf (
						filename);
						
					pixbuf = pixbuf.ScaleSimple (
						Allocation.Width,
						Allocation.Height, 
						Gdk.InterpType.Hyper);
						
					Image.Pixbuf = pixbuf;
				}
			}
			
			return base.OnButtonPressEvent (evnt);
		}
		
		public Gtk.Image Image {
			get { return image; }
		}
		
		public bool Editable {
			get { return _editable; }
			set { 
				_editable = value;
				if (IsRealized) {
					GdkWindow.Cursor = null;
					if (_editable)
						GdkWindow.Cursor = _cursor;
				
				}
			}
		}
	}
}
