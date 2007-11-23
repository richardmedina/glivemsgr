using System;
using Gtk;
using RickiLib.Widgets;

namespace GLiveMsgr.Gui
{
	
	
	public class RitchTextView : Gtk.TextView
	{
	
		private RitchAnchorCollection anchors;
		private TextTag tag;
		
		public RitchTextView () : base (new RitchTextBuffer ())
		{
			anchors = new RitchAnchorCollection ();
			tag = new TextTag ("myctag");
			tag.Invisible = true;
			
			this.Buffer.TagTable.Add (tag);
			
			this.WrapMode = WrapMode.Char;
			
			//this.Buffer.Changed += Buffer_Changed;
			this.Buffer.InsertText += Buffer_InsertText;
			this.Buffer.DeleteRange += Buffer_DeleteRange;
			
			//SendingTest ();
		}
		
		public string GetText ()
		{
			Gtk.TextIter iter = this.Buffer.StartIter;
			
			string text = string.Empty;
			
			do {
				Gtk.TextChildAnchor anchor = iter.ChildAnchor;
				if (anchor != null) {
					foreach (IRitchAnchor an in anchors) {							
						if (an.Type == RitchAnchorType.Emoticon) {
							RitchAnchorEmoticon _anchor = 
								(RitchAnchorEmoticon) an;

							if (anchor == _anchor.Anchor) {
								text += _anchor.Emoticon.Trigger;
								break;
							}
						}
					}
				}else
					text += iter.Char;
			} while (iter.ForwardChar ());
			
			return text;
		}
		
		public new void InsertAtCursor (string text)
		{
			Gtk.TextIter iter = this.Buffer.GetIterAtMark (this.Buffer.InsertMark);
			
			this.Buffer.Insert (
				ref iter,
				text);
		}
		
		protected override bool OnButtonPressEvent (Gdk.EventButton args)
		{			
			//if (args.Type == Gdk.EventType.TwoButtonPress)
			//	this.Buffer.Replace ();
			
			return base.OnButtonPressEvent (args);
		}
		
		/*
		private ProgressBar bar;
		
		private void SendingTest ()
		{
			TextIter iter = this.Buffer.StartIter;
			
			Gtk.TextChildAnchor anchor = this.Buffer.CreateChildAnchor (
				ref iter);
			
			VBox vbox = new VBox (false, 0);
			vbox.BorderWidth = 10;
			bar = new ProgressBar ();
			bar.Fraction = 0.35d;
			bar.BarStyle = ProgressBarStyle.Continuous;
			vbox.PackStart (Factory.Label ("<b>image.jpg</b>"),
				false, false, 0);
				
			HBox hbox = new HBox ();
			
			Gtk.Button button = new Button ();
			button.Relief = ReliefStyle.None;
			button.Child = new Image (Stock.Cancel, IconSize.Menu);
			button.Clicked += delegate {
				canceled = !canceled;
				button.Sensitive = false;
				bar.Sensitive = false;
			};
			
			hbox.PackStart (button, false, false, 0);
			hbox.PackStart (bar);
			
			vbox.PackStart (hbox, false, false, 0);
			vbox.PackStart (new Label ("Receiving file..."), 
				false, false, 0);
			
			Viewport viewport = new Viewport ();
			
			viewport.Add (vbox);
			//viewport.ModifyBg (
			//	StateType.Normal,
			//	new Gdk.Color (0xAA, 0xAA, 0xAA));
			
			this.AddChildAtAnchor (viewport, anchor);
			
			GLib.Timeout.Add (100, bar_update);
		}
		
		bool canceled = false;
		
		private bool bar_update ()
		{
			if (canceled) {
				bar.Text = "Canceled";
				return true;
			}
			if (bar.Fraction == 1.0d)
				//bar.Fraction = 0;
				return false;

			if (bar.Fraction < 0.99)
				bar.Fraction += 0.001d;
			else bar.Fraction = 1.0d;
						
			 
			
			bar.Text = string.Format ("Completed {0}%..", 
				(bar.Fraction * 100).ToString ("0.0"));
			return true;
		}
		*/
		protected override void OnPopulatePopup (Menu menu)
		{
			base.OnPopulatePopup (menu);
			menu.HideAll ();
			
			Gtk.ImageMenuItem item = new ImageMenuItem (Stock.Open, null);
			item.Show ();

			menu.Add (item);
			
			item = new ImageMenuItem (
				Stock.Save,
				null);
			
			item.Show ();
			
			menu.ModifyBg (StateType.Normal, 
				new Gdk.Color (255, 255, 255));
			
			menu.Append (item);
		}
		
