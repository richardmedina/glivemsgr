
using System;
using Cairo;
namespace GLiveMsgr.Gui
{
	
	
	public class ToolbarButton : ToolbarItem
	{
		
		private Cairo.ImageSurface _image;
		private Cairo.Color _color;
		
		//private Image imageOut;
		
		public ToolbarButton (string filename)
		{
			_image = new ImageSurface (filename);
			_color = new Cairo.Color (0f, 0f, 0.5);
		}
		public override void Draw (Cairo.Context context)
		{
			Image.Show (context, X, Y);
			
			if (HasMouseOver) {
				context.Color = Theme.BgColor;
				context.LineWidth = 1;
				context.Rectangle (X, Y, Width, Height);
				context.Stroke ();
			}
		}
		
		public override float Width {
			get { return _image.Width; }
		}
		
		public override float Height {
			get { return _image.Height; }
		}
					
		public ImageSurface Image {
			get { return _image; }
		}
				
		public override Cairo.Color Color{
			get { return _color; }
		}
	}
}
