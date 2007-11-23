
using System;
using Gtk;
using Gdk;


namespace GLiveMsgr.Gui
{
	
	
	public class WMTitleBar : Gtk.EventBox
	{
		private WMButtons buttons;
		private HBox hbox;
		
		private Gtk.Window window;
		
		private bool isDragging;
		private double relative_x;
		private double relative_y;
		
		private Gdk.Cursor cursorDef;
		private Gdk.Cursor cursorHand;
		
//		private static int i;
		
		public WMTitleBar (Gtk.Window window)
		{
			this.window = window;
			this.isDragging = false;
			this.relative_x = 0;
			this.relative_y = 0;
			this.cursorDef = new Gdk.Cursor (Gdk.CursorType.Fleur);
			this.cursorHand = new Gdk.Cursor (Gdk.CursorType.LeftPtr);
		
			base.ModifyBg (StateType.Normal,
				Theme.GetGdkColor (Theme.ToolbarGradientStartColor));
			
			buttons = new WMButtons (window);
			
			hbox = new HBox (false, 0);
			hbox.PackEnd (buttons, false, false, 0);
			
			base.Add (hbox);
			
		}
		
		//protected override 
		
		protected override bool OnMotionNotifyEvent (
			Gdk.EventMotion args)
		{	
			if (this.isDragging) {
				double xdiff = this.relative_x - args.X;
				double ydiff = this.relative_y - args.Y;
				int x, y;
				
				window.GdkWindow.GetPosition (out x, out y);
				
				window.Move (
					(int) (x - xdiff),
					(int) (y - ydiff));
			}
			
			return base.OnMotionNotifyEvent (args);
		}
		
		protected override bool OnButtonPressEvent (Gdk.EventButton args)
		{
			this.IsDragging = true;
			this.relative_x = args.X;
			this.relative_y = args.Y;
			
			if (args.Type == Gdk.EventType.TwoButtonPress) 
				if (args.Button == 1) {
					buttons.MaxButton.Click ();
					this.isDragging = false;
				}
			
			return base.OnButtonPressEvent (args);
		}
		
		protected override bool OnButtonReleaseEvent (Gdk.EventButton args)
		{
			this.IsDragging = false;
			return base.OnButtonReleaseEvent (args);
		}
		
		
		public WMButtons Buttons {
			get { return buttons; }
		}
		
		protected bool IsDragging {
			get { return this.isDragging; }
			set {
				this.isDragging = value;
				if (value)
					this.GdkWindow.Cursor = cursorDef;
				else 
					this.GdkWindow.Cursor = cursorHand;
			}
		}
	}
}
