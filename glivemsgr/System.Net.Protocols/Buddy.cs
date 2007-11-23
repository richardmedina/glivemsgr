
using System;

namespace System.Net.Protocols
{
	
	
	public abstract class Buddy
	{
		private string username;
		private string alias;
		private int state; // zero ever be offline
		
		public event EventHandler aliasChanged;
		public event EventHandler stateChanged;
		
		public Buddy (string username) : 
			this (username, string.Empty)
		{
		}
		
		public Buddy (string username, string alias) : 
			this (username, alias, 0)
		{
		}
		
		public Buddy (string username, string alias, int state)
		{
			this.username = username;
			this.alias = alias;
			this.state = state;
			
			this.aliasChanged = onAliasChanged;
			this.stateChanged = onStateChanged;
		}
		
		public virtual Conversation OpenConversation ()
		{
			return null;
		}
		
		protected virtual void OnAliasChanged ()
		{
			Console.WriteLine ("Throw AliasChanged");
			aliasChanged (this, EventArgs.Empty);
		}
		
		protected virtual void OnStateChanged ()
		{
			Console.WriteLine ("Throw StateChanged");
			stateChanged (this, EventArgs.Empty);
		}
				
		private void onAliasChanged (object sender, EventArgs args)
		{
		}
		
		private void onStateChanged (object sender, EventArgs args)
		{
		}
				
		public string Username {
			get { return username; }
			protected set { username = value; }
		}
		
		// Setter will be used by server for alias change notification 
		public string Alias {
			get { return alias; }
			set {
				alias = value;
				OnAliasChanged ();
			}
		}
		
		public int State {
			get { return state; }
			set { 
				state = value;
				OnStateChanged ();
				//this.SendStateChanged ();
			}
		}
		
		public event EventHandler AliasChanged {
			add { aliasChanged += value; }
			remove { aliasChanged -= value; }
		}
		
		public event EventHandler StateChanged {
			add { stateChanged += value; }
			remove { stateChanged -= value; }
		}
	}
}
