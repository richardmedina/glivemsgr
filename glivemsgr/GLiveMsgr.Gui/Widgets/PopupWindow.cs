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
		
		public PopupWindow () : base (string.Empty)
		{
			BorderWidth = 10;
			Resize (200, 150);
			ModifyBg (StateType.Normal,
				new Gdk.Color (255, 255, 255));
		}
		
		protected override void OnRealized ()
		{
			base.OnRealized ();
		}


		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			
			bool ret = base.OnExposeEvent (evnt);
			
			Cairo.Context context = Gdk.CairoHelper.Create (evnt.Window);
			
			context.Rectangle (1, 1, this.Allocation.Width -2, 55);
			
			Cairo.Gradient grad = new Cairo.LinearGradient (
				0, 0,
				0, 55);
			
			grad.AddColorStop (0, 
				Theme.BgColor);
			
			grad.AddColorStop (1,
				Theme.BaseColor);
			
			context.Pattern = grad;
			
			context.Fill ();
						
			Cairo.ImageSurface imageSurf = 
				new Cairo.ImageSurface ("gnome-logo.png");
			
			imageSurf.Show (context,
				Allocation.Width - imageSurf.Width - 5,
				Allocation.Height - imageSurf.Height - 5);

			double lw = 2;
			
			context.NewPath ();
			context.LineWidth = lw;
			context.LineCap = Cairo.LineCap.Round;
			context.LineJoin = Cairo.LineJoin.Bevel;
			context.Color = Theme.BgColor;//new Cairo.Color (0.7f, 0.7f, 0.7f);
			
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

			
			((IDisposable) context.Target).Dispose ();
			((IDisposable) context).Dispose ();
			
			//Child.SendExpose (evnt);
			return ret;
		}

		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
			
			Gdk.Pixmap pixmap = new Gdk.Pixmap (GdkWindow, allocation.Width, allocation.Height, 1);
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
				//pixmap.DrawRectangle (gc, true, 0, 20, w, 100);
				
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
	}
}
