
using System;
using Gtk;
using System.Net.Protocols;

namespace GLiveMsgr.Gui
{
	
	public class EmoticonBox : Gtk.Window
	{
		private int cols = 5;
		private Gtk.Viewport viewport;
		
		public event EmoticonSelectedHandler Selected; 
		
		public EmoticonBox () : base (WindowType.Toplevel)
		{
			this.viewport = new Viewport ();
			//ConversationBackground
			this.viewport.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BgColor));
			
			this.WindowPosition = WindowPosition.CenterOnParent;
			
			this.Selected = onSelected;
			
			Gtk.VBox vbox = new VBox (false, 0);
			EmoticonManager man = new EmoticonManager ();
			man.CloseSession ();
			
			Debug.WriteLine (man.Count);
			HBox hbox = new HBox (false, 0);
			vbox.PackStart (hbox, false, false, 0);
			int i = 4;
			
			foreach (Emoticon e in man) {
				if ( ++i == cols) {
					hbox = new HBox (false, 0);
					vbox.PackStart (hbox, false, false, 0);
					i = 0;
				}
				EmoticonButton button = new EmoticonButton (e);
				button.Clicked += button_Clicked;
				button.ButtonReleaseEvent += button_ButtonReleaseEvent;
				hbox.PackStart (button,
					false, false, 0);
			}
			
			viewport.Add (vbox);
			
			base.Add (viewport);
			
			base.ModifyBg (StateType.Normal,
				new Gdk.Color (0xFF, 0xFF, 0xFF));
			
			this.Events = Gdk.EventMask.AllEventsMask;
		}
		
		protected override void OnRealized ()
		{
			base.OnRealized ();
			
			this.GdkWindow.TypeHint = Gdk.WindowTypeHint.Toolbar;
			
			this.GdkWindow.SetDecorations (
				Gdk.WMDecoration.All &
				(Gdk.WMDecoration.Resizeh |
				Gdk.WMDecoration.Border)
			);
		}
		
		protected override bool OnKeyPressEvent (Gdk.EventKey args)
		{
			if (args.Key != Gdk.Key.Left &&
				args.Key != Gdk.Key.Right &&
				args.Key != Gdk.Key.Up &&
				args.Key != Gdk.Key.Down)
				base.Hide ();
			
			return base.OnKeyPressEvent (args);
		}
		
		protected override bool OnFocusOutEvent (Gdk.EventFocus args)
		{
			this.Hide ();
			return base.OnFocusOutEvent (args);
		}
		private void button_Clicked (object sender, EventArgs args)
		{
			EmoticonButton button = (EmoticonButton) sender;
			this.Selected (this, 
				new EmoticonSelectedArgs (button.Emoticon));
			this.Hide ();
		}
		
		[GLib.ConnectBefore]
		private void button_ButtonReleaseEvent (object sender,
			ButtonReleaseEventArgs args)
		{
			EmoticonButton button = (EmoticonButton) sender;
			
			if (args.Event.Button == 3) {
				this.Selected (this, 
					new EmoticonSelectedArgs (button.Emoticon));
			}
		}
		
		private void onSelected (object sender, EmoticonSelectedArgs args)
		{
		}				
	}
}
