
using System;
using System.Diagnostics;
using RickiLib.Widgets;
using Gtk;

namespace GLiveMsgr.Gui
{
	
	
	public class LogonFooterWidget : Gtk.VBox
	{
		
		private LinkWidget [] links;
		private string [,] labels = {
			//"You forgot your password?",
			//"State of the service",
			//"Gets a Windows Live ID"
			{"Ricki's Blog", "http://ricki.dana-ide.org"},
			{"Project Home", "http://glivemsgr.dana-ide.org"},
			{"Mail me", "mailto:ricki@dana-ide.org"}
		};
		
		private string [] link_location = {
			"http://ricki.dana-ide.org",
			"http://glivemsgr.dana-ide.org",
			"mailto:ricki@dana-ide.org",
		};
		
		public LogonFooterWidget () : base (false, 3)
		{
			links = new LinkWidget [labels.Length];
			
			for (int i = 0; i < labels.GetLength (0); i ++) {
				links [i] = new LinkWidget (labels [i, 0]);
				links [i].Location = labels [i, 1];
				links [i].ModifyBg (StateType.Normal,
					Theme.GdkColorFromCairo (Theme.BaseColor));
				links [i].ModifyText (StateType.Normal,
					Theme.GdkColorFromCairo (Theme.BgColor));
				links [i].ModifyFg (StateType.Normal,
					Theme.GdkColorFromCairo (Theme.BgColor));
				
				links [i].Clicked += link_Clicked;
				
				HBox hbox = new HBox (false, 0);
				hbox.PackStart (links [i], true, false, 0);
				base.PackStart (hbox);
			}
		}
		
		private void link_Clicked (object sender, EventArgs args)
		{
			LinkWidget link = sender as LinkWidget;
			lauchBrowser (link.Location);
			//for (int i = 0; i < links.Length; i ++)
			//	if (link == links [i]) {
			//		lauchBrowser (link_location [i]);
			//	}
		}
		
		private void lauchBrowser (string location)
		{
			Process process = new Process ();
			process.StartInfo.FileName = GLiveMsgr.Gui.Settings.Instance.Browser;
			process.StartInfo.Arguments = location;
			process.Start ();
		}
	}
}
