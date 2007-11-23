// project created on 19/02/2007 at 09:13 p
using System;
using Gtk;
using System.Net.Protocols.Msnp;


namespace GLiveMsgr.Gui
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Application.Init ();
			MainWindow window = new MainWindow ();
			window.ShowAll ();
			Application.Run ();
		}
	}
}