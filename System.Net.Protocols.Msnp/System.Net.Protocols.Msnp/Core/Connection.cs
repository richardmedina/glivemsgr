// Connection.cs
//
// Copyright (c) 2008 Ricardo Medina <ricki@dana-ide.org>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

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
		
		private int _select_request_delay = 100;
		private int _poll_delay = 1000000;
		
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
		
		// FIXME: DNS Resolving Errors, using deprecated by missing implementation Dns methods 
		public virtual void Open ()
		{
			bool success = true;
			//int times = 1;
			//Console.WriteLine ("Openning connection");
			
			do {
				success = true;
				//Console.WriteLine ("Attemping to connect (#{2}) to: {0}:{1}", 
				//		_hostname, _port, times ++);
				try {
					// For some reason if does not parse the host as IPAddress,
					// the BeginConnect with hostname as string
					// fails.
					 //Dns.GetHostEntry (_hostname);
						//Dns.Resolve (_hostname);
						Dns.BeginResolve (_hostname, endResolve, null);
					
					//Dns.GetHostEntry
					
					
				} catch (Exception exception) {
					if (!AllowReconnect)
						throw exception;
				}
			} while (!success);
		}
		
		public new void Close ()
		{
			Client.Shutdown (SocketShutdown.Both);
			Client.Close ();
			Console.WriteLine ("Disconnecting");
			OnDisconnected ();
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
			Console.WriteLine (">>{0}", string.Format (format, objs));
		}
		
		public void RawSend (string format, params object [] objs)
		{
			_writer.Write (format, objs);
			_writer.Flush ();
			Console.WriteLine (">>{0}", string.Format (format, objs));
		}
		
		public string Read ()
		{
			string text = null;
			bool excep = false;
			
			lock (_reader) {
			
			//Console.WriteLine ("Read ()..");
			//try {
			text = _reader.ReadLine ();
		//	} catch {
		//		excep = true;
		//	}
			
			if (excep) {
				text = string.Empty;
				if (IsAsynchronousReading) {
					try {
						_thread.Abort ();
					} catch (Exception e) {
						Console.WriteLine ("Read (): {0}", e);
						
						Close ();
						return string.Empty;
					}
				}
			}
			
			if (text == null)
				text = string.Empty;
			
			if (text.Length > 0)
				OnDataArrived (text);
			}
			
			Console.WriteLine ("<<{0}", text);
			return text;
		}
		// TODO: must read 'length' characters
		public string Read (int length)
		{
			string str = string.Empty;
			
			//byte []
			
			for (int i = 0; i < length; i ++) {
				char c = (char) _reader.Read ();
				//console.WriteLine ("reading: {0}",c);
				str += c.ToString ();
			}
			
			
			Console.WriteLine ("<<{0}", str);
			return str;
		}
		
		protected virtual void OnConnected ()
		{
			_connected (this, EventArgs.Empty);
		}
				
		protected virtual void OnDataArrived (string data)
		{
			_dataArrived (this, new DataArrivedArgs (data));
		}
		
		protected virtual void OnDisconnected ()
		{
			_disconnected (this, EventArgs.Empty);
		}
		
		private void endResolve (IAsyncResult iar)
		{
			try {
				if (iar.IsCompleted) {
					IPHostEntry hostentry = Dns.EndResolve (iar);
					BeginConnect (hostentry.AddressList [0], 
						_port, endConnect, null);
				}
			} catch (Exception e) {
				Console.WriteLine (e);
			}
		}
		
		private void endConnect (IAsyncResult iar)
		{
			try {
				if (iar.IsCompleted) {
					EndConnect (iar);
					
					_writer = new StreamWriter (GetStream ());
					_reader = new StreamReader (GetStream ());
					
					OnConnected ();
				}
			} catch (Exception exc) {
				Console.WriteLine (exc);
			}
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
				Thread.Sleep (_select_request_delay);
			
			OnDisconnected ();
		}
		//FIXME. some errors in reading
		private bool select ()
		{
			if (Client.Connected) {
				if (Client.Poll (_poll_delay, SelectMode.SelectError))
					return false;
				
				if (Client.Poll (_poll_delay, SelectMode.SelectRead))
					Read ();
			} else 
				return false;
			
			return true;
		}
		
		public string Hostname {
			get { return _hostname; }
			set { _hostname = value; }
		}
		
		public int Port {
			get { return _port; }
			set { _port=value; }
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
