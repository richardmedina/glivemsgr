
using System;
using System.IO;
using System.Collections;
using Gtk;
using System.Collections.Generic;

using RickiLib.Widgets;


namespace GLiveMsgr.Gui
{
	
	
	public class CellRendererContactIcon : Gtk.CellRendererPixbuf
	{
		
		private bool isMouseOver;
		
		public CellRendererContactIcon ()
		{
			isMouseOver = true;
			
		}
		
		protected override void Render (Gdk.Drawable window, Widget widget, Gdk.Rectangle background_area,
			Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			//Gdk.Pixmap pixmap = new Gdk.Pixmap (window,
			//	widget.Allocation.Width, 
			//	widget.Allocation.Height,
			//	-1);
			
			//Graphics g = Gtk.DotNet.Graphics.FromDrawable (window);//pixmap);			
			
			//byte [] bytes = base.Pixbuf.SaveToBuffer ("png");
			
			if (Pixbuf != null) {
				int px = (cell_area.X + cell_area.Width) - 
					(Pixbuf.Width / 2) - (cell_area.Width /2);
				int py = (cell_area.Y + cell_area.Height) -
					(Pixbuf.Height / 2) - (cell_area.Height / 2);
				
				//px = cell_area.X;
				//py = cell_area.Y;
				
				//Console.WriteLine ("Pixbuf at :{0},{1}", px, py);
				
				window.DrawPixbuf (widget.Style.WhiteGC,
					Pixbuf,
					0,
					0,
					px,
					py,
					Pixbuf.Width,
					Pixbuf.Height,
					Gdk.RgbDither.None,
					0, 0);
				
				if ((flags & CellRendererState.Prelit) > 0) {	
					Cairo.Context cr = Gdk.CairoHelper.Create (window);
					
					int x = cell_area.X;
					int y = cell_area.Y;
					int w = x + cell_area.Width;
					int h = y + cell_area.Height;
					
					
					cr.Color = new Cairo.Color (0, 0, 0, 0.5);
					cr.LineWidth = 1;
					cr.LineJoin = Cairo.LineJoin.Miter;
					cr.NewPath ();
					cr.MoveTo (x, y);
					cr.LineTo (w, y);
					cr.LineTo (w, h);
					cr.LineTo (x, h);
					cr.ClosePath ();
					
					cr.Stroke ();
				
					//img.Show (cr, 0, 0);
					((IDisposable)cr.Target).Dispose ();
					((IDisposable)cr).Dispose ();
				}
			}
/*
			if (base.Pixbuf != null) {
				byte [] bytes = base.Pixbuf.SaveToBuffer ("png");
			
				System.Drawing.Image image = null;
			
				using (MemoryStream ms = new MemoryStream (bytes))
					image = new System.Drawing.Bitmap (ms);
				

				
				int y = background_area.Y + 
					(background_area.Height / 2) - (image.Height / 2);
				int x = cell_area.X +
					(cell_area.Width / 2) - (image.Width /2);
				
				
				//if (flags == CellRendererState.
				
				
				g.DrawImage (image,
					x, 
					y);

				if ((flags & CellRendererState.Prelit) > 0) {				
				System.Drawing.Pen pen = 
					new System.Drawing.Pen (Color.LightGray);//System.Drawing.Pens.DarkGray;
				
				
				g.DrawLine (
					pen,
					cell_area.X, cell_area.Y + cell_area.Height,
					cell_area.X, cell_area.Y
				);

				
				g.DrawLine (
					pen,
					cell_area.X, cell_area.Y,
					cell_area.X + cell_area.Width, cell_area.Y
				);
				
				pen.Color = System.Drawing.Color.DarkGray;
				pen.Width = 1.5f;
				
				g.DrawLine (
					pen,
					cell_area.X + cell_area.Width, cell_area.Y,
					cell_area.X + cell_area.Width, cell_area.Y + cell_area.Height
				);
				
				g.DrawLine (
					pen,
					cell_area.X + cell_area.Width, cell_area.Y + cell_area.Height,
					cell_area.X, cell_area.Y + cell_area.Height
				);
				}			
				//g.DrawRectangle (System.Drawing.Pens.DarkGray, 
				//	cell_area.X, cell_area.Y, 
				//	cell_area.Width, cell_area.Height);
				
				
			}
			
*/			
			//g.DrawImage (image, 50, 2);
			//g.DrawRectangle (System.Drawing.Pens.Green, 0, 0, 10, 10);
			
		}
		
		[GLib.Property ("ismouseover")]
		public bool IsMouseOver {
			get { return isMouseOver; }
			set { isMouseOver = value; }
		}
/*				
		private void newDraw (Graphics g, Gdk.Rectangle background_area)
		{
		}
*/
	}
}
