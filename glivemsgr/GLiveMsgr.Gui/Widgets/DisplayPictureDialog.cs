// DisplayPictureWindow.cs
// 
// Copyright (C) 2009 Ricaardo Medina López
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
	
	// TODO. This dialog will show a picture catalog
	public class DisplayPictureWindow : Gtk.Dialog
	{
		private Gtk.IconView _iconPictures;
				
		public DisplayPictureWindow() : base ()
		{			
			Resize (420, 320);
			
			_iconPictures = new IconView ();
			Gtk.ScrolledWindow sw = new ScrolledWindow ();
			sw.AddWithViewport (_iconPictures);
			VBox.PackStart (
				Factory.Label ("<big><b>Display Picture.</b></big>\nSelect your display picture", 
					Allocation.Width -10, 
					Justification.Left), 
				false, false, 0);
			
			VBox.PackStart (sw, true, true, 5);
			
			VBox.ShowAll ();
			AddButton (Stock.Cancel, ResponseType.Cancel);
			AddButton (Stock.Apply, ResponseType.Apply);
			AddButton (Stock.Ok, ResponseType.Ok);
			AddButton (Stock.Help, ResponseType.Help);
		}
		
		public new Gtk.ResponseType Run ()
		{
			Gtk.ResponseType response;
			
			do {
				response = (Gtk.ResponseType) base.Run ();
			} while (response != ResponseType.Ok &&
				response != ResponseType.Cancel &&
				response != ResponseType.DeleteEvent);
			
			return response;
		}
	}
}
