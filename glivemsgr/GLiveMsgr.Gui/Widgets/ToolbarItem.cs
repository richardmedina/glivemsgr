
using System;
using System.Drawing;

namespace GLiveMsgr.Gui
{
	
	
	public abstract class ToolbarItem
	{
		private Point location;
		private Size size;
		private bool hasMouseOver;
		
		public event EventHandler MouseOut;
		public event EventHandler MouseOver;
		public event EventHandler Clicked;
		
		public ToolbarItem ()
		{
			location = new Point (0, 0);
			location.X = 10;
			
			MouseOut = onMouseOut;
			MouseOver = onMouseOver;
			Clicked = onClicked;
			
			hasMouseOver = false;
		}
		
		//public abstract void Draw (Graphics graphics);
		
		internal void SendMouseOut ()
		{
			this.HasMouseOver = false;
			MouseOut (this, EventArgs.Empty);
		}
		
		internal void SendMouseOver ()
		{
			this.HasMouseOver = true;
			MouseOver (this, EventArgs.Empty);
		}
		
		internal void SendMouseClicked ()
		{
			Clicked (this, EventArgs.Empty);
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
		
		public Point Location {
			get {
				return location;
			}
			set {
				location = value;
			}
		}
		
		public virtual Size Size {
			get { return size; }
			set { size = value; }
		}
		
		public bool HasMouseOver {
			get {
				return hasMouseOver;
			}
			protected set {
				hasMouseOver = value;
			}
		}
		
//		public abstract System.Drawing.Image Image { get; }
//		public abstract System.Drawing.Size Size { get; }
		public abstract System.Drawing.Brush Brush { get; }
		
	}
}
