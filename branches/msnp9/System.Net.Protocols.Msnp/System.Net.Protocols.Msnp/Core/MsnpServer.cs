// MsnpServer.cs
//
// Copyright (c) 2008 [copyright holders]
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

namespace System.Net.Protocols.Msnp.Core
{
	
	
	public class MsnpServer : Connection, IMsnpCommandable
	{
		private MsnpServerType _serverType;
		
		private event MsnpCommandArrivedHandler _commandArrived;
		
		public MsnpServer (MsnpServerType servertype) : base (string.Empty, 0)
		{
			_serverType = servertype;
			
			_commandArrived = onCommandArrived;
		}
		
		protected virtual void OnCommandArrived (MsnpCommand command)
		{
			_commandArrived (this, new MsnpCommandArrivedArgs (command));
		}
		
		protected override void OnDataArrived (string data)
		{
			MsnpCommand command = MsnpCommand.Parse (ServerType, data);
			OnCommandArrived (command);
			base.OnDataArrived (data);
		}
		
		private void onCommandArrived (object sender, MsnpCommandArrivedArgs args)
		{
		}
		
		public MsnpServerType ServerType {
			get { return _serverType; }
			protected set { _serverType = value; }
		}
		
		// Signals..
		
		public event MsnpCommandArrivedHandler CommandArrived {
			add { _commandArrived += value; }
			remove { _commandArrived -= value; }
		}
	}
}
