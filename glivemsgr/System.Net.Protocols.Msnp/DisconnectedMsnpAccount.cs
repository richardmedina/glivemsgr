
using System;
using System.Net.Protocols.Msnp;

namespace System.Net.Protocols.Msnp
{
	
	
	public class DisconnectedMsnpAccount : MsnpAccount
	{
		private string [] groups = {
			"Group1",
			"Group2",
			"Group3",
			"Group4"
		};
		
		private string [,] contacts = {
			{"ricki@dana-ide.org", "Ricardo"},
			{"ricki9@gmail.com", "ricki9 at gmail"},
			{"kristian@innovaciontecnologica.com", "Kristian Medina"}
		};
		
		public DisconnectedMsnpAccount (string username, string password) : 
			base (username, password)
		{
		}
		
		public new int Login ()
		{
			for (int i = 0; i < groups.Length; i ++)
				this.Groups.Add (
					new MsnpGroup (groups [i], i));
			
			Random rand = new Random ();
			for (int x = 0; x < 100; x ++)
			for (int i = 0; i < contacts.Length /2; i ++) {
				//Debug.WriteLine ("{0}:{1}", contacts [i, 0], contacts [i,1]);
				MsnpContact c = new MsnpContact (contacts [i, 0],
					contacts [i, 1]);
				
				c.Groups.Add (this.Groups [rand.Next (groups.Length)]);
				
				this.Buddies.Add (c);
			}
			this.Alias = "Hello dolly!";
			
			
			base.OnStarted ();
			return 0; // success
		}
		
		public new void Logout ()
		{
		}
	}
}
