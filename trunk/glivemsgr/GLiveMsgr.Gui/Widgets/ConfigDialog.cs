// PreferencesDialog.cs
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
using System.Collections.Generic;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class ConfigDialog : Gtk.Dialog
	{
		private System.Collections.Generic.List<IConfigPage> _pages;
		
		private ConfigCategoriesView _view_categories;
		private Gtk.Button _btn_apply;
		private Gtk.EventBox _container;
		
		public ConfigDialog ()
		{
			Title = GLiveMsgr.Gui.Settings.Instance.GetWindowTitle (
				"Customization");
			WindowPosition = Gtk.WindowPosition.CenterOnParent;
			
			_pages = new List<IConfigPage> ();
			_view_categories = new ConfigCategoriesView ();
			_view_categories.SelectionChanged += view_categoriesSelectionChanged;
			
			_container = new EventBox ();
			
			loadConfigPages ();
			
			foreach (IConfigPage page in _pages)
				_view_categories.AddCategory (page);
		
			Resize (520, 440);
			
			Gtk.HBox hbox = new HBox (false, 5);
			hbox.PackStart (_view_categories, false, false, 0);
			hbox.PackStart (_container);
			
			VBox.PackStart (hbox);
			VBox.ShowAll ();
			
			AddButton (Stock.Help, ResponseType.Help);
			
			_btn_apply = new Button (Stock.Apply);
			_btn_apply.Clicked += delegate { Save (); };
			_btn_apply.Show ();
			ActionArea.Add (_btn_apply);
			AddButton (Stock.Cancel, ResponseType.Cancel);
			AddButton (Stock.Ok, ResponseType.Ok);
		}
		
		private void loadConfigPages ()
		{
			_pages.Add (new AppearanceConfigPage ());
		}
		
		protected override void OnResponse (ResponseType response)
		{
			switch (response) {
				case ResponseType.Apply:
					Save ();
					Run ();
				break;
				case ResponseType.Ok:
					Save ();
				break;
			}
			base.OnResponse (response);
		}
		
		private void view_categoriesSelectionChanged (object sender, 
			PageActionArgs args)
		{
			if (_container.Child != null)
				_container.Remove (_container.Child);
			
			_container.Add (args.Page.GetWidget ());
			_container.ShowAll ();
		}

		private void Save ()
		{
			Console.Write ("Saving ..");
			foreach (IConfigPage page in _pages)
				page.Save ();
			Console.WriteLine ("Done");
		}
		
		public Gtk.Button ApplyButton {
			get { return _btn_apply; }
		}
	}
}
