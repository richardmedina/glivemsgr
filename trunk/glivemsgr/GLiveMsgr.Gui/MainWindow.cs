
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
			
			AppPaintable = true;
			
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
			
			context.Rectangle (1, 1, this.Allocation.Width -2, 55);
			
			Cairo.Gradient grad = new Cairo.LinearGradient (
				0, 0,
				0, 55);
			
			grad.AddColorStop (0, 
				Theme.BgColor);
			
			grad.AddColorStop (1,
				Theme.BaseColor);
			
			context.Pattern = grad;
			
			context.Fill ();
			
			Cairo.Color c = Theme.TextColor;
			c.A = 0.5f;
			context.Color = c;
			
			
			Pango.Layout l = Pango.CairoHelper.CreateLayout (context);
			//l.FontDescription = new Pango.FontDescription ();
			l.FontDescription = this.Style.FontDesc;
			l.SetMarkup ("<small><b>GNOME Live Messenger\nby Ricki Medina</b></small>");
			
			//l.FontDescription.Weight = Pango.Weight.Bold;
			//l.FontDescription.Size = Pango.Units.FromPixels (5);
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

			double lw = 2;
			
			context.NewPath ();
			context.LineWidth = lw;
			context.LineCap = Cairo.LineCap.Round;
			context.LineJoin = Cairo.LineJoin.Bevel;
			context.Color = Theme.BgColor;//new Cairo.Color (0.7f, 0.7f, 0.7f);
			
			double mx = 0 + (lw/2);
			double my = 0 + (lw/2);
			double mw = Allocation.Width - lw;
			double mh = Allocation.Height - lw;
			
			context.MoveTo (mx, my);
			context.LineTo (mw, mx);
			context.LineTo (mw, mh);
			context.LineTo (mx, mh);
			context.ClosePath ();
			context.Stroke ();

			
			((IDisposable) context.Target).Dispose ();
			((IDisposable) context).Dispose ();
			
			//Child.SendExpose (evnt);
			return ret;
		}
		
		protected override bool OnDeleteEvent (Gdk.Event args)
		{
			//if (mainWidget.Account != null)
			//	mainWidget.Account.Logout ();
			
			Hide ();
			return true;//base.OnDeleteEvent (args);
		}

		int w =0, h = 0;
		Gdk.Pixmap pixmap;
		
		protected override void OnSizeAllocated (Gdk.Rectangle alloc)
		{
			base.OnSizeAllocated (alloc);

			w = alloc.Width;
			h = alloc.Height;
			
			pixmap = new Gdk.Pixmap (GdkWindow, alloc.Width, alloc.Height, 1);
			
			using(Gdk.GC gc = new Gdk.GC(pixmap))
			{
				Gdk.Colormap colormap = Gdk.Colormap.System;
				Gdk.Color white = new Gdk.Color(255, 255, 255);
				colormap.AllocColor(ref white, true, true);

				Gdk.Color black = new Gdk.Color(0, 0, 0);
				colormap.AllocColor(ref black, true, true);

				gc.Foreground = white;
				
				pixmap.DrawRectangle(gc, true, 0, 0, w, h);

				gc.Foreground = black;
				
				for (int i = 0; i < 3; i ++) {
					pixmap.DrawPoint (gc, i, 0);
					pixmap.DrawPoint (gc, 0, i);
				}
				
				for (int i = 0; i < 3; i ++) {
					pixmap.DrawPoint (gc,
						Allocation.Width -1,
						i);
					pixmap.DrawPoint (gc, 
						Allocation.Width -1 - i, 
						0);
				}
				
				for (int i = 0; i < 3; i ++) {
					pixmap.DrawPoint (gc,
						Allocation.Width -1 -i,
						Allocation.Height - 1);
					pixmap.DrawPoint (gc,
						Allocation.Width -1,
						Allocation.Height - 1 - i);
				}
				
				for (int i = 0; i < 3; i ++) {
					pixmap.DrawPoint (gc,
						i,
						Allocation.Height - 1);
					pixmap.DrawPoint (gc,
						0,
						Allocation.Height -1 -i);
				}
			}
			
			ShapeCombineMask (pixmap, 0, 0);
			pixmap.Dispose ();
			//return ret;
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
