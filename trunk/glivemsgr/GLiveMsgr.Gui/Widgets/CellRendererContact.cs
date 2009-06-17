
using System;
using System.Collections;
using Gtk;
using System.Collections.Generic;
using System.Threading;

namespace GLiveMsgr.Gui
{
	
	
	public class CellRendererContact : Gtk.CellRendererText
	{
		private static EmoticonManager emoticons = new EmoticonManager ();
		
		private Gdk.Pixmap pixmap = null;
		
		static CellRendererContact ()
		{
			emoticons.LoadSession (string.Empty);
		}
		
		public CellRendererContact ()
		{
			//emoticons = new EmoticonManager ();
			//emoticons.LoadSession (string.Empty);
		}
		
		protected override void Render (Gdk.Drawable window, Widget widget, Gdk.Rectangle background_area,
			Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			
				//Mutex m = new Mutex ();
				//m.WaitOne ();
			using (Cairo.Context context = Gdk.CairoHelper.Create (window)) {				
				int x = background_area.X;
				
				bool isgroup = false;
				if (IsExpander) {
					x = 15;
					isgroup = true;
				}
				
				showText (context, Text, ref x, background_area.Y, true, isgroup);
				((IDisposable)context.Target).Dispose ();
			}
		}
		
		private void showImage (Cairo.Context cr, string filename, ref int x, int y)
		{
		//	Console.WriteLine ("Adding image {0} at {1}", filename, x);
			
			Cairo.ImageSurface image = new Cairo.ImageSurface (filename);
			image.Show (cr, x, y);
			
			//image.Destroy ();
			
			x += image.Width;
			((IDisposable) image).Dispose ();
		}

		private void showText (Cairo.Context cr, string text, ref int x, int y, bool usemarkup, bool isgroup) 
		{
			//Console.WriteLine ("Adding text : '{0} at {1}'", text, x);
			
			Pango.Layout layout = Pango.CairoHelper.CreateLayout (cr);
			/*
			if (usemarkup) {
				if (!isgroup) {
					text = text.Replace ("<", "&lt;");
					text = text.Replace (">", "&gt;");
					text = text.Replace ("&", "&amp;");
				}
				
				layout.SetMarkup (
					string.Format ("<small>{0}</small>",text));
			}
			else
				layout.SetText (text);
			*/
			
			Gtk.Style s = new Style ();
			
			layout.FontDescription = s.FontDesc;
			if (isgroup)
				layout.FontDescription.Weight = Pango.Weight.Bold;
			
			layout.SetText (text); 
			
			cr.Color = new Cairo.Color (0, 0, 0);
			cr.MoveTo (x, y);
			
			Pango.CairoHelper.ShowLayout (cr, layout);
			
			int lw, lh;
			layout.GetPixelSize (out lw, out lh);
			
			x += lw;
			layout.Dispose ();
		}
		
		private void showText (Cairo.Context cr, string text, ref int x, int y)
		{
			showText (cr, text, ref x, y);
		}
	}
}