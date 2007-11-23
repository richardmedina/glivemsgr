
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using RickiLib.Widgets;

namespace GLiveMsgr.Gui
{
	
	
	public class Toolbar : RickiLib.Widgets.DrawingAreaGDI
	{
		/*
		private Gdk.Pixmap pixmap;
		private System.Drawing.Graphics g;
		private System.Drawing.Pen pen;
		private System.Drawing.Brush brush;
		*/
		private ToolbarItemCollection items;
		private Brush backgroundBrush;
		
		public Toolbar ()
		{
			this.backgroundBrush = null;
			base.HeightRequest = 30;
			items = new ToolbarItemCollection ();
			
			base.Events = Gdk.EventMask.AllEventsMask;			
		}
		
		protected override bool OnConfigureEvent (Gdk.EventConfigure args)
		{
			if (backgroundBrush == null)
				backgroundBrush = new LinearGradientBrush (
					new Point (0, 0),
					new Point (0, Allocation.Height),
					Theme.ToolbarGradientStartColor,
					Theme.ToolbarGradientEndColor
				);
			
			return base.OnConfigureEvent (args);
		}

		
		protected override void OnPaint (Graphics graphics)
		{	
			Pen border = new Pen (Color.LightBlue);
			
			graphics.FillRectangle (
				BackgroundBrush,
				0, 0,
				this.Allocation.Width,
				this.Allocation.Height
			);
			
			int x = 5;
			int hseparator = 2;
			int vseparator = 2;
			
			//Debug.WriteLine ("Drawing buttons {0}", items.Count);
			foreach (ToolbarButton button in items) {
				button.Location = new Point (x, vseparator);
				
				if (button.HasMouseOver) {					
						graphics.DrawRectangle (
						border,
						button.Location.X, 
						button.Location.Y,
						button.Image.Width, 
						button.Image.Height);
				}
				
				graphics.DrawImage (button.Image, button.Location);
				x += button.Location.X + button.Image.Width + hseparator;
				//y += button.Image.Height;
			}
		
		}
		
		
		protected override bool OnEnterNotifyEvent (Gdk.EventCrossing args)
		{
			if (!timer_enabled) {
				timer_enabled = true;
				GLib.Timeout.Add (50, onTimer);
				
			}
			
			timer_killed = false;
			
			return base.OnEnterNotifyEvent (args);
		}
		
		protected override bool OnLeaveNotifyEvent (Gdk.EventCrossing args)
		{
			timer_killed = true;
			
			return base.OnLeaveNotifyEvent (args);
		}
		
		
		private bool timer_enabled = false;
		private bool timer_killed = true;
		
		private ToolbarItem selected_item = null;
		
		private bool onTimer ()
		{
			if (timer_killed) {
				timer_enabled = false;
				timer_killed = true;
			}
			int x, y;
			Gdk.ModifierType mask;
			
			this.GdkWindow.GetPointer (out x, out y, out mask);
			
			ToolbarItem item = selected_item;
			
			if (SearchItem (x, y, out selected_item)) {
				if (item != null)
					item.SendMouseOut ();
				
				selected_item.SendMouseOver ();
				base.QueueResize ();
			} else {
				if (item != null) {
					item.SendMouseOut ();
					base.QueueResize ();
				}
			}
			
			return timer_enabled;
		}
		
		private bool SearchItem (int mousex, int mousey, out ToolbarItem i)
		{
			i = null;
			foreach (ToolbarItem item in items) {
				if ((mousex >= item.Location.X &&
					mousex <= item.Location.X + item.Size.Width) &&
					mousey >= item.Location.Y &&
					mousey <= item.Location.Y + item.Size.Height) {
					i = item;
					return true;
				}
			}
			
			return false;
		}
		
		protected override bool OnButtonReleaseEvent (Gdk.EventButton args)
		{
			ToolbarItem item;
			
			if (SearchItem ((int) args.X, (int) args.Y, out item)) {
				item.SendMouseClicked ();
			}
			
			return base.OnButtonReleaseEvent (args);
		}

		protected new virtual Brush BackgroundBrush {
			get{ return backgroundBrush; }
			set { backgroundBrush = value; }
		}
		
		public ToolbarItemCollection Items {
			get {	
				return items;
			}
		}
	}
}
