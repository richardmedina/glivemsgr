
using System;
using System.IO;

namespace GLiveMsgr.Gui
{
	
	
	public class ConfigurationServices
	{
		
		private string path;
		private string configName;
		private string emoticonsFolder;
		
		public ConfigurationServices ()
		{
			
			configName = "GLiveMessenger";
			path = Environment.GetFolderPath (
				Environment.SpecialFolder.CommonApplicationData);
				
			path = System.IO.Path.Combine (path, configName);
			
			emoticonsFolder = System.IO.Path.Combine (
				path,
				"emoticons"
			);
		}
		
		public string Path {
			get { return path; }
		}
		
		public string EmoticonsFolder {
			get { return emoticonsFolder; }
		}
	}
}
