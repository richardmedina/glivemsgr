
using System;
using System.Collections;
using Gtk;
//using Gdk;
//using Gtk.DotNet;
using System.Collections.Generic;

//using Cairo;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GLiveMsgr.Gui
{
	
	
	public class CellRendererContact : Gtk.CellRendererText
	{
//		private System.Drawing.Color color;
/*		
		private int leftPadding;
		private int rightPadding;
		private int topPadding;
		private int bottomPadding;
*/		
		private EmoticonManager emoticons;
		
		public CellRendererContact ()
		{
			emoticons = new EmoticonManager ();
			emoticons.LoadSession (string.Empty);
		}
		
		protected override void Render (Gdk.Drawable window, Widget widget, Gdk.Rectangle background_area,
			Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			//Gdk.Pixmap pixmap = new Gdk.Pixmap (window,
			//	widget.Allocation.Width, 
			//	widget.Allocation.Height,
			//	-1);
			
			Graphics g = Gtk.DotNet.Graphics.FromDrawable (window);//pixmap);			
	
			background_area.X = cell_area.X;
			
			if (this.IsExpander)
				background_area.X = 15;
			
				
			newDraw (g, background_area);
		}
		
		//FIXME: Must be performed by Cairo
		private void newDraw (Graphics g, Gdk.Rectangle background_area)
		{
			Brush fbrush = new SolidBrush (Color.Black);
			
			int fontSize= 8;
			FontStyle fs = FontStyle.Regular;
			
			if (this.IsExpander)
				fs = FontStyle.Bold;
			
			string tmpstring = (string) base.GetProperty ("text");

			if (tmpstring == "Drag and drop your contacts here") {
				fs |= FontStyle.Italic;
				fbrush = Brushes.Gray;
			//	fontSize = 6;
			}

			Font font = new Font ("monospace", fontSize, fs);				

			SizeF s = g.MeasureString (
				"test",
				font
			);

			// Position y-relative for text positioning			
			float yy = background_area.Y + 
				(background_area.Height / 2)
				- (s.Height / 2);
			
			if (this.IsExpander) {
				g.DrawString (tmpstring,
					font,
					Brushes.Black,
					background_area.X,
					yy);
				return;
			}
			
			EmoticonCollection matches = new EmoticonCollection ();
			
			foreach (Emoticon emoticon in emoticons) {
				int index = 0;
				int lastindex = index;
								
				while ((index = tmpstring.IndexOf (
					emoticon.Trigger, index)) != -1) {
				
					tmpstring.Substring (
						lastindex, 
						index - lastindex);
					
					if (index > -1) {
						matches.Add (index,
							emoticon);
					}
					
					index += emoticon.Trigger.Length;
					lastindex = index;
				}
			}
			
			float x = background_area.X;
			
			// Position y-relative for image positioning
			//float y = background_area.Y;
			
			int _index = 0;
			int _last_index = _index;
			
			
			// TODO: Image positioning could not be correct,
			//	because the image height is not same
						
			foreach (KeyValuePair <int, Emoticon> pair in matches) {
				_index = pair.Key;
				Emoticon _emoticon = pair.Value;
				
				if (_index > _last_index) {
					string sub = tmpstring.Substring (
						_last_index, _index - _last_index);

					SizeF size = g.MeasureString (
						sub,
						font
					);
					
					g.DrawString (sub,
						font,
						fbrush, 
						x, yy);
					
					
					x += size.Width;
				}
				
				System.Drawing.Image image = new Bitmap (_emoticon.Filename);
				
				int tmp_y = background_area.Y + 
					(background_area.Height / 2)
					- (image.Height / 2);
				
				GraphicsState state = g.Save ();
				
				g.DrawImage (image, x, tmp_y);
				
				g.Restore (state);
				
				x += image.Width;
				
				_index += _emoticon.Trigger.Length;
				
				_last_index = _index;				
			}
			
			if (_last_index < tmpstring.Length) {
				string sub = tmpstring.Substring (
					_last_index,
					tmpstring.Length - _last_index);
					
				g.DrawString (
					sub,
					font,
					fbrush,
					x, yy
				);
			}
		}
/*						
		public int Padding {
			set {
				HPadding = value;
				VPadding = value;
			}
		}
		
		public int HPadding {
			set {
				LeftPadding = RightPadding = value;
			}
		}
		
		public int VPadding {
			set {
				TopPadding = BottomPadding = value;
			}
		}
		
		public int LeftPadding {
			get {
				return leftPadding;
			}
			set {
				leftPadding = value;
			}
		}
		
		public int RightPadding {
			get {
				return rightPadding;
			}
			set {
				rightPadding = value;
			}
		}
		
		public int TopPadding {
			get {
				return topPadding;
			}
			set {
				topPadding = value;
			}
		}
		
		public int BottomPadding {
			get {
				return bottomPadding;
			}
			set {
				bottomPadding = value;
			}
		}
*/
	}
}
