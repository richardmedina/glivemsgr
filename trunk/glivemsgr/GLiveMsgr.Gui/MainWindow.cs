
using System;
using Gtk;
using RickiLib.Widgets;
using System.Net.Protocols.Msnp;
using System.Threading;

namespace GLiveMsgr.Gui
{
	
	
	public class MainWindow : CustomWindow
	{
		private MsnpAccount account;
		private MainWidget mainWidget;
		private NotificationIcon notification;
		
//		private Gdk.Cursor resizeCursor;
		
		public MainWindow () : base (WindowType.Toplevel)
		{
			Title = "GNOME Live Messenger - by Ricki Medina";
			Decorated = false;
			AddEvents ((int) Gdk.EventMask.ButtonPressMask);
			ModifyBg (StateType.Normal, 
				Theme.GdkColorFromCairo (Theme.BaseColor));
			
			account = new MsnpAccount ();
			account.ConversationRequest += account_ConversationRequest;
//			resizeCursor = new Gdk.Cursor (Gdk.CursorType.BottomRightCorner);
			
			
			mainWidget = new MainWidget (account);
			VBox.PackStart (mainWidget);
			
			Resize (280, 530);
			showNotification ();			
		}
		
		public void Quit ()
		{
			mainWidget.Account.Logout ();
			
			Application.Quit ();
		}
				
		private void showNotification ()
		{
			//notification.
			notification = new NotificationIcon (this);
			
			
			ThreadNotify notif = new ThreadNotify (
				delegate {
					notification.ShowAll ();
					//base.ShowAll ();
				});
			
			notif.WakeupMain ();
		}
		
		protected override bool OnButtonPressEvent (Gdk.EventButton evnt)
		{
			if (evnt.X > Allocation.Width - 15 &&
				evnt.Y > Allocation.Height - 15)
				GdkWindow.BeginResizeDrag (Gdk.WindowEdge.SouthEast,
					1, (int) evnt.XRoot, (int)  evnt.YRoot, evnt.Time);
			else if (evnt.X > Allocation.Width - 15)
				GdkWindow.BeginResizeDrag (Gdk.WindowEdge.East,
					1, (int) evnt.XRoot, (int) evnt.YRoot, evnt.Time);
			else if (evnt.Y > Allocation.Height - 15)
				GdkWindow.BeginResizeDrag (Gdk.WindowEdge.South,
					1, (int) evnt.XRoot, (int) evnt.YRoot, evnt.Time);
			else
				GdkWindow.BeginMoveDrag (1, (int) evnt.XRoot, (int) evnt.YRoot, evnt.Time);
			
			return base.OnButtonPressEvent (evnt);
		}
						
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			bool ret = base.OnExposeEvent (evnt);
			
			Cairo.Context context = Gdk.CairoHelper.Create (evnt.Window);
			
			context.Rectangle (1, 1, this.Allocation.Width -2, 50);
			
			Cairo.Gradient grad = new Cairo.LinearGradient (
				0, 0,
				0, 50);
			
			grad.AddColorStop (0, 
				Theme.BgColor);
			
			grad.AddColorStop (1,
				Theme.BaseColor);
			
			context.Pattern = grad;
			
			context.Fill ();
			
			context.Color = Theme.TextColor; 
			
			Pango.Layout l = Pango.CairoHelper.CreateLayout (context);
			l.SetMarkup ("<b>GNOME Live Messenger</b>\nby Ricki Medina");
			l.FontDescription = new Pango.FontDescription ();
			l.FontDescription.Weight = Pango.Weight.Bold;
			l.FontDescription.Size = Pango.Units.FromPixels (5);
			l.Alignment = Pango.Alignment.Right;
			
			int width,height;
			l.GetPixelSize (out width, out height);
			double x = this.Allocation.Width - width - 5;
			
			context.MoveTo (x, 5);
			Pango.CairoHelper.ShowLayout (context, l);			
			
			Cairo.ImageSurface imageSurf = 
				new Cairo.ImageSurface ("gnome-logo.png");
			
			imageSurf.Show (context,
				Allocation.Width - imageSurf.Width - 5,
				Allocation.Height - imageSurf.Height - 5);

			
			((IDisposable) context.Target).Dispose ();
			((IDisposable) context).Dispose ();
			
			return ret;
		}
		
		protected override bool OnDeleteEvent (Gdk.Event args)
		{
			//if (mainWidget.Account != null)
			//	mainWidget.Account.Logout ();
			
			Hide ();
			return true;//base.OnDeleteEvent (args);
		}
		
		private void account_ConversationRequest (object sender,
			ConversationRequestArgs args)
		{
			ThreadNotify tn = new ThreadNotify (delegate {
				ConversationWindow win = new ConversationWindow (args.Conversation);
				win.ShowAll ();
			});
			
			tn.WakeupMain ();
		}

		
		public MsnpAccount Account {
			get { return account; }
		}
	}
}
