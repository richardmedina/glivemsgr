
using System;
using Gtk;
using System.Drawing;
using System.Drawing.Drawing2D;
using RickiLib.Widgets;

//using Cairo;

using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationHeaderWidget : RickiLib.Widgets.DrawingAreaGDI
	{	
		private System.Drawing.Brush brush;
		
		private string _remoteUser;
		private string _remoteAlias;
		
		public ConversationHeaderWidget (string remoteUser, string remoteAlias)
		{
			_remoteUser = remoteUser;
			_remoteAlias = remoteAlias;
			
			base.HeightRequest = 30;
			
			
			//base.ModifyBg (StateType.Normal,
			//	new Gdk.Color (0xFF, 0xFF, 0xFF));
			
			//init ();
		}
		/*
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			Cairo.Context context = Gdk.CairoHelper.Create (evnt.Window);
			
			Gradient gradient = 
				new Cairo.LinearGradient (
					0, 0,
					0, Allocation.Height);
		
			gradient.AddColorStop (0, Theme.BaseColor);
			gradient.AddColorStop (1, Theme.SelectedBgColor);
			
			context.Pattern = gradient;
			
			context.Rectangle (0, 0, Allocation.Width, Allocation.Height);
			
			context.Fill ();
			
			((IDisposable) context.Target).Dispose ();
			((IDisposable) context).Dispose ();
			
		
			return base.OnExposeEvent (evnt);
		}

		*/
		protected override void OnPaint (Graphics graphics)
		{
			brush = new LinearGradientBrush (
				new Point (0, 0),
				new Point (0, Allocation.Height),
				Theme.ConversationHeaderGradientEndColor,
				Theme.ConversationHeaderGradientStartColor
			);
			
			graphics.FillRectangle (
				brush,
				0, 0,
				this.Allocation.Width,
				this.Allocation.Height
			
			);			
			
			Brush fbrush = new SolidBrush (Color.Black);

			Font font = new Font ("Sans", 
				8, FontStyle.Regular);
			
			
			System.Drawing.Image image = new System.Drawing.Bitmap (
				"images/typing.png"
			);
			
			graphics.DrawImage (image, new Point (5, 3));
			
			graphics.DrawString (_remoteAlias, 
				new Font (font, FontStyle.Bold | FontStyle.Italic),
				fbrush,
				20, 3);
			
			fbrush = new SolidBrush (Color.Gray);
			
			graphics.DrawString (
				string.Format ("<{0}>", _remoteUser),
				font,
				fbrush,
				30, 15);
		}
	}
}
