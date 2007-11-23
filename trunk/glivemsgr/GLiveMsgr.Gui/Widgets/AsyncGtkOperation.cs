
using System;
using System.Threading;
using Gtk;

namespace GLiveMsgr.Gui
{
	public delegate bool AsyncGtkOperationHandler ();
	
	public class AsyncGtkOperation
	{
		private Thread thread;
		private AsyncGtkOperationHandler handler;
		private GtkOperationTerminatedArgs terminatedArgs;
		
		public event GtkOperationTerminatedHandler Terminated;
		
		public AsyncGtkOperation (AsyncGtkOperationHandler handler)
		{
			Terminated = onTerminated;
			this.handler = handler;
			thread = new Thread (callback);
		}
		
		public void Start ()
		{
			thread.Start ();
		}
		
		protected virtual void OnTerminated ()
		{
		}
		
		private void onTerminated (object sender, 
			GtkOperationTerminatedArgs args)
		{
			this.OnTerminated ();
		}
		
		private void callback ()
		{
			terminatedArgs = new
				GtkOperationTerminatedArgs (handler ());
				
				
			ThreadNotify notify = new ThreadNotify (notify_handler);
			notify.WakeupMain ();
		}
		
		private void notify_handler ()
		{
			Terminated (this, terminatedArgs);
		}
	}
}
