
using System;
using Gtk;
using RickiLib.Widgets;
using RickiLib.Types;

namespace GLiveMsgr.Gui
{
	
	
	public class ArrowButton : Gtk.Button //: Gtk.VBox 
	{
		//private Gtk.Button button;
		
		//private int separator = 0;
		//private bool larrow = true; // Left arrow is current?
		
		//public event EventHandler Clicked;
		
		private Gtk.Arrow _arrow;
		
		public ArrowButton ()
		{
			_arrow = new Arrow (ArrowType.Left, ShadowType.None);
			
			Child = _arrow;
			
			Relief = ReliefStyle.None;
			SetSizeRequest (25, 30);
		}
		
		public ArrowType ArrowType {
			get { return _arrow.ArrowType; }
			set { _arrow.ArrowType = value; }
		}
		
		public Arrow Arrow {
			get { return _arrow; }
		}
	}
}

