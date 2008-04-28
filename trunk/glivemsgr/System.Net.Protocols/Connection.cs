using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace System.Net.Protocols
{
	
	
	public class Connection : System.Net.Sockets.TcpClient
	{
	
		private string _hostname;
		private int _port;
		private StreamWriter _writer;
		private StreamReader _reader;
				
		private event DataArrivedHandler _dataArrived;
		private event EventHandler _disconnected;
		
		
		private Thread _thread;
		private bool _isAsynchronousReading;
		
		public Connection (string hostname, int port)
		{
			this._hostname = hostname;
			this._port = port;
			this._isAsynchronousReading = false;
			
			_dataArrived = onDataArrived;
			_disconnected = onDisconnected;
						
			_thread = new Thread (thread_callback);
			_thread.IsBackground = false;
		}
		
		public void Open ()
		{
			base.Connect (_hostname, _port);
			
			_writer = new StreamWriter (base.GetStream ());
			_reader = new StreamReader (base.GetStream ());
		}
		
		public new void Close ()
		{
			base.Close ();
			//OnDisconnected ();
		}
		
		public void StartAsynchronousReading ()
		{
			_thread.Start ();
			this._isAsynchronousReading = true;
		}
		
		public void Send (string format, params object [] objs)
		{
			_writer.WriteLine (format, objs);
			_writer.Flush ();
		}
		
		public void RawSend (string format, params object [] objs)
		{
			_writer.Write (format, objs);
			_writer.Flush ();
		}
		
		public string Read ()
		{
			string text = null;
			bool excep = false;
			
			try {
				text = _reader.ReadLine ();
			} catch (Exception e1) {
				Debug.WriteLine (e1.ToString ());
				excep = true;
			}
			
			if (excep) {
				text = string.Empty;
				Debug.WriteLine ("Throw Disconnected");
				if (IsAsynchronousReading) {
					try {
						_thread.Abort ();
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
				char c = (char) _reader.Read (); 
				str += c.ToString ();
			}
			
			return str;
		}
				
		protected virtual void OnDataArrived (string data)
		{
			_dataArrived (this, new DataArrivedArgs (data));
		}
		
		protected virtual void OnDisconnected ()
		{
			_disconnected (this, EventArgs.Empty);
		}
		
		private void onDataArrived (object sender, DataArrivedArgs args)
		{
		}
		
		private void onDisconnected (object sender, EventArgs args)
		{
		}
		
		private void thread_callback ()
		{
			while (select ())
				Thread.Sleep (100);
			
			OnDisconnected ();
		}
		
		private bool select ()
		{
//			Debug.WriteLine ("waiting select");
			if (this.Active) {
				if (Client.Poll (1, SelectMode.SelectError))
					return false;
				if (Client.Poll (1, SelectMode.SelectRead)) {
					string data = Read ();
					if (data.Length > 0)
						OnDataArrived (data);
				}
			} else 
				return false;
			
			return true;
		}
		
		public string Hostname {
			get { return _hostname; }
		}
		
		public int Port {
			get { return _port; }
		}
		
		public bool IsAsynchronousReading {
			get { return _isAsynchronousReading; }
		}
		
		public event DataArrivedHandler DataArrived {
			add { _dataArrived += value; }
			remove { _dataArrived -= value; }
		}
		
		public event EventHandler Disconnected {
			add { _disconnected += value; }
			remove { _disconnected -= value; }
		}
	}
}
