
using System;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class WMButtons : Gtk.HBox
	{
		
		private Gtk.Button buttonMin;
		private Gtk.Button buttonMax;
		private Gtk.Button buttonClose;
		
		private Gtk.Window parent;
		
		public WMButtons (Gtk.Window parent)
		{
			this.parent = parent;
			
			buttonMin = createButton (Stock.GoDown);
			buttonMin.Clicked += delegate {
				this.parent.GdkWindow.Iconify ();
			};
			
			buttonMax = createButton (Stock.GoUp);
			buttonMax.Clicked += delegate {
				if ((int)(this.parent.GdkWindow.State & Gdk.WindowState.Maximized) > 0)
					this.parent.GdkWindow.Unmaximize ();
				else
					this.parent.GdkWindow.Maximize ();
			};
			
			buttonClose = createButton (Stock.Close);
			
			base.PackStart (buttonMin, false, false, 0);
			base.PackStart (buttonMax, false, false, 0);
			base.PackStart (buttonClose, false, false, 0);
		}
		
		private Gtk.Button createButton (string stock_id)
		{
			Gtk.Button button = new Button ();
			
			button.Child = new Image (stock_id,
				IconSize.Button);
			
			button.SetSizeRequest (20, 20);
			button.Relief = ReliefStyle.None;
			
			return button;
		}
		
		public Gtk.Button MinButton {
			get { return buttonMin; }
		}
		
		public Gtk.Button MaxButton {
			get { return buttonMax; }
		}
		
		public Gtk.Button CloseButton {
			get { return buttonClose; }
		}
		
	}
}
