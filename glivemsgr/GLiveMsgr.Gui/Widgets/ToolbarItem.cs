
using System;
using Cairo;

namespace GLiveMsgr.Gui
{
	
	
	public abstract class ToolbarItem
	{
		//private Point location;
		//private Size size;
		
		private float _x;
		private float _y;
		private float _width;
		private float _height;
		
		private bool _hasMouseOver;
		
		public event EventHandler _mouseOut;
		public event EventHandler _mouseOver;
		public event EventHandler _clicked;
		
		public ToolbarItem ()
		{
			//location = new Point (0, 0);
			//location.X = 10;
			X = 0;
			Y = 10;
			
			_mouseOut = onMouseOut;
			_mouseOver = onMouseOver;
			_clicked = onClicked;
			
			_hasMouseOver = false;
		}
		
		public abstract void Draw (Cairo.Context context);
		
		internal void SendMouseOut ()
		{
			this.HasMouseOver = false;
			_mouseOut (this, EventArgs.Empty);
		}
		
		internal void SendMouseOver ()
		{
			this.HasMouseOver = true;
			_mouseOver (this, EventArgs.Empty);
		}
		
		internal void SendMouseClicked ()
		{
			_clicked (this, EventArgs.Empty);
		}
		
		protected virtual void OnClicked ()
		{
		}
		
		protected virtual void OnMouseOut ()
		{
		}
		
		protected virtual void OnMouseOver ()
		{
		}
		
		private void onClicked (object sender, EventArgs args)
		{
			OnClicked ();
		}
		
		private void onMouseOut (object sender, EventArgs args)
		{
			OnMouseOut ();
		}
		
		private void onMouseOver (object sender, EventArgs args)
		{
			OnMouseOver ();
		}
		
		public virtual float X {
			get { return _x; }
			set { _x= value; }
		}
		
		public virtual float Y {
			get { return _y; }
			set { _y = value; }
		}
		
		public virtual float Width {
			get { return _width; }
			set { _width = value; }
		}
		
		public virtual float Height {
			get { return _height; }
			set { _height = value; }
		}
		
		
		public bool HasMouseOver {
			get { return _hasMouseOver; }
			protected set { _hasMouseOver = value; }
		}
		
		
		public event EventHandler MouseOut {
			add { _mouseOut += value; }
			remove { _mouseOut -= value; }
		}
		
		public event EventHandler MouseOver {
			add { _mouseOver += value; }
			remove { _mouseOver -= value; }
		}
		
		public event EventHandler Clicked {
			add { _clicked += value; }
			remove { _clicked -= value; }
		}
//		public abstract System.Drawing.Image Image { get; }
//		public abstract System.Drawing.Size Size { get; }
		public abstract Cairo.Color Color { get; }
		
	}
}
