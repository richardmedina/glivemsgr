
using System;
using Gtk;
using RickiLib.Widgets;

using System.Net.Protocols;
using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class ConversationEntryWidget : Gtk.Viewport
	{
		private Gtk.HBox hbox;
		private RitchTextView view;
		private Gtk.VBox vbox;
		private Gtk.EventBox eb;
		
		private Gtk.Button buttonSend;
		private Gtk.Button buttonSearch;
		
		private CustomScrolledWindow scrolled;
		
		private MsnpConversation conversation;

		public ConversationEntryWidget (MsnpConversation conv)
		{
			this.conversation = conv;
			
			base.ShadowType = ShadowType.None;
			view = new RitchTextView ();
			view.KeyPressEvent += view_KeyPressEvent;

			buttonSend = new Button ("_Send");
			buttonSend.Clicked += buttonSend_Clicked;
			buttonSearch = new Button ("S_earch");
			
			hbox = new HBox (false, 0);
			
			vbox = new VBox (false, 0);
			vbox.PackEnd (buttonSearch, false, false, 0);
			vbox.PackEnd (buttonSend, false, false, 0);
			
			eb = new EventBox ();
			eb.Add (vbox);
			
			//White
			eb.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BaseColor));
			
			scrolled = new CustomScrolledWindow ();
			scrolled.ShadowType = ShadowType.None;
			scrolled.Add (view);
			
			//ConversationBackground
			base.ModifyBg (StateType.Normal,
				Theme.GdkColorFromCairo (Theme.BgColor));

			hbox.PackStart (scrolled);
			hbox.PackEnd (eb, false, false, 0);
			base.Add (hbox);
		}
		
		protected override void OnShown ()
		{
			base.OnShown ();
			this.view.GrabFocus ();
		}

		
		[GLib.ConnectBefore]
		private void view_KeyPressEvent (object sender, KeyPressEventArgs args)
		{
			bool control_pressed = (((int) args.Event.State) & ((int) Gdk.ModifierType.ControlMask)) > 0;
		
			if (args.Event.Key == Gdk.Key.KP_Enter ||
				args.Event.Key == Gdk.Key.Return &&
				!control_pressed) {
				buttonSend.Click ();
				args.RetVal = true;
			}
		}
		
		private void buttonSend_Clicked (object sender, EventArgs args)
		{
			string data = view.GetText ();
			

			if (data.StartsWith ("RAW")) {
				string raw = data.Substring (4);
				Console.WriteLine ("RAW:{0}", raw);
				conversation.RawSend (raw);
				view.Buffer.Clear ();
				return;
			}
			
			conversation.SendText (data);
			view.Buffer.Clear ();
		}
		
		public RitchTextView RitchTextView {
			get {
				return view;
			}
		}
	}
}
