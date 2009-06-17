// ContactPopup.cs created with MonoDevelop
// User: ricki at 21:46 01/20/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class PopupWindow : Gtk.Window
	{
		private bool _logoVisible;
		
		private Gdk.Cursor cursor_right = new Gdk.Cursor (
			Gdk.CursorType.RightSide);
		private Gdk.Cursor cursor_bottom = new Gdk.Cursor (
			Gdk.CursorType.BottomSide);
		private Gdk.Cursor cursor_bottomright = new Gdk.Cursor (
			Gdk.CursorType.BottomRightCorner);
			
		private int _logo_posx = 0;
		private int _logo_posy = 0;
		private int _logo_width = 0;
		private int _logo_height = 0;

		
		public PopupWindow () : this (string.Empty, WindowType.Toplevel)
		{
		}
		
		public PopupWindow (string title) : this (title, WindowType.Toplevel)
		{
		}
		
		public PopupWindow (WindowType type) : this (string.Empty, type)
		{
		}
		
		public PopupWindow (string title, WindowType type) : base (type)
		{
			Title = title;
			BorderWidth = 5;
			AddEvents ((int) (Gdk.EventMask.ButtonPressMask | Gdk.EventMask.PointerMotionMask));
			Resize (200, 150);
			ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BaseColor));
		}
		
		protected override bool OnMotionNotifyEvent (Gdk.EventMotion evnt)
		{			
			GdkWindow.Cursor = null;
			
			if (evnt.X > Allocation.Width - 15 && 
				evnt.Y > Allocation.Height - 15) {
				GdkWindow.Cursor = cursor_bottomright;
			} else if (evnt.X > Allocation.Width -15) {
				GdkWindow.Cursor = cursor_right;
			} else if (evnt.Y > Allocation.Height -15) {
				GdkWindow.Cursor = cursor_bottom;
			}
			
			//Child.ProcessEvent (evnt);
			
			return base.OnMotionNotifyEvent (evnt);
		}


		protected override bool OnButtonPressEvent (Gdk.EventButton evnt)
		{
			if (evnt.Type == Gdk.EventType.TwoButtonPress &&
				evnt.Button ==1) {
				if ((GdkWindow.State & Gdk.WindowState.Maximized) > 0 ||
					(GdkWindow.State & Gdk.WindowState.Fullscreen) > 0) {
					GdkWindow.Unfullscreen ();
					GdkWindow.Unmaximize ();
					
				} else {
					GdkWindow.Maximize ();
					if ((evnt.State & Gdk.ModifierType.ControlMask) > 0)
						GdkWindow.Fullscreen ();
				}
			}
			else if (evnt.X > Allocation.Width - 15 &&
				evnt.Y > Allocation.Height - 15)
				GdkWindow.BeginResizeDrag (Gdk.WindowEdge.SouthEast,
					1, (int) evnt.XRoot, (int)  evnt.YRoot, evnt.Time);
			else if (evnt.X > Allocation.Width - 15)
				GdkWindow.BeginResizeDrag (Gdk.WindowEdge.East,
					1, (int) evnt.XRoot, (int) evnt.YRoot, evnt.Time);
			else if (evnt.Y > Allocation.Height - 15)
				GdkWindow.BeginResizeDrag (Gdk.WindowEdge.South,
					1, (int) evnt.XRoot, (int) evnt.YRoot, evnt.Time);
			else
				GdkWindow.BeginMoveDrag (1, (int) evnt.XRoot, (int) evnt.YRoot, evnt.Time);
			
			return base.OnButtonPressEvent (evnt);
		}

		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			
			bool ret = base.OnExposeEvent (evnt);
			
			using (Cairo.Context context = 
				Gdk.CairoHelper.Create (evnt.Window)) {
				
				context.Rectangle (1, 1, 
					Allocation.Width -2, Allocation.Height);
			
				Cairo.Gradient grad = new Cairo.LinearGradient (
					0, 0,
					0, Allocation.Height);
			
				grad.AddColorStop (0, 
					Theme.BgColor);
			
				grad.AddColorStop (1,
					Theme.BaseColor);
				
				Gdk.Color mmc = Theme.GdkColorFromCairo (Theme.BgColor);
				Gdk.Color mmc2 = Theme.GdkColorFromCairo (Theme.BaseColor);
				Console.WriteLine ("Drawing. Cairo {0},{1},{2} Gdk {3},{4},{5}",
				                   Theme.BgColor.R, Theme.BgColor.G, Theme.BgColor.B,
				                   mmc.Red, mmc.Green, mmc.Blue);
				
				Console.WriteLine ("Drawing2. Cairo {0},{1},{2} Gdk {3},{4},{5}",
				                   Theme.BaseColor.R, Theme.BaseColor.G, Theme.BaseColor.B,
				                   mmc2.Red, mmc2.Green, mmc2.Blue);
			
				context.Pattern = grad;
			
				context.Fill ();
				
				if (LogoVisible) {
					// FIXME. this resource must be released..
					Cairo.ImageSurface imageSurf = 
						new Cairo.ImageSurface ("gnome-logo.png");
			
					_logo_posx = Allocation.Width - imageSurf.Width - 5;
					_logo_posy = Allocation.Height - imageSurf.Height - 5;
					_logo_width = imageSurf.Width;
					_logo_height = imageSurf.Height;
			
					imageSurf.Show (context,
						_logo_posx,
						_logo_posy);
					((IDisposable) imageSurf).Dispose ();
				}

				double lw = 2;
			
				context.NewPath ();
				context.LineWidth = lw;
				context.LineCap = Cairo.LineCap.Round;
				context.LineJoin = Cairo.LineJoin.Bevel;
				context.Color = Theme.BgColor;
			
				double mx = 0 + (lw/2);
				double my = 0 + (lw/2);
				double mw = Allocation.Width - lw;
				double mh = Allocation.Height - lw;
			
				context.MoveTo (mx, my);
				context.LineTo (mw, mx);
				context.LineTo (mw, mh);
				context.LineTo (mx, mh);
				context.ClosePath ();
				context.Stroke ();
			}
			
			if (Child != null)
				Child.SendExpose (evnt);
			
			return ret;
		}

		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
			
			Gdk.Pixmap pixmap = new Gdk.Pixmap (GdkWindow, 
				allocation.Width, 
				allocation.Height, 1);
			using(Gdk.GC gc = new Gdk.GC(pixmap))
			{
				Gdk.Colormap colormap = Gdk.Colormap.System;
				Gdk.Color white = new Gdk.Color(255, 255, 255);
				colormap.AllocColor(ref white, true, true);

				Gdk.Color black = new Gdk.Color(0, 0, 0);
				colormap.AllocColor(ref black, true, true);

				gc.Foreground = white;
				
				pixmap.DrawRectangle(gc, true, 
					0, 0, 
					allocation.Width, allocation.Height);
				
				gc.Foreground = black;
				
				for (int i = 0; i < 3; i ++) {
					pixmap.DrawPoint (gc, i, 0);
					pixmap.DrawPoint (gc, 0, i);
				}
				
				for (int i = 0; i < 3; i ++) {
					pixmap.DrawPoint (gc,
						Allocation.Width -1,
						i);
					pixmap.DrawPoint (gc, 
						Allocation.Width -1 - i, 
						0);
				}
				
				for (int i = 0; i < 3; i ++) {
					pixmap.DrawPoint (gc,
						Allocation.Width -1 -i,
						Allocation.Height - 1);
					pixmap.DrawPoint (gc,
						Allocation.Width -1,
						Allocation.Height - 1 - i);
				}
				
				for (int i = 0; i < 3; i ++) {
					pixmap.DrawPoint (gc,
						i,
						Allocation.Height - 1);
					pixmap.DrawPoint (gc,
						0,
						Allocation.Height -1 -i);
				}
			}
			
			ShapeCombineMask (pixmap, 0, 0);
			pixmap.Dispose ();
		}
		
		public bool LogoVisible {
			get { return _logoVisible; }
			set { 
				_logoVisible = value;
				QueueDraw ();
			}
		}
		
		public int LogoPosX {
			get { return _logo_posx; }
		}
		
		public int LogoPosY {
			get { return _logo_posy; }
		}
		
		public int LogoWidth {
			get { return _logo_width; }
		}
		
		public int LogoHeight {
			get { return _logo_height; }
		}
	}
}
