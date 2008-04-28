// project created on 19/02/2007 at 09:13 p
using System;
using System.Text;
using Gtk;
using System.Security.Cryptography;
using System.Net.Protocols.Msnp;


namespace GLiveMsgr.Gui
{
	class MainClass
	{
	
		//private static string xmldata = "<msnobj Creator=\"karl113@hotmail.com\" Size=\"15045\"	Type=\"3\" Location=\"amsn.tmp\" Friendly=\"AAA=\" SHA1D=\"n42ExCY45hQyzyZ39AC7aZmP3j8=\" SHA1C=\"exiHARLx1BqTCBnT2KT0A6bogH0=\" />";

		public static void Main(string[] args)
		{	
			//MsnpP2PMessageTest ();
			//BinaryHeaderTest ();
			RunGliveMsgr ();
		}
		
		private static void RunGliveMsgr ()
		{
			Application.Init ();
			MainWindow window = new MainWindow ();
			window.ShowAll ();
			
			Application.Run ();
		}
		
		private static void BinaryHeaderTest ()
		{
			MsnpObjectHeader h = new MsnpObjectHeader ();
			byte [] bytes = h.ToByteArray ();
			
			foreach (byte b in bytes)
				Console.Write ("{0:X2} ", b);
		}
		
		private static void MsnpP2PMessageTest ()
		{
			MsnpP2PMessage msg = new MsnpP2PMessage ();
			msg.AppId = 1;
			msg.BranchUID = "{33517CE4-02FC-4428-B6F4-39927229B722}";
			msg.CallId = "{9D79AE57-1BD5-444B-B14E-3FC9BB2B5D58}";
			msg.ContentType = "application/x-msnmsgr-sessionreqbody";
			
			MsnpObject msnpobj = MsnpObject.Create (msg.Sender, 
				"/home/ricki/Desktop/wifi_spam.png");
			
			
			
			msg.Context = Utils.Base64 (
				Encoding.Default.GetBytes (msnpobj.ToString ()));
			msg.CSeq = 0;
			msg.EufGUID = "{5D3E02AB-6190-11D3-BBBB-00C04F795683}";
			msg.MaxForwards = 0;
			msg.MsnSLPVersion ="MSNSLP/1.0";
			msg.Receiver = "karl113@hotmail.com";
			msg.Sender = "ricardo@innovaciontecnologica.com";
			msg.SessionId = 0;
			
			Console.WriteLine (msg);
		}
	}
}