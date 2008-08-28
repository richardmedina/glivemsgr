using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace System.Net.Protocols.Msnp.Core
{
	
	
	public class Connection : System.Net.Sockets.TcpClient, IConnection
	{
		private string _hostname;
		private int _port;
		private StreamWriter _writer;
		private StreamReader _reader;
				
		private event EventHandler _connected;
		private event DataArrivedHandler _dataArrived;
		private event EventHandler _disconnected;
		
		
		private Thread _thread;
		private bool _isAsynchronousReading;
		
		private bool _allowReconnect;
		
		public Connection (string hostname, int port)
		{
			_hostname = hostname;
			_port = port;
			_isAsynchronousReading = false;
			_allowReconnect = false;
			
			_connected = onConnected;
			_dataArrived = onDataArrived;
			_disconnected = onDisconnected;
						
			_thread = new Thread (thread_callback);
			_thread.IsBackground = false;
		}
		
		// FIXME: DNS Resolving Errors 
		public void Open ()
		{
			bool success = true;
			int times = 1;
			
			
			
			do {
				success = true;
				Console.WriteLine ("Attemping to connect (#{2}) to: {0}:{1}", 
						_hostname, _port, times ++);
				try {
					// For some reason if does not parse the host as IPAddress,
					// the BeginConnect overload with hostname as string
					// fails.
					IPHostEntry hostentry = Dns.Resolve (_hostname);
					
					BeginConnect (hostentry.AddressList [0], _port, endConnect, null);
				} catch (Exception exception) {
					if (!AllowReconnect)
						throw exception;
				}
			} while (!success);
		}
		
		public new void Close ()
		{
			//lock (this) {
			//	this.Active = false;
			//}
			//Active = false;
			base.Close ();
		}
		
		public void StartAsynchronousReading ()
		{
			_isAsynchronousReading = true;
			_thread.IsBackground = true;
			_thread.Start ();
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
			} catch {
				excep = true;
			}
			
			if (excep) {
				text = string.Empty;
				if (IsAsynchronousReading) {
					try {
						_thread.Abort ();
					} catch {
						Close ();
						
						OnDisconnected ();
						//Disconnected (this, EventArgs.Empty);
						return string.Empty;
					}
				}
			}
			
			if (text == null)
				text = string.Empty;
			
			if (text.Length > 0)
				OnDataArrived (text);
			
			//Debug.WriteLine (text);

			return text;
		}
		// TODO: must read 'length' characters
		public string Read (int length)
		{
			string str = string.Empty;
			
			for (int i = 0; i < length; i ++) {
				char c = (char) _reader.Read ();
				//console.WriteLine ("reading: {0}",c);
				str += c.ToString ();
			}
			
			return str;
		}
		
		protected virtual void OnConnected ()
		{
			onConnected (this, EventArgs.Empty);
		}
				
		protected virtual void OnDataArrived (string data)
		{
			_dataArrived (this, new DataArrivedArgs (data));
		}
		
		protected virtual void OnDisconnected ()
		{
			_disconnected (this, EventArgs.Empty);
		}
		
		private void endConnect (IAsyncResult iar)
		{
			Console.WriteLine ("endConnect Start..");
			try {
				if (iar.IsCompleted) {
					_writer = new StreamWriter (base.GetStream ());
					_reader = new StreamReader (base.GetStream ());
					base.EndConnect (iar);
					OnConnected ();
				}
			} catch (Exception exc) {
				Console.WriteLine ("Exception Getted : {0}", exc.Message);
			}
			Console.WriteLine ("endConnect End..");
		}
		
		private void onConnected (object sender, EventArgs args)
		{
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
			//lock (this) {
			
			if (this.Active) {
				if (Client.Poll (1, SelectMode.SelectError))
					return false;
				
				if (Client.Poll (1, SelectMode.SelectRead))
					Read ();
			} else 
				return false;
			//}
			return true;
		}
		
		public string Hostname {
			get { return _hostname; }
			protected set { _hostname = value; }
		}
		
		public int Port {
			get { return _port; }
			protected set { _port=value; }
		}
		
		public bool IsAsynchronousReading {
			get { return _isAsynchronousReading; }
		}
		
		public new event EventHandler Connected {
			add { _connected += value; }
			remove { _connected -= value; }
		}
		
		public event DataArrivedHandler DataArrived {
			add { _dataArrived += value; }
			remove { _dataArrived -= value; }
		}
		
		public event EventHandler Disconnected {
			add { _disconnected += value; }
			remove { _disconnected -= value; }
		}
		
		public new bool Active {
			get { return base.Connected; }
		}
		
		public bool AllowReconnect {
			get { return _allowReconnect; }
			set { _allowReconnect = value; }
		}
	}
}