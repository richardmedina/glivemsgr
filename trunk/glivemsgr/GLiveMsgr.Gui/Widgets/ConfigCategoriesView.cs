// ConfigCategoriesView.cs
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

namespace GLiveMsgr.Gui
{
	
	// TODO. Must throw an Event when selection is changed
	public class ConfigCategoriesView : Gtk.TreeView
	{
		private Gtk.ListStore _store;
		private Gtk.CellRendererText _renderer;
		
		private event PageActionHandler _selection_changed; 
		
		
		public ConfigCategoriesView ()
		{
			_store = new ListStore (
				typeof (string), 
				typeof (IConfigPage));
			
			Model = _store;
			
			_renderer = new CellRendererText ();
			AppendColumn ("Category", _renderer, "text", 0);
			Selection.Changed += selectionChanged;
			
		}
		
		public void AddCategory (IConfigPage page)
		{
			_store.AppendValues (page.Title, page);
		}
		
		protected virtual void OnSelectionChanged (IConfigPage page)
		{
			_selection_changed (this, new PageActionArgs (page));
		}
		
		private void selectionChanged (object sender, EventArgs args)
		{
			Gtk.TreeIter iter; 
			if (Selection.GetSelected (out iter)) {
				IConfigPage page = (IConfigPage) _store.GetValue (iter, 1);
				OnSelectionChanged (page);
			}
			Console.WriteLine ("Selection Changed");
		}
		
		public event PageActionHandler SelectionChanged {
			add { _selection_changed += value; }
			remove { _selection_changed -= value; }
		}
	}
}
