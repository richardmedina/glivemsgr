
using System;
using System.IO;
using System.Drawing;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public static class Theme
	{
		private static Color logon_background;
		private static Color logon_entry_background;
		private static Color logon_entry_foreground;
		
		private static Color conv_header_gradient_start_color;
		private static Color conv_header_gradient_end_color;
		
		private static Color toolbar_gradient_start_color;
		private static Color toolbar_gradient_end_color;
		
		private static Color conv_background;
		
		//private Cairo.Color
		// Set default theme
		static Theme ()
		{
		
			FgColor = RgbToCairoColor (0, 0, 0);
			BgColor = RgbToCairoColor (0xED, 0xEC, 0xEB);
			TextColor = RgbToCairoColor (0x1A, 0x1A, 0x1A);
			BaseColor = RgbToCairoColor (0xFF, 0xFF, 0xFF);
			SelectedFgColor = RgbToCairoColor (0xFF, 0xFF, 0xFF);
			SelectedBgColor = RgbToCairoColor (0xA8, 0x76, 0x1C);
			TooltipFgColor = RgbToCairoColor (0, 0, 0);
			TooltipBgColor = RgbToCairoColor (0xF5, 0xF5, 0xF5);

			loadThemeFromStyle ();

			logon_background = DrawingColorFromGdk ( 
				GdkColorFromCairo (BaseColor));
			
			logon_entry_background = logon_background;
			logon_entry_foreground = Color.FromArgb (0x66, 0x66, 0x66);
			
			conv_background = Theme.DrawingColorFromGdk (GdkColorFromCairo (BaseColor));
			
			Console.WriteLine ("DrawingColor: {0}::{1}::{2}",
				logon_background.R,
				logon_background.G,
				logon_background.B);
			
			Gdk.Color c = Theme.GetGdkColor (conv_background);
			Console.WriteLine ("GDkColor: {0}>{1}>{2}",
				c.Red, c.Green, c.Blue);
				
			
			//conv_border = Color.Aqua;
			
			/*
			Debug.WriteLine ("R={0} G={1} B={2}", 
				conv_background.R,
				conv_background.G,
				conv_background.B);
			*/
			conv_header_gradient_end_color = DrawingColorFromGdk (
				GdkColorFromCairo (Theme.BgColor));
				
			conv_header_gradient_start_color = Color.White;
			
			toolbar_gradient_start_color = conv_header_gradient_end_color;
			
			toolbar_gradient_end_color = conv_background;
			
			
			//loadThemeColors ();
		}
		
		public static System.Drawing.Color DrawingColorFromGdk (Gdk.Color color)
		{
			//Console.WriteLine ("{0:X}.{1:X}.{2:X}", 
			//	(byte)color.Red, (byte)color.Green, (byte)color.Blue);
			
			return System.Drawing.Color.FromArgb (
				(byte) color.Red, 
				(byte) color.Green, 
				(byte) color.Blue);
		}
		
		public static Gdk.Color GetGdkColor (Color color)
		{
			Gdk.Color gdkcolor = new Gdk.Color (
				color.R, 
				color.G, 
				color.B);
			
			return gdkcolor;
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
			//Console.WriteLine ("Converting {0},{1},{2} to  {3},{4},{5}",
			//	red, green, blue,
			//	unit* red, unit * green, unit * blue);
				
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
		/*
		private static void loadThemeColors ()
		{
			GConf.Client client = new GConf.Client ();
			string key = (string) client.Get ("/desktop/gnome/interface/gtk_color_scheme");
			
			Console.WriteLine ("Getting val for theme:\n{0}", key);
			
			foreach (string line in key.Split ("\n".ToCharArray ())) {
				string [] field = line.Split (":".ToCharArray ());
				//int r,g,b;
					
				int r = int.Parse (field [1].Substring (1, 2),
					System.Globalization.NumberStyles.HexNumber);
				
				int g = int.Parse (field [1].Substring (5, 2),
					System.Globalization.NumberStyles.HexNumber);
					
				int b = int.Parse (field [1].Substring (9, 2),
					System.Globalization.NumberStyles.HexNumber);
				
				//Console.WriteLine ("Color {3} is ({0:X},{1:X},{2:X})", 
				//	r, g, b, field [0]);
				
				Cairo.Color cairo_color = RgbToCairoColor (r, g, b);
				
				if (field [0] == "fg_color")
					fgColor = cairo_color;
				else if (field [0] == "bg_color")
					bgColor = cairo_color;
				else if (field [0] == "text_color")
					textColor = cairo_color;
				else if (field [0] == "base_color")
					baseColor = cairo_color;
				else if (field [0] == "selected_fg_color")
					selectedFgColor = cairo_color;
				else if (field [0] == "selected_bg_color")
					selectedBgColor = cairo_color;
				else if (field [0] == "tooltip_fg_color")
					tooltipFgColor = cairo_color;
				else if (field [0] == "tooltip_bg_color_color")
					tooltipBgColor = cairo_color;
			}
			
			
		}*/
		
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
		
		public static Color LogonBackground {
			get {
				return logon_background;
			}
		}
		
		public static Color LogonEntryBackground {
			get {
				return logon_entry_background;
			}
		}
		
		public static Color LogonEntryForeground {
			get {
				return logon_entry_foreground;
			}
		}
		
		public static Color ConversationBackground {
			get {
				return conv_background;
			}
		}
		
		public static Color ConversationHeaderGradientStartColor {
			get {
				return conv_header_gradient_start_color;
			}
		}
		
		public static Color ConversationHeaderGradientEndColor {
			get {
				return conv_header_gradient_end_color;
			}
		}
		
		public static Color ToolbarGradientStartColor {
			get {
				return toolbar_gradient_start_color;
			}
		}
		
		public static Color ToolbarGradientEndColor {
			get {
				return toolbar_gradient_end_color;
			}
		}
		
	}
}
