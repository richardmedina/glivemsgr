// AliasChangeButton.cs created with MonoDevelop
// User: ricki at 11:04 11/21/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using RickiLib.Widgets;

namespace GLiveMsgr.Gui
{
	
	
	public class AliasChangeButton : RickiLib.Widgets.EditableLabelButton
	{
		private Gtk.Arrow arrowState;
		private Gtk.Image imageState;
		private ContactStateMenu stateMenu;
		
		public AliasChangeButton ()
		{
			
			
			arrowState = new Arrow (ArrowType.Down, ShadowType.None);
			stateMenu = new ContactStateMenu ();
			stateMenu.Changed += stateMenu_Changed;
			
			imageState = new Image (stateMenu.GetSelectedMenuItem ().Image.Pixbuf);
			base.HBox.Spacing = 5;
			base.HBox.PackStart (imageState, false, false, 0);
			base.HBox.PackStart (arrowState, false, false, 0);
			
			base.HBox.ReorderChild (imageState, 0);	
		}
		
		private void menuPositionFunc (Gtk.Menu menu, out int x, 
			out int y, out bool pushin)
		{
			this.GdkWindow.GetOrigin (out x, out y);
			x += this.Allocation.X;
			y += this.Allocation.Top + this.Allocation.Height;
			
			pushin = false;
		}
		
		private void stateMenu_Changed (object sender, EventArgs args)
		{
			ExtendedMenuItem item = 
				stateMenu.GetSelectedMenuItem ();
			
			imageState.Pixbuf = item.Image.Pixbuf;
		}
		
		protected override void OnClicked ()
		{	
			if (this.Allocation.Width > 100)
				stateMenu.WidthRequest = this.Allocation.Width;
			else
				stateMenu.WidthRequest = 100;
			
			stateMenu.Popup (null, null,
				menuPositionFunc,
				0, 0);
			
			base.OnClicked ();
		}
		
		public ContactStateMenu StateMenu {
			get { return stateMenu; }
		}
	}
}
