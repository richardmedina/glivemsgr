
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class LogonHeaderWidget : Gtk.VBox
	{
		
		//private Gdk.Pixmap pixmap;
		
		public LogonHeaderWidget ()
		{
			//AddEvents ((int) Gdk.EventMask.ButtonPressMask);			
			HeightRequest = 50;
		}
		/*
		protected override bool OnButtonPressEvent (Gdk.EventButton evnt)
		{
			GdkWindow.BeginMoveDrag (1, 
				(int) evnt.XRoot, 
				(int) evnt.YRoot, 
				evnt.Time);
			return base.OnButtonPressEvent (evnt);
		}*/

/*
		protected override bool OnConfigureEvent (Gdk.EventConfigure args)
		{
			
			pixmap = new Gdk.Pixmap (args.Window, Allocation.Width, Allocation.Height, -1);
			
			Cairo.Context context = Gdk.CairoHelper.Create (pixmap);
			
			context.Rectangle (0, 0, this.Allocation.Width, this.Allocation.Height);
			
			Cairo.Gradient grad = new Cairo.LinearGradient (
				0, 0,
				0, this.Allocation.Height -5);
			
			grad.AddColorStop (0, 
				Theme.BgColor);
			
			grad.AddColorStop (1,
				Theme.BaseColor);
			
			context.Pattern = grad;
			
			context.Fill ();
			
			context.Color = Theme.TextColor; 
			
			Pango.Layout l = Pango.CairoHelper.CreateLayout (context);
			l.SetMarkup ("<b>GNOME Live Messenger</b>\nby Ricki Medina");
			l.FontDescription = new Pango.FontDescription ();
			l.FontDescription.Weight = Pango.Weight.Bold;
			l.FontDescription.Size = Pango.Units.FromPixels (5);
			l.Alignment = Pango.Alignment.Right;
			
			int width,height;
			l.GetPixelSize (out width, out height);
			double x = this.Allocation.Width - width - 5;
			
			context.MoveTo (x, 5);
			Pango.CairoHelper.ShowLayout (context, l);
			
			((IDisposable) context.Target).Dispose ();
			((IDisposable) context).Dispose ();
			
			return base.OnConfigureEvent (args);
			
			
		}
*/		/*
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			bool ret = base.OnExposeEvent (evnt);
			
			evnt.Window.DrawDrawable (
				this.Style.WhiteGC, pixmap,
				evnt.Area.X, evnt.Area.Y,
				evnt.Area.X, evnt.Area.Y,
				evnt.Area.Width, evnt.Area.Height);
			
			return ret;
		}*/
	}
}
