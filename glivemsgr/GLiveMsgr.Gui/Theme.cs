
using System;
using System.IO;
using Gtk;
using Cairo;

namespace GLiveMsgr.Gui
{
	
	
	public static class Theme
	{
		// Set default theme
		private static event EventHandler _modified;
		
		static Theme ()
		{
			_modified = onModified;
			FgColor = RgbToCairoColor (0, 0, 0);
			
			BgColor = RgbToCairoColor (0xFC, 0x9A, 0x3F);			
			BaseColor = RgbToCairoColor (255, 0, 0); //(0xFF, 0xE6, 0xD5);
			
			TextColor = RgbToCairoColor (0x1A, 0x1A, 0x1A);
			
			SelectedFgColor = RgbToCairoColor (0xFF, 0xFF, 0xFF);
			SelectedBgColor = RgbToCairoColor (0xA8, 0x76, 0x1C);
			TooltipFgColor = RgbToCairoColor (0, 0, 0);
			TooltipBgColor = RgbToCairoColor (0xF5, 0xF5, 0xF5);
			
			loadThemeFromStyle ();
		}
		
		private static void loadYellowTheme ()
		{
			BgColor = RgbToCairoColor (0xFC, 0x9A, 0x3F);			
			BaseColor = RgbToCairoColor (0xFF, 0xE6, 0xD5);
			
			SelectedBgColor = BgColor;
			
			TextColor = RgbToCairoColor (0x1A, 0x00, 0x00);
		}
		
		private static void loadBlueTheme ()
		{
			BgColor = RgbToCairoColor (0, 0, 0xFF);
		}

		public static Gdk.Color GdkColorFromCairo (Cairo.Color color)
		{
			float cairo_unit = byte.MaxValue;
			
			Gdk.Color gdk_color = new Gdk.Color (
				(byte) (color.R * cairo_unit),
				(byte) (color.G * cairo_unit),
				(byte) (color.B * cairo_unit));// * cairo_unit));
			
			return gdk_color;
		}
		
		public static Cairo.Color CairoColorFromGdk (Gdk.Color color)
		{
			double gdk_unit = byte.MaxValue;
			
			byte r = UshortToByte (color.Red);
			byte g = UshortToByte (color.Green);
			byte b = UshortToByte (color.Blue);
			
			Cairo.Color c = new Cairo.Color (
				(r / gdk_unit), 
				(g / gdk_unit), 
				(b / gdk_unit));
			
			Console.WriteLine ("R{0},G{1},B{2}", c.R, c.G, c.B);
			return c;
		}

		public static byte UshortToByte (ushort val)
		{
			ushort b = (ushort) (val << 8);
			b = (ushort) (b >> 8);
			return (byte) b;
		}
		
		public static Cairo.Color RgbToCairoColor (int red, int green, int blue)
		{
			float unit = 1f / ushort.MaxValue;
				
			return new Cairo.Color (unit * red, unit * green, unit * blue);
		}
		
		public static void SendModified ()
		{
			_modified (null, EventArgs.Empty);
		}
		
		private static void loadThemeFromStyle ()
		{
			Gtk.Style gtkstyle = new Gtk.Style ();
			
			Gdk.Color c = gtkstyle.Base (StateType.Normal);
			
			Console.WriteLine ("Console. R {0}, G {1} B {2}",
				c.Red, c.Green, c.Blue);
			
			                   
			Theme.FgColor = Theme.CairoColorFromGdk (
				gtkstyle.Foreground (StateType.Normal));
			
			Theme.BgColor = Theme.CairoColorFromGdk ( 
				gtkstyle.Background (StateType.Selected));
				
			Theme.BaseColor = Theme.CairoColorFromGdk (
				gtkstyle.Base (StateType.Normal));
			
			Theme.SelectedBgColor = Theme.CairoColorFromGdk (
				gtkstyle.Base (StateType.Normal));
			
			Gdk.Color color = GdkColorFromCairo (Theme.BaseColor);
				//Console.WriteLine ("Selected : {0}, {1:X},{2:X}",
				//	(byte) color.Red, (byte) color.Green, (byte) color.Blue);
		}
		
		private static void onModified (object sender, EventArgs args)
		{
		}
		
		private static Cairo.Color fgColor;
		private static Cairo.Color bgColor;
		private static Cairo.Color textColor;
		private static Cairo.Color baseColor;
		private static Cairo.Color selectedFgColor;
		private static Cairo.Color selectedBgColor;
		private static Cairo.Color tooltipFgColor;
		private static Cairo.Color tooltipBgColor;
		
		
		public static  Cairo.Color FgColor {
			get { return fgColor; }
			set { fgColor = value; }
		}
		
		public static Cairo.Color BgColor {
			get { return bgColor; }
			set { bgColor = value; }
		}
		
		public static Cairo.Color TextColor {
			get { return textColor; }
			set { textColor = value; }
		}
		
		public static Cairo.Color BaseColor {
			get { return baseColor; }
			set { baseColor = value; }
		}
		
		public static Cairo.Color SelectedFgColor {
			get { return selectedFgColor; }
			set { selectedFgColor = value; }
		}
		
		public static Cairo.Color SelectedBgColor {
			get { return selectedBgColor; }
			set { selectedBgColor = value; }
		}
		
		public static Cairo.Color TooltipFgColor {
			get { return tooltipFgColor; }
			set { tooltipFgColor = value; }
		}
		
		public static Cairo.Color TooltipBgColor {
			get { return tooltipBgColor; }
			set { tooltipBgColor = value; }
		}
		
		public static event EventHandler Modified {
			add { _modified += value; }
			remove { _modified -= value; }
		}
	}
}
