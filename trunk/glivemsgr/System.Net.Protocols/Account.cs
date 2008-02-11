// project created on 04/06/2007 at 23:25
using System;

namespace System.Net.Protocols
{
 
	public abstract class Account : Buddy
	{
		
		private string password;
		private bool logged;
		
		private event EventHandler started;
		private event EventHandler terminated;
		
		public Account (string username, string password) : base (username)
		{
			started = onStarted;
			terminated = onTerminated;
			logged = false;
			this.password = password;
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
			logged = true;
			started (this, EventArgs.Empty);
		}
		
		protected virtual void OnTerminated ()
		{
			logged = false;
			terminated (this, EventArgs.Empty);
		}
		
		private void onStarted (object sender, EventArgs args)
		{
		}
		
		private void onTerminated (object sender, EventArgs args)
		{
		}
		
		public string Password {
			get { return password; }
			set { password = value; }
		}
		
		public bool Logged {
			get { return logged; }
			protected set { logged = value; }
		}
		
		public abstract ProtocolType Protocol { get; }
		
		public event EventHandler Started {
			add { started += value; }
			remove { started -= value; }
		}
		
		public event EventHandler Terminated {
			add { terminated += value; }
			remove { terminated -= value; }
		}
	}
}