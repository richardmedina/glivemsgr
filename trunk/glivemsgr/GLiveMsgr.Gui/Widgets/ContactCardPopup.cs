// ContactCardPopup.cs created with MonoDevelop
// User: ricki at 02:53 03/20/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net.Protocols.Msnp;
using Gtk;
using RickiLib.Widgets;

namespace GLiveMsgr.Gui
{
	
	public class ContactCardPopup : PopupWindow
	{
		private MsnpContact _contact;
		
		private LinkWidget _emailLink;
		
		private Gtk.Label _aliasLabel;
		
		private int _time = 0;
		
		private int _gracex = 10;
		private int _gracey = 0;
		
		private bool _timeoutActive = false;
		
		private Gdk.Screen _screen = Gdk.Screen.Default; 
		
		private Gtk.VBox _vbox;
		
		public ContactCardPopup () : this (null)
		{
		}
		
		public ContactCardPopup (MsnpContact contact) : 
			base (WindowType.Popup)
		{
			Decorated = false;
			KeepAbove = true;
			Resize (150, 150);
			LogoVisible = true;
			
			_contact = contact;
			
			init ();
		}
		
		protected override void OnRealized ()
		{
			base.OnRealized ();
		}

		
		protected override bool OnEnterNotifyEvent (Gdk.EventCrossing args)
		{
			setPosition ((int) args.XRoot, (int) args.YRoot);
			
			return base.OnEnterNotifyEvent (args);
		}
		
		private void setPosition (int x, int y)
		{
			x += _gracex;
			y += _gracey;
			
			if (x + Allocation.Width > _screen.Width)
				x = x - Allocation.Width - (_gracex * 2);
			
			if (y + Allocation.Height > _screen.Height)
				y = y - Allocation.Height - (_gracey * 2);
			
			Move (x, y);
			Resize (150, 150);
		}

		
		private void init ()
		{
			_emailLink = new LinkWidget (string.Empty);
			_emailLink.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BaseColor));
				
			_aliasLabel = new Label ();
			_aliasLabel.Wrap = true;
			_aliasLabel.Justify = Justification.Center;
			
			_vbox = new VBox (false, 5);
			
			_vbox.PackStart (_emailLink, false, false, 0);
			_vbox.PackStart (_aliasLabel);
			
			_vbox.ShowAll ();
			update ();
			
			Add (_vbox);
		}
		
		public void Flash (int x, int y)
		{
			_time = 1;
			setPosition (x, y);
			if (!Visible)
				Present ();
			
			if (!_timeoutActive) {
				_timeoutActive = true;
				GLib.Timeout.Add (1000, timeout);
			}
		}
		
		private bool timeout ()
		{
			if (_time == 0) {
				Hide ();
				_timeoutActive = false;
				return false;
			}
			
			_time --;
			return true;
		}
		
		private void update ()
		{
			if (_contact != null) {
				_emailLink.Text = _contact.Username;
				_aliasLabel.Text = _contact.Alias;
			} else {
				_emailLink.Text = "...";
				_aliasLabel.Text =  "Not Avaliable";
			}
		}
		
		public MsnpContact Contact {
			get { return _contact; }
			set { 
				_contact = value;
				update (); 
			}
		}
	}
}
