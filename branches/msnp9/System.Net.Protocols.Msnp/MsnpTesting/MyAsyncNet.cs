// MyAsyncNet.cs
// 
// Copyright (C) 2008 Ricardo Medina <ricki@dana-ide.org>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Net.Sockets;
using System.Threading;

namespace MsnpTesting
{
	
	
	public class MyAsyncNet : TcpClient
	{
		
		public new event EventHandler Connected;
		
		public MyAsyncNet()
		{
			Connected = onConnected;
		}
		
		public void Open ()
		{
			Console.WriteLine ("Connecting ...");
			IAsyncResult ia = BeginConnect ("irc.undernet.org", 6667, endConnect, null);
			Thread.CurrentThread.Name = "_hilo_principal_";
			Console.WriteLine ("End Open");
			
			EndConnect (ia);
			Console.WriteLine ("MyEnds");
			//Thread.GetDomain ().
			
		}
		
		private void endConnect (IAsyncResult ia)
		{
			Thread.CurrentThread.Name = "_hilo_secundario_";
			if (ia.IsCompleted) {
				//EndConnect (ia);
				Console.WriteLine ("Connected!");
				Console.WriteLine ("Thread: {0}", Thread.CurrentThread.Name);
				Connected (this, EventArgs.Empty);
			}
		}
		
		private void onConnected (object sender, EventArgs args)
		{
		}
	}
}
