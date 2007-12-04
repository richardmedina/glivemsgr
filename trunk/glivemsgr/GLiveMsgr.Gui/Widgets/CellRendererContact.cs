
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
							
				int index = 0;
				// x will be used for text or image orizontal position
				
				string text = Text;
				
				bool realized = false;
				
				int x = background_area.X;

				if (this.IsExpander) {
					x = 15;
				}
					showText (context, Text, ref x, background_area.Y, true);
					return;
				//}
				
				

				//while (index < text.Length) {
				//	Console.WriteLine ("While iteration index {0}", index);
					for (int i = 0; i < emoticons.Count; i ++) {
						Emoticon emoticon = emoticons [i];
						realized = false;
						int pos = text.IndexOf (
							emoticon.Trigger, 
							index);
						
						if (pos == -1) {
							continue;
						}
						
						if (pos > index) {
						//	Console.WriteLine ("pos is major than index");
							showText (context, 
							text.Substring (index, pos), 
							ref x, background_area.Y);
							// Pedding
							index += pos;
						}
						//else {
							showImage (context, 
							emoticon.Filename, 
							ref x, 
							background_area.Y);
							
							pos += emoticon.Trigger.Length;
						//}
						
						index = pos;
						//Console.WriteLine ("Current index is now {0} and length {1} pos {2} trigger {3}", 
						//	index, text.Length, pos, emoticon.Trigger.Length);
						//realized = true;
						i = -1;
						//break;
					}
					
					//if (!realized)
					//	if (index < text.Length) {
					//		showText (context, text.Substring (index, text.Length - index), ref x, background_area.Y);
					//	}
				//}
				
				//((IDisposable)context.Target).Dispose ();
				//((IDisposable)context).Dispose ();
			}
			//	m.ReleaseMutex ();
		}
		
		private void showImage (Cairo.Context cr, string filename, ref int x, int y)
		{
		//	Console.WriteLine ("Adding image {0} at {1}", filename, x);
			
			Cairo.ImageSurface image = new Cairo.ImageSurface (filename);
			image.Show (cr, x, y);
			
			//image.Destroy ();
			
			x += image.Width;
		}

		private void showText (Cairo.Context cr, string text, ref int x, int y, bool usemarkup) 
		{
			//Console.WriteLine ("Adding text : '{0} at {1}'", text, x);
			
			Pango.Layout layout = Pango.CairoHelper.CreateLayout (cr);
			
			if (usemarkup)
				layout.SetMarkup (text);
			else
				layout.SetText (text);
			
			cr.Color = new Cairo.Color (0, 0, 0);
			cr.MoveTo (x, y);
			Pango.CairoHelper.ShowLayout (cr, layout);
			
			int lw, lh;
			layout.GetPixelSize (out lw, out lh);
			
			x += lw;
		}
		
		private void showText (Cairo.Context cr, string text, ref int x, int y)
		{
			showText (cr, text, ref x, y);
		}
	}
}