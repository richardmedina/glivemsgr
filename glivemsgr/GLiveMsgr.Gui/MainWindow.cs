
using System;
using Gtk;
using RickiLib.Types;
using RickiLib.Widgets;
using System.Net.Protocols.Msnp;
using System.Threading;

namespace GLiveMsgr.Gui
{
	
	
	public class MainWindow : PopupWindow
	{
		private MsnpAccount account;
		private MainWidget mainWidget;
		private Gtk.StatusIcon icon;
		private NotificationIcon notification;
		
		private WindowCollection windows;
		
		private Gtk.VBox vbox;
		
		private Gdk.Cursor _cursor_hand = new Gdk.Cursor (Gdk.CursorType.Hand2);
		
		private bool _notifCreated = false;
		
		public MainWindow ()
		{
			Title = GLiveMsgr.Gui.Settings.Instance.GetWindowTitle (
				"Login");
			Decorated = false;
			Icon = Gdk.Pixbuf.LoadFromResource ("messenger_icon.png");
			
			ModifyBg (StateType.Normal, 
				Theme.GdkColorFromCairo (Theme.BaseColor));
			
			WindowPosition = Gtk.WindowPosition.Center;
			
			LogoVisible = true;
			
			account = new MsnpAccount ();
			account.ConversationRequest += account_ConversationRequest;
			
			windows = new WindowCollection ();
			
			mainWidget = new MainWidget (account);
			
			vbox = new VBox (false, 0);
			
			vbox.PackStart (mainWidget);
			
			Add (vbox);
			SetSizeRequest (280, 530);
			
			Theme.Modified += delegate { theme_refresh (); };
			//showNotification ();
		}
		
		public void Quit ()
		{
			mainWidget.Account.Logout ();
			
			Application.Quit ();
		}
				
		private void showNotification ()
		{
		}
		
		protected override bool OnButtonPressEvent (Gdk.EventButton evnt)
		{
			if (evnt.Button == 1) { // && 
				//evnt.Type == Gdk.EventType.TwoButtonPress) {
				if (evnt.X > LogoPosX &&
					evnt.Y > LogoPosY &&
					evnt.X < LogoPosX + LogoWidth &&
					evnt.Y < LogoPosY + LogoHeight) {
						ConfigDialog dlg = new ConfigDialog ();
					dlg.ApplyButton.Clicked += delegate { Theme.SendModified (); };
					dlg.Run ();
					dlg.Destroy ();
					Theme.SendModified ();
					return false;
				}
			}
			
			return base.OnButtonPressEvent (evnt);
		}
		
		protected override bool OnMotionNotifyEvent (Gdk.EventMotion evnt)
		{
			if (evnt.X > LogoPosX &&
					evnt.Y > LogoPosY &&
					evnt.X < LogoPosX + LogoWidth &&
					evnt.Y < LogoPosY + LogoHeight) {
				
					evnt.Window.Cursor = _cursor_hand;
					return false;
				}
			else 
				evnt.Window.Cursor = null;
		
			return base.OnMotionNotifyEvent (evnt);
		}


		protected override void OnShown ()
		{
			base.OnShown ();
			if (!_notifCreated) {
				notification = new NotificationIcon (this);
				_notifCreated = true;
			}
		}
		
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			bool ret = base.OnExposeEvent (evnt);
			
			using (Cairo.Context context = Gdk.CairoHelper.Create (evnt.Window)) {
				Pango.Layout l = Pango.CairoHelper.CreateLayout (context);
				
				l.FontDescription = this.Style.FontDesc;
				l.SetMarkup ("<small><b>GNOME Live Messenger\nby Ricki Medina</b></small>");
			
				l.Alignment = Pango.Alignment.Right;
				
				int width,height;
				l.GetPixelSize (out width, out height);
				double x = this.Allocation.Width - width - 5;
				
				context.MoveTo (x, 5);
				
				Cairo.Color color = Theme.TextColor;
				color.A = 0.5f;
				
				context.Color = color;
				Pango.CairoHelper.ShowLayout (context, l);			
			}
			
			return ret;
		}

		protected override bool OnDeleteEvent (Gdk.Event args)
		{
			Hide ();
			return true;
		}
		
		protected override bool OnKeyPressEvent (Gdk.EventKey args)
		{
			//if (args.Key == Gdk.Key.Alt_L) {
				// Show/Hide WM Decorations and Menubar
				//Decorated = !Decorated;
			//}
			return base.OnKeyPressEvent (args);
		}


		private void account_ConversationRequest (object sender,
			ConversationRequestArgs args)
		{
			ThreadNotify tn = new ThreadNotify (delegate {
				// FIXME. Find if conversation already is in window
				ConversationWindow win = new ConversationWindow (args.Conversation);
				windows.Add (win);
				win.ShowAll ();
				account.Conversations.Removed += account_Conversations_Removed;
			});
			
			tn.WakeupMain ();
		}
		
		private void account_Conversations_Removed (object sender,
			WatchedCollectionEventArgs <MsnpConversation> args)
		{
			//args.Instance
			foreach (ConversationWindow window in windows) {
				if (args.Instance == window.Conversation)
					Console.WriteLine ("Conversation found"); 
			}
		}
		
		private void theme_refresh ()
		{
			QueueDraw ();
			foreach (Gtk.Window win in windows)
				win.QueueDraw ();
		}
		
		public MsnpAccount Account {
			get { return account; }
		}
	}
}
