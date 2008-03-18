
using System;
using System.IO;
using Gtk;
using Cairo;

namespace GLiveMsgr.Gui
{
	
	
	public static class Theme
	{
		// Set default theme
		static Theme ()
		{
		
			FgColor = RgbToCairoColor (0, 0, 0);
			
			BgColor = RgbToCairoColor (0xFC, 0x9A, 0x3F);			
			BaseColor = RgbToCairoColor (0xFF, 0xE6, 0xD5);
			
			TextColor = RgbToCairoColor (0x1A, 0x1A, 0x1A);
			
			SelectedFgColor = RgbToCairoColor (0xFF, 0xFF, 0xFF);
			SelectedBgColor = RgbToCairoColor (0xA8, 0x76, 0x1C);
			TooltipFgColor = RgbToCairoColor (0, 0, 0);
			TooltipBgColor = RgbToCairoColor (0xF5, 0xF5, 0xF5);
			
			loadYellowTheme ();
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
//			double unit = 1/255;
			
			Gdk.Color gdk_color = new Gdk.Color (
				(byte) (color.R * 255),
				(byte) (color.G * 255),
				(byte) (color.B * 255));
			
			//Console.WriteLine (gdk_color);
			Console.WriteLine ("From cairo : {0:X},{1:X},{2:X}", 
				(int) (color.R * 255), (int) (color.G * 255), (int) (color.B * 255));
			
			return gdk_color;
		}
		
		public static Cairo.Color CairoColorFromGdk (Gdk.Color color)
		{
			return RgbToCairoColor ((byte) color.Red, 
				(byte) color.Green, 
				(byte) color.Blue);
		}
		
		public static Cairo.Color RgbToCairoColor (int red, int green, int blue)
		{
			double unit = 1f / 255f;
				
			return new Cairo.Color (unit * red, unit * green, unit * blue);
		}
		
		private static void loadThemeFromStyle ()
		{
			Gtk.Style gtkstyle = new Gtk.Style ();
			Theme.FgColor = Theme.CairoColorFromGdk (
				gtkstyle.Foreground (StateType.Normal));
			
			Theme.BgColor = Theme.CairoColorFromGdk ( 
				gtkstyle.Background (StateType.Normal));
				
			Theme.BaseColor = Theme.CairoColorFromGdk (
				gtkstyle.Base (StateType.Normal));
			
			Theme.SelectedBgColor = Theme.CairoColorFromGdk (
				gtkstyle.Base (StateType.Normal));
			
			//Gdk.Color color = Theme.GdkColorFromCairo (Theme.SelectedBgColor);
			
			//foreach (Gdk.Color color in gtkstyle.BaseColors)
			Gdk.Color color = GdkColorFromCairo (Theme.BaseColor);
				Console.WriteLine ("Selected : {0}, {1:X},{2:X}",
					(byte) color.Red, (byte) color.Green, (byte) color.Blue);
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
	}
}
