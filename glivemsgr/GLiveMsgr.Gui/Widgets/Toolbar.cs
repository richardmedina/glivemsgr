
using System;
using System.Collections;
using System.Collections.Generic;
using RickiLib.Widgets;
using Cairo;

namespace GLiveMsgr.Gui
{
	
	
	public class Toolbar : Gtk.DrawingArea
	{
		private ToolbarItemCollection items;
		private Cairo.Pattern pattern;
		
		public Toolbar ()
		{
			base.HeightRequest = 30;
			items = new ToolbarItemCollection ();
			
			base.Events = Gdk.EventMask.AllEventsMask;			
		}
		
		protected override bool OnConfigureEvent (Gdk.EventConfigure args)
		{
			Cairo.Gradient gradient = new LinearGradient (
				0, 0,
				0, Allocation.Height);
			
			gradient.AddColorStop (0, Theme.BaseColor);
			gradient.AddColorStop (1, Theme.BgColor);
			gradient.AddColorStop (2, Theme.BaseColor);
			
			pattern = gradient;
			
			return base.OnConfigureEvent (args);
		}

		
		protected override bool OnExposeEvent (Gdk.EventExpose args)
		{	
			bool ret = false; //base.OnExposeEvent (args);
			
			using (Cairo.Context context = 
				Gdk.CairoHelper.Create (args.Window)) {
				
				context.Pattern = pattern;
				context.Rectangle (0, 0, 
					Allocation.Width, Allocation.Height);
				context.Fill ();
			
			
				float x = 5;
				int hseparator = 2;
				int vseparator = 2;
			
				foreach (ToolbarItem item in items) {
					item.X = x;
					item.Y = vseparator;
					
					item.Draw (context);

					x += item.X + item.Width + hseparator;
				}
			}
		
			return ret;
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
		
		protected override bool OnMotionNotifyEvent (Gdk.EventMotion args)
		{
			int mousex = (int) args.X;
			int mousey = (int) args.Y;
			
			foreach (ToolbarItem item in items) {
				item.SendMouseOut ();
				if ((mousex >= item.X &&
					mousex <= item.X + item.Width) &&
					mousey >= item.Y &&
					mousey <= item.Y + item.Height) {
					item.SendMouseOver ();
				}
			}
		
			return base.OnMotionNotifyEvent (args);
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
			
			GdkWindow.GetPointer (out x, out y, out mask);
			
			ToolbarItem item = selected_item;
			
			if (SearchItem (x, y, out selected_item)) {
				if (item != null)
					item.SendMouseOut ();
				
				selected_item.SendMouseOver ();
				QueueResize ();
			} else {
				if (item != null) {
					item.SendMouseOut ();
					QueueResize ();
				}
			}
			
			return timer_enabled;
		}
		
		private bool SearchItem (int mousex, int mousey, out ToolbarItem i)
		{
			i = null;
			foreach (ToolbarItem item in items) {
				if ((mousex >= item.X &&
					mousex <= item.X + item.Width) &&
					mousey >= item.Y &&
					mousey <= item.Y + item.Height) {
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
/*
		protected new virtual Brush BackgroundBrush {
			get{ return backgroundBrush; }
			set { backgroundBrush = value; }
		}
*/		
		public ToolbarItemCollection Items {
			get {	
				return items;
			}
		}
	}
}
