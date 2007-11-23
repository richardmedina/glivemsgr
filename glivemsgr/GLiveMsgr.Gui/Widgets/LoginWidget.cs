
using System;
using Gtk;
using RickiLib.Widgets;

namespace GLiveMsgr.Gui
{
	
	
	public class LoginWidget : Gtk.Table
	{
		private Gtk.Entry entryEmail;
		private Gtk.Entry entryPassword;
		
		private Gtk.Button buttonConnect;
		private Gtk.Button buttonCancel;
		
		
		public LoginWidget () : base (2, 3, false)
		{
			base.ColumnSpacing = 5;
			base.RowSpacing = 5;
			
			entryEmail = new Entry ();
			entryEmail.Text = "ricardo@innovaciontecnologica.com";
			entryPassword = new Entry ();
			entryPassword.Visibility = false;
			entryPassword.Text = "09b9085a";
			
			buttonConnect = new Button (Stock.Ok);
			buttonCancel = new Button (Stock.Cancel);
			
			Utils.TableShrinkAttach (this, Factory.Label ("Email"), 0, 0);
			Utils.TableShrinkAttach (this, Factory.Label ("Password"), 0, 1);
			
			Utils.TableFillAttach (this, entryEmail, 1, 0);
			Utils.TableFillAttach (this, entryPassword, 1, 1);
			
			HButtonBox box = new HButtonBox ();
			box.Spacing = 5;
			box.LayoutStyle = ButtonBoxStyle.End;
			
			box.Add (buttonCancel);
			box.Add (buttonConnect);
			
			base.Attach (box,// AttachOptions.Fill, AttachOptions.Fill,
				0, 2, 2, 3);
		}
		
		public Gtk.Button ButtonConnect {
			get {
				return buttonConnect;
			}
		}
		
		public Gtk.Button ButtonCancel {
			get {
				return buttonCancel;
			}
		}
		
		public string Email {
			get {
				return entryEmail.Text;
			}
			set {
				entryEmail.Text = value;
			}
		}
		
		public string Password {
			get {
				return entryPassword.Text;
			}
			set {
				entryPassword.Text = value;
			}
		}
	}
}
