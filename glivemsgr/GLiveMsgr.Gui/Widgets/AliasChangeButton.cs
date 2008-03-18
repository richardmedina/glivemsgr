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
		private Gtk.Arrow _arrowState;
		private Gtk.Image _imageState;
		private ContactStateMenu stateMenu;
		
		public AliasChangeButton ()
		{
			_arrowState = new Arrow (ArrowType.Down, ShadowType.None);
			stateMenu = new ContactStateMenu ();
			stateMenu.Changed += stateMenu_Changed;
			
			_imageState = new Image (
				stateMenu.GetSelectedMenuItem ().Image.Pixbuf);
			
			HBox.Spacing = 5;
			HBox.PackStart (_imageState, false, false, 0);
			HBox.PackStart (_arrowState, false, false, 0);
			
			HBox.ReorderChild (_imageState, 0);
			
			ModifyBg (StateType.Prelight,
			          Theme.GdkColorFromCairo (Theme.BaseColor));
			ModifyBg (StateType.Active,
			          Theme.GdkColorFromCairo (Theme.BgColor));
		}
		
		private void menuPositionFunc (Gtk.Menu menu, out int x, 
			out int y, out bool pushin)
		{
			GdkWindow.GetOrigin (out x, out y);
			x += Allocation.X;
			y += Allocation.Top + Allocation.Height;
			
			pushin = false;
		}
		
		private void stateMenu_Changed (object sender, EventArgs args)
		{
			ExtendedMenuItem item = 
				stateMenu.GetSelectedMenuItem ();
			
			_imageState.Pixbuf = item.Image.Pixbuf;
		}
		
		protected override void OnClicked ()
		{	
			if (Allocation.Width > 100)
				stateMenu.WidthRequest = Allocation.Width;
			else
				stateMenu.WidthRequest = 100;
			
			stateMenu.Popup (null, null, menuPositionFunc, 0, 0);
			
			base.OnClicked ();
		}
		
		public ContactStateMenu StateMenu {
			get { return stateMenu; }
		}
	}
}
