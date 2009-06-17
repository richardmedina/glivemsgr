// Settings.cs created with MonoDevelop
// User: ricki at 10:46 a 14/10/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
#define _GCONF

using System;
#if _GCONF
using GConf;
#endif

namespace GLiveMsgr.Gui
{
	
	
	public class Settings
	{
//		private string downloadFolder;
//		private string defaultPicture;
		
		private string browser;
		private string _appname = "GNOME Live Messenger by Ricki Medina";
		
		private static Settings instance = new Settings ();
		
		public Settings ()
		{
			browser = getDefaultBrowser ();
		}
		
		internal string GetWindowTitle (string title)
		{
			return string.Format ("{0} - {1}", title, _appname);
		}
		
		private string getDefaultBrowser ()
		{
			string default_browser = string.Empty;
#if _GCONF
			GConf.Client gconf = new Client();
			default_browser = gconf.Get ("/desktop/gnome/applications/browser/exec").ToString ();
#endif
			return default_browser;
		}
		
		public string Browser {
			get { return browser; }
		}
		
		public static Settings Instance {
			get { return instance; }
		}
	}
}
