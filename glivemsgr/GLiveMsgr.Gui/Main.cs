// project created on 19/02/2007 at 09:13 p
using System;
using Gtk;
using System.Security.Cryptography;
using System.Net.Protocols.Msnp;


namespace GLiveMsgr.Gui
{
	class MainClass
	{
	
		//private static string xmldata = "<msnobj Creator=\"karl113@hotmail.com\" Size=\"15045\"	Type=\"3\" Location=\"amsn.tmp\" Friendly=\"AAA=\" SHA1D=\"n42ExCY45hQyzyZ39AC7aZmP3j8=\" SHA1C=\"exiHARLx1BqTCBnT2KT0A6bogH0=\" />";

		public static void Main(string[] args)
		{	/*	
			Console.WriteLine (xmldata);
			
			MsnpObject obj = MsnpObject.Create ("Ricki", "/home/ricki/image.bmp");
			
			Console.WriteLine (obj);
			
			if (MsnpObject.Parse ("1", xmldata, out obj)) {
				Console.WriteLine ("ClientIdentificationNumber: {0}", obj.ClientIdNumber);
				Console.WriteLine ("Creator : {0}", obj.Creator);
				Console.WriteLine ("Size: {0}", obj.Size);
				Console.WriteLine ("Type: {0}", obj.Type);
				Console.WriteLine ("Location: {0}", obj.Location);
				Console.WriteLine ("Friendly: {0}", obj.Friendly);
				Console.WriteLine ("SHA1D: {0}", obj.SHA1D);
				Console.WriteLine ("SHA1C: {0}", obj.SHA1C);
				Console.WriteLine ("FullObject: {0}", obj);
			} else {
				Console.WriteLine ("No pude obtener informacion");
			}
			
			
			
			MsnpObjectHeader head = new MsnpObjectHeader ();
			head.SessionId = 10;
			
			Console.WriteLine ("Head is:{0}", head);
			*/
			Application.Init ();
			MainWindow window = new MainWindow ();
			window.ShowAll ();
			
			Application.Run ();
		}
	}
}