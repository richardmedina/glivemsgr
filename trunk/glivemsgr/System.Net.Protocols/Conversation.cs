
using System;

namespace System.Net.Protocols
{
	
	
	public class Conversation
	{
		
		private BuddyCollection buddies;
		
		private event EventHandler started;
		public event DataReceivedHandler DataReceived;
		private event DataEventHandler dataGet;
		private event DataEventHandler dataSent;
		
		public event TypingHandler Typing;
		public event EventHandler Closed;
		
		private bool connected;
		
		public Conversation ()
		{
			Closed = onClosed;
			DataReceived = onDataReceived;
			
			dataGet = onDataGet;
			dataSent = onDataSent;
			
			Typing = onTyping;
			buddies = new BuddyCollection ();
			connected = false;
		}
		
		public virtual void SendText (string text)
		{
		}
		
		protected virtual void OnDataReceived (string data)
		{
		}
		
		protected virtual void OnClosed ()
		{
			Closed (this, EventArgs.Empty);
		}
		
		protected virtual void OnTyping (Buddy buddy)
		{
		}

		
		public void Close ()
		{
			OnClosed ();
		}
		
		protected void SendClosed ()
		{
			Closed (this, EventArgs.Empty);
		}
		
		protected void SendDataReceived (string data)
		{
			DataReceived (this,
				new DataReceivedArgs (data));
		}
		
		protected void SendTyping (Buddy buddy)
		{
			Typing (this, new TypingArgs (buddy));
		}
		
		protected void SendDataSent (string data)
		{
			OnDataSent (data);
		}
		
		protected void SendDataGet (Buddy buddy, string data)
		{
			OnDataGet (buddy, data);
		}
		
		protected void SendDataGet (string data)
		{
			OnDataGet (null, data);
		}
		
		protected virtual void OnDataGet (Buddy buddy, string data)
		{
			dataGet (this, new DataEventArgs (buddy, data));
		}
		
		protected virtual void OnDataSent (string data)
		{
			dataSent (this, new DataEventArgs (data));
		}
				
		private void onDataReceived (object sender, 
			DataReceivedArgs args)
		{
			OnDataReceived (args.Data);
		}
		
		protected virtual void OnStarted ()
		{
			started (this, EventArgs.Empty);
		}
		
		private void onStarted (object sender, EventArgs args)
		{
		}
		
		private void onDataSent (object sender, DataEventArgs args)
		{
		}
		
		private void onDataGet (object sender, DataEventArgs args)
		{
		}
		
		private void onClosed (object sender, EventArgs args)
		{
		}
		
		private void onTyping (object sender,
			TypingArgs args)
		{
			OnTyping (args.Buddy);
		}
		
		protected BuddyCollection Buddies {
			get { return buddies; }
		}
		
		public event EventHandler Started {
			add { started += value; }
			remove { started -= value; }
		}
		
		public event DataEventHandler DataSent {
			add { dataSent += value; }
			remove { dataSent += value; }
		}
		
		public event DataEventHandler DataGet {
			add { dataGet += value; }
			remove { dataGet += value; }
		}
		
		public bool Connected {
			get { return connected; }
			set { connected = value; }
		}
	}
}
