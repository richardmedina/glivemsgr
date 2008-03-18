
using System;
using System.Threading;
using Gtk;
using RickiLib.Widgets;

using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	
	public class LogonDataWidget : Gtk.HBox
	{
		private FixedComboBoxEntry comboEmail;
		private FixedEntry entryPassword;
		
		//private ComboMsnState comboState;
		
		private Gtk.VBox vboxAnim;
		
		private Gtk.VBox [] containers;
		private Gtk.Button buttonConnect;
		
		private Gtk.CheckButton [] checkOptions;
		
		private string [] options_labels = {
			"Remember email address",
			"Remember password",
			"Auto login"
		};
		
		
		//private Gtk.Image imageAnim;
		
		private MsnpAccount account;
		
		public LogonDataWidget (MsnpAccount account)
		{	
		
			this.account = account;
			comboEmail = new FixedComboBoxEntry (
				new string [] {
					"email1@address.com",
					"email2@address.com"
				});
			
			comboEmail.Entry.Text = string.Empty;
			
			entryPassword = new FixedEntry ();
			entryPassword.Entry.Visibility = false;
			
			entryPassword.Entry.Text = string.Empty;
			//comboState = new ComboMsnState ();
			//comboState.ModifyBase (StateType.Normal, new Gdk.Color (255, 0, 0));
			buttonConnect = new Button ("Login");
			buttonConnect.Relief = ReliefStyle.None;
			
			buttonConnect.ModifyBg (StateType.Active,
			          Theme.GdkColorFromCairo (Theme.BaseColor));
			buttonConnect.ModifyBg (StateType.Prelight,
			          Theme.GdkColorFromCairo (Theme.BgColor));
			// LogonEntryForeground
			buttonConnect.ModifyText (StateType.Active,
				Theme.GdkColorFromCairo (Theme.TextColor));
			
			buttonConnect.Clicked += buttonConnect_Clicked;
			
			VBox vbox = new VBox (false, 5);
			
			vbox.PackStart (centeredWidget (new DisplayPictureWidget ()), false, false, 10);
			
			
			vbox.PackStart (
				alignWidget (Factory.Label ("Email Address")), 
				false, 
				false, 
				0);
				
			vbox.PackStart (comboEmail, false, false, 0);
			
			vbox.PackStart (
				alignWidget (Factory.Label ("Password")), 
				false, 
				false, 
				0);
				
			vbox.PackStart (entryPassword, false, false, 0);
			
			//vbox.PackStart (comboState, false, false, 0);
			
			vboxAnim = new VBox (false, 0);
			
			createAnimChilds ();
			
			vboxAnim.PackStart (containers [0], false, false, 0);
			
			vbox.PackStart (vboxAnim, false, false, 0);
			
			vbox.PackStart (centeredWidget (buttonConnect), true, false, 0);
			
			base.PackStart (vbox, true, false, 0);
		
		}
		/*
		protected override bool OnExposeEvent (Gdk.EventExpose evnt)
		{
			
		
			
			
			Cairo.Context c = Gdk.CairoHelper.Create (evnt.Window);
			
			c.Rectangle (10, 100, 200, 100);
			c.Color = new Cairo.Color (1, 0, 0);
			c.Stroke ();
			
			((IDisposable)c.Target).Dispose ();
			((IDisposable)c).Dispose ();
			bool ret =  base.OnExposeEvent (evnt);
			return ret;
		}
		*/
		
		private void ShowChild (int i)
		{
			int j = i==0?1:0;
			
			vboxAnim.Remove (containers [i]);
			vboxAnim.PackStart (containers [j]);
		}
		
		
		
		private void createAnimChilds ()
		{
		
			containers = new VBox [2];
			containers [0] = new VBox ();
			containers [1] = new VBox ();
		
			checkOptions = new CheckButton [options_labels.Length];
			
			for (int i = 0; i < checkOptions.Length; i ++) {
				checkOptions [i] = new CheckButton (
					options_labels [i]);
				
				containers [0].PackStart (
					alignWidget (checkOptions [i]), 
					false,
					false,
					0
				);
			}
			
			Gtk.Image image = Image.LoadFromResource ("login_anim.gif");
			
			containers [1].PackStart (Factory.Label ("Connecting..."), 
				false, false, 0);
			containers [1].PackStart (image, true, false, 0);
			
			containers [0].ShowAll ();
			containers [1].ShowAll ();
			
		}
		
		private Gtk.Widget centeredWidget (Gtk.Widget widget)
		{
			HBox hbox = new HBox (false, 0);
			hbox.PackStart (widget, true, false, 0);
			
			return hbox;			
		}
		
		private Gtk.Widget alignWidget (Gtk.Widget widget)
		{
			HBox hbox = new HBox (false, 0);
			hbox.PackStart (widget, false, false, 0);
			
			return hbox;
		}
		
		private bool connecting = false;
		
		private void buttonConnect_Clicked (object sender, EventArgs args)
		{
			if (connecting) {
				ShowChild (1);
				//Cancel login
			}
			else {
				ShowChild (0);
				account.Username = this.EntryEmail.Text;
				account.Password = this.EntryPassword.Text;
				account.State = MsnpContactState.Online;
				new Thread ((ThreadStart)delegate {
					int ret = account.Login ();
					new ThreadNotify (delegate {
						if (ret != 0) {
							buttonConnect.Click ();
							MessageDialog d = new MessageDialog (
								null,
								DialogFlags.Modal,
								MessageType.Error,
								ButtonsType.Close,
								"<b>Error</b>\nError iniciando sesion.");
							d.Run ();
							d.Destroy ();
						}
					}).WakeupMain ();
				}).Start ();
			}
			
			//buttonConnect.Label = flag?"_Login":"_Cancel";
			
			connecting = !connecting;

		}
		
		public Gtk.Button ButtonConnect {
			get { return buttonConnect; }
		}
		
		public Gtk.Entry EntryEmail {
			get { return this.comboEmail.Entry; }
		}
		
		public Gtk.Entry EntryPassword {
			get { return this.entryPassword.Entry; }
		}
		
		/*public ComboMsnState ComboState {
			get { return comboState; }
		}*/
	}
}
