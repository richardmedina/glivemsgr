
using System;
using System.Drawing;

namespace GLiveMsgr.Gui
{
	
	
	public class ToolbarButton : ToolbarItem
	{
		
		private Image image;
		private Brush brush;
		//private Image imageOut;
		
		public ToolbarButton (string filename)
		{
			image = new Bitmap (filename);
			brush = new SolidBrush (Color.LightBlue);
			//imageOver = new Bitmap (filenameOver);
		}
		public void Draw (Graphics graphics)
		{
			
		}

					
		public Image Image {
			get {
				return image;
			}
		}
		
		public override Size Size {
			get {
				return image.Size;
			}
		}
		
		public override Brush Brush {
			get {
				return brush;
			}
		}
	}
}
