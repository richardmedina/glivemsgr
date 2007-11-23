
using System;
using System.IO;
using System.Collections;
using Gtk;
//using Gdk;
//using Gtk.DotNet;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

using RickiLib.Widgets;


namespace GLiveMsgr.Gui
{
	
	
	public class CellRendererContactIcon : Gtk.CellRendererPixbuf
	{
		
		private bool isMouseOver;
		
		public CellRendererContactIcon ()
		{
			isMouseOver = false;
			
		}
		
		protected override void Render (Gdk.Drawable window, Widget widget, Gdk.Rectangle background_area,
			Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			//Gdk.Pixmap pixmap = new Gdk.Pixmap (window,
			//	widget.Allocation.Width, 
			//	widget.Allocation.Height,
			//	-1);
			
			Graphics g = Gtk.DotNet.Graphics.FromDrawable (window);//pixmap);			
			
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

				if (this.isMouseOver) {				
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
