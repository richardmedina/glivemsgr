
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace System.Net.Protocols
{
	
	
	public class BetaConnection : System.Net.Sockets.TcpClient
	{
	
		private string hostname;
		private int port;
		private StreamWriter writer;
		private StreamReader reader;
		
		public event DataArrivedHandler DataArrived;
		public event EventHandler Disconnected;
		
		
		private Thread thread;
		private bool isAsynchronousReading;
		
		public BetaConnection (string hostname, int port)
		{
			this.hostname = hostname;
			this.port = port;
			this.isAsynchronousReading = false;
			
			this.DataArrived = onDataArrived;
			this.Disconnected = onDisconnected;
			

			thread = new Thread (thread_callback);
			thread.IsBackground = false;
		}
		
		public void Open ()
		{
			base.Connect (hostname, port);
			
			writer = new StreamWriter (base.GetStream ());
			reader = new StreamReader (base.GetStream ());
		}
		
		public void StartAsynchronousReading ()
		{
			thread.Start ();
			this.isAsynchronousReading = true;
		}
		
		public void Send (string format, params object [] objs)
		{
			writer.WriteLine (format, objs);
			writer.Flush ();
		}
		
		public void RawSend (string format, params object [] objs)
		{
			writer.Write (format, objs);
			writer.Flush ();
		}
		
		public string Read ()
		{
			
			string text = null;
			bool excep = false;
			
			try {
				text = reader.ReadLine ();
			} catch (Exception e1) {
				Debug.WriteLine (e1.ToString ());
				excep = true;
			}
			
			if (excep) {
				text = string.Empty;
				Debug.WriteLine ("Throw Disconnected");
				if (IsAsynchronousReading) {
					try {
						thread.Abort ();
					} catch (Exception e) {
						Debug.WriteLine ("Aborted: {0}", e.Message);
						Close ();
						
						OnDisconnected ();
						//Disconnected (this, EventArgs.Empty);
						return string.Empty;
					}
				}
			}
			
			if (text == null)
				text = string.Empty;
			
			//Debug.WriteLine (text);

			return text;
		}
		// TODO: must read 'length' characters
		public string Read (int length)
		{
			string str = string.Empty;
			
			for (int i = 0; i < length; i ++) {
				char c = (char) reader.Read (); 
				str += c.ToString ();
			}
			
			return str;
		}
				
		protected virtual void OnDataArrived (string data)
		{
			DataArrived (this, new DataArrivedArgs (data));
		}
		
		protected virtual void OnDisconnected ()
		{
			this.Disconnected (this, EventArgs.Empty);
		}
		
		private void onDataArrived (object sender, DataArrivedArgs args)
		{
		}
		
		private void onDisconnected (object sender, EventArgs args)
		{
			OnDisconnected ();
		}
				
		private void thread_callback ()
		{
			//while ()
			//	Thread.Sleep (100);
			//if (reader.Read
				
			//}
		}
		
		private bool select ()
		{
			
//			Debug.WriteLine ("waiting select");
			if (this.Active) {
//				rfd.Add (this.Client);
//				wfd.Add (this.Client);
//				efd.Add (this.Client);

//				Socket.Select (rfd, null, null, 1);
//			Debug.WriteLine ("Select exited");
			
//				if (rfd.Count > 0) {
//					string data = Read ();
//					if (data.Length > 0)
//						OnDataArrived (data);
//				}
			} else return false;
			
			return true;
		}
		
		public string Hostname {
			get {
				return hostname;
			}
		}
		
		public int Port {
			get {
				return port;
			}
		}
		
		public bool IsAsynchronousReading {
			get {
				return isAsynchronousReading;
			}
		}
	}
}