		private void showEmoticon (Emoticon emoticon)
		{	
			int index = 0;
			int last_index = 0;
			
			do {
				string buffer = this.Buffer.GetSlice (
					this.Buffer.StartIter,
					this.Buffer.EndIter,
					false);
				index = buffer.IndexOf (emoticon.Trigger, 0);
				
				if (index > -1) {
					last_index = index + emoticon.Trigger.Length;
				
					Gtk.TextIter start = getIterAtOffset (index);
					
					start.BackwardChar ();
					
					Gtk.TextIter end = getIterAtOffset (last_index);
										
					Gtk.TextMark mark = this.Buffer.CreateMark (
						"mark_anchor",
						end, 
						true);
					
					//string text_to_remove = this.Buffer.GetText (
					//	start, 
					//	end, 
					//	true);
					
					//Debug.WriteLine ("Attemping to delete '{0}' with {1} chars",
					//	text_to_remove, text_to_remove.Length);
					
					this.Buffer.Delete (ref start, ref end);
					
					start = this.Buffer.GetIterAtMark (mark);
				
					RitchAnchorEmoticon rae = new RitchAnchorEmoticon (emoticon);
				
					AddAnchor (rae, mark);
					this.Buffer.DeleteMark (mark);
				}
			}while (index > -1);

		}
		
		private void Buffer_InsertText (object sender, Gtk.InsertTextArgs args)
		{
			EmoticonManager emoticons = new EmoticonManager ();
			emoticons.CloseSession ();
			foreach (Emoticon emoticon in emoticons) 
				this.showEmoticon (emoticon);
		}
		
		private void Buffer_DeleteRange (object sender, DeleteRangeArgs args)
		{
			bool exit = true;
			do {
				exit = true;
				IRitchAnchor an = null;
				
				foreach (IRitchAnchor anchor in anchors) {
					if (anchor.Anchor.Deleted) {
						an = anchor;
						exit = false;
						break;
					}
				}
				
				if (!exit) {
					//Debug.WriteLine ("Removing Anchor");
					anchors.Remove (an);
				}
			} while (!exit);
		}
		
		private Gtk.TextIter getIterAtOffset (int index)
		{
			TextIter iter = this.Buffer.StartIter;
			int i = 0;
			bool is_anchor = false;
			do {
				if (iter.ChildAnchor != null) {
					//Debug.WriteLine ("Anchor detected at {0}", 
					//	iter.Offset);
					is_anchor = true;
				}
				if (iter.IsEnd) {
					//Debug.WriteLine ("End detected at {0}",
					//	iter.Offset);
					break;
				}
				
				iter.ForwardChar ();
				
				if (is_anchor)
					continue;
			} while (i++ < index);
			
			return iter;
		}
		
		private void AddAnchor (IRitchAnchor anchor, Gtk.TextMark mark)
		{
			//Debug.WriteLine ("Adding anchor");
			Gtk.TextIter iter = this.Buffer.GetIterAtMark (mark);
			
			//Debug.WriteLine ("Event throwns");
			Gtk.TextChildAnchor tca = this.Buffer.CreateChildAnchor (
				ref iter);
			//Debug.WriteLine ("End Event throwns");
			anchor.Anchor = tca;
			anchor.Mark = mark;
			
			this.AddChildAtAnchor(anchor.Widget, anchor.Anchor);
			
			anchors.Add (anchor);
		}
		
		
		public new RitchTextBuffer Buffer {
			get { return (RitchTextBuffer) base.Buffer; }
		}
	}
}
