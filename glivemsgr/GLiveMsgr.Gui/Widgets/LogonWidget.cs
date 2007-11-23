
using System;
using System.Threading;
using Gtk;
using RickiLib.Widgets;

using System.Net.Protocols.Msnp;

namespace GLiveMsgr.Gui
{
	
	public class LogonWidget : Gtk.VBox
	{
		private LogonHeaderWidget logonHeader;
		private LogonFooterWidget logonFooter;		
		private LogonDataWidget logonData;
		
		private MsnpAccount account;
		
		public LogonWidget (MsnpAccount account)
		{
			this.account = account;
			
			logonHeader = new LogonHeaderWidget ();
			logonData = new LogonDataWidget (this.account);
			logonFooter = new LogonFooterWidget ();
			
			//base.ModifyBg (StateType.Normal, 
			//	Theme.GdkColorFromCairo (Theme.SelectedBgColor));
			
			PackStart (logonHeader, false, false, 20);
			PackStart (logonData, true, true, 0);
			PackEnd (logonFooter, true, false, 0);
			
			//PackStart (VBox);
			ShowAll ();
			
		}
				
		public LogonHeaderWidget LogonHeader {
			get { return logonHeader; }
		}
		
		public LogonDataWidget LogonData {
			get { return logonData; }
		}
		
		public LogonFooterWidget LogonFooter {
			get { return logonFooter; }
		}
	}
}
