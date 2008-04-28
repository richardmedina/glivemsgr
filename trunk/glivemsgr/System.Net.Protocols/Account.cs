// project created on 04/06/2007 at 23:25
using System;

namespace System.Net.Protocols
{
 
	public abstract class Account : Buddy
	{
		
		private string _password;
		private bool _logged;
		
		private event EventHandler _started;
		private event EventHandler _terminated;
		
		public Account (string username, string password) : base (username)
		{
			_started = onStarted;
			_terminated = onTerminated;
			_logged = false;
			_password = password;
		}
		
		// When success, must be return zero or else any non-zero value 
		public virtual int Login ()
		{
			return 0;
		}
		
		public virtual void ChangeAlias (string newalias)
		{
		}
		
		public virtual void Logout ()
		{
			OnTerminated ();
		}
				
		protected virtual void OnStarted ()
		{
			_logged = true;
			_started (this, EventArgs.Empty);
		}
		
		protected virtual void OnTerminated ()
		{
			_logged = false;
			_terminated (this, EventArgs.Empty);
		}
		
		private void onStarted (object sender, EventArgs args)
		{
		}
		
		private void onTerminated (object sender, EventArgs args)
		{
		}
		
		public string Password {
			get { return _password; }
			set { _password = value; }
		}
		
		public bool Logged {
			get { return _logged; }
			protected set { _logged = value; }
		}
		
		public abstract ProtocolType Protocol { get; }
		
		public event EventHandler Started {
			add { _started += value; }
			remove { _started -= value; }
		}
		
		public event EventHandler Terminated {
			add { _terminated += value; }
			remove { _terminated -= value; }
		}
	}
}