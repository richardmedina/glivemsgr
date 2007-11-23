
using System;
using Gtk;
using RickiLib.Widgets;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class CustomComboBox : Gtk.ToggleButton
	{
		private HBox hbox;
		
		private Gtk.Menu menu;
		private MsnpContactState state;
		
		private Gtk.Label labelLeft;
		private Gtk.Label labelRight;
		
		public CustomComboBox (Gtk.Menu menu)
		{
			this.menu = menu;
			hbox = new HBox (false, 5);
			
			labelLeft = Factory.Label ("");
			labelLeft.Ellipsize = Pango.EllipsizeMode.End;
				//Justification.Left);
			//labelLeft.WidthRequest = 50;
			
			labelRight = Factory.Label ("");
			
			hbox.PackStart (labelLeft, true, true, 0);
			hbox.PackEnd (new Arrow (ArrowType.Down, ShadowType.None),
				false, 
				false, 
				0);

			hbox.PackEnd (labelRight, 
				false, 
				false, 
				0);
						
			base.Child = hbox;
		}
		
		protected override void OnSizeAllocated (Gdk.Rectangle rect)
		{
			base.OnSizeAllocated (rect);
			//labelLeft.WidthRequest = labelRight.Allocation.Left - 10;
			//Debug.WriteLine (rect);
			
		}
		
		protected override void OnFocusGrabbed ()
		{
			base.OnFocusGrabbed ();
		}
		
		protected override void OnToggled ()
		{
		

		//protected override bool OnButtonReleaseEvent (Gdk.EventButton args)
		//{
			if (this.InButton)
				if (menu != null) {
					this.Active = true;
					menu.Hidden += delegate {
						this.Active = false;
					};
					
					menu.WidthRequest = this.Allocation.Width;
					menu.Popup (null, 
						null, 
						menuPositionFunc, 
						0, 
						0);
				}
			base.OnToggled ();
		}
		
		private void menuPositionFunc (Gtk.Menu menu, out int x, 
			out int y, out bool pushin)
		{
			this.GdkWindow.GetOrigin (out x, out y);
			x += this.Allocation.X;
			y += this.Allocation.Top + this.Allocation.Height;
			
			pushin = false;
		}
		
		public string Text {
			get { return labelLeft.Text; }
			set { labelLeft.Text = value; }
		}
		
		public new MsnpContactState State {
			get { return state; }
			set { 
				state = value;
				labelRight.Text = string.Format ("({0})",
					state.ToString ());
			}
		}
		
		public Gtk.Menu Menu {
			get { return menu; }
		}
	}
}
