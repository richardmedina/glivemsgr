
using System;
using Gtk;
using RickiLib.Widgets;
using RickiLib.Types;
using System.Drawing;

namespace GLiveMsgr.Gui
{
	
	
	public class HidePicturesPanel : Gtk.VBox 
	{
		private DrawingAreaGDI button;
		
		private int separator = 0;
		private bool larrow = true; // Left arrow is current?
		
		public event EventHandler Clicked;
		
		public HidePicturesPanel ()
		{
			button = new DrawingAreaGDI ();
			button.Events |= Gdk.EventMask.ButtonReleaseMask |
				Gdk.EventMask.ButtonPressMask;
			button.SetSizeRequest (separator + 12, 30);
			button.Paint += button_onPaint;
			button.ButtonReleaseEvent += button_ButtonReleaseEvent;
			
			Clicked = new EventHandler (onClicked);
			
			
			base.PackStart (button, false, false, 0);
		}
		
		private void onClicked (object sender, EventArgs args)
		{
		}
		
		private void button_ButtonReleaseEvent (object sender,
			ButtonReleaseEventArgs args)
		{
			Clicked (this, EventArgs.Empty);
			this.larrow = !larrow;
			button.QueueResize ();
		}
		
		private void button_onPaint (object sender, RickiLib.Types.PaintArgs args)
		{
			Pen pen = new Pen (Color.LightBlue);
			pen.Width = 2;
			Pen borderPen = new Pen (Color.DarkGray);
			borderPen.Width = 3;
			
			args.Graphics.DrawRectangle (
				pen,
				0, 
				0,
				button.Allocation.Width - separator,
				button.Allocation.Height - pen.Width
			);
			
			Brush brush = new SolidBrush (Theme.ConversationBackground);
			args.Graphics.FillRectangle (
				brush,
				button.Allocation.Width - separator + pen.Width,
				0,
				button.Allocation.Width,
				button.Allocation.Height
				
			);
			
			int centery = button.Allocation.Height / 2;
			
			Point [] left_arrow = new Point [] {
				new Point (
					button.Allocation.Width + (int) pen.Width, 
					centery - 7),
				new Point ((int) pen.Width *3 , centery),
				new Point (
					button.Allocation.Width + (int) pen.Width, 
					centery + 7)
			};
			
			
			Point [] right_arrow = new Point [] {
				new Point ((int) pen.Width *2, centery - 7),
				new Point (button.Allocation.Width - (int) pen.Width, 
					centery),
				new Point ((int) pen.Width *2, centery + 7)
			};
			
			Point [] current_arrow = left_arrow;
			
			if (larrow) {
				current_arrow = right_arrow;
			}
			
			args.Graphics.DrawLines (
				borderPen,
				current_arrow
			);		
		}
		
		public bool IsLeftArrow {
			get {
				return larrow;
			}
		}
	}
}

