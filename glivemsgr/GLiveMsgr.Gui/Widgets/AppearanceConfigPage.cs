// AppearanceConfigPage.cs
// 
// Copyright (C) 2009 Ricardo Medina López
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Gtk;
using RickiLib.Widgets;

namespace GLiveMsgr.Gui
{
	
	
	public class AppearanceConfigPage : IConfigPage
	{
		private Gtk.VBox _vbox;
		private Gtk.ComboBox _combo_colortype;
		private Gtk.ColorButton _btn_color1;
		private Gtk.ColorButton _btn_color2;
		
		public AppearanceConfigPage()
		{
			_vbox = new Gtk.VBox (false, 5);
			_vbox.BorderWidth = 10;
			_combo_colortype = new Gtk.ComboBox (
				new string []
					{"Solid color","Gradient"});
			_combo_colortype.Active = color_equals (Theme.BgColor, 
				Theme.BaseColor)?0:1;
			
			_combo_colortype.Changed += combo_colortypeChanged;
			
			_btn_color1 = new ColorButton (
				Theme.GdkColorFromCairo (Theme.BgColor));
			_btn_color2 = new ColorButton (
				Theme.GdkColorFromCairo (Theme.BaseColor));
			
			_vbox.PackStart (Factory.Label ("<b><big>Colors</big></b>", 100, Justification.Left), false, false, 0);
			
			Gtk.HBox hbox = new HBox (false, 5);
			hbox.PackStart (_combo_colortype, false, false, 0);
			hbox.PackStart (_btn_color1, false, false, 0);
			hbox.PackStart (_btn_color2, false, false, 0);
			
			_vbox.PackStart (hbox, false, false, 0);
			_vbox.Shown += vboxShown;
		}
				
		public void Save ()
		{
			Theme.BgColor = Theme.CairoColorFromGdk (_btn_color1.Color);
			Theme.BaseColor = Theme.CairoColorFromGdk (_btn_color2.Color);
		}

		public Widget GetWidget ()
		{
			return _vbox;
		}
		
		private void vboxShown (object sender, EventArgs args)
		{
			if (_combo_colortype.Active == 0)
				_btn_color2.Hide ();
		}
		private void combo_colortypeChanged (object sender, EventArgs args)
		{
			if (_combo_colortype.Active == 0)
				_btn_color2.Hide ();
			else
				_btn_color2.Show ();
		}

		private bool color_equals (Cairo.Color color1, Cairo.Color color2)
		{
			if (color1.A == color2.A &&
				color1.B == color2.B &&
				color1.G == color2.G &&
				color1.R == color2.R)
				return true;
			
			return false;
		}

		
		public string Title {
			get { return "Appearance"; }
		}


	}
}
