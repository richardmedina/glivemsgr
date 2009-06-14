// MsnpClient.cs
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
	
	
	public class MsnpClient : Connection, IMsnpCommandable
	{
		private MsnpClientType _serverType;
		
		private event MsnpCommandArrivedHandler _commandArrived;
		private event MsnpMessageHandler _message_arrived;
		
		public MsnpClient (MsnpClientType servertype) : base (string.Empty, 0)
		{
			_serverType = servertype;
			
			_commandArrived = onCommandArrived;
			_message_arrived = onMessageArrived;
		}
		
		public void SendCommand (MsnpCommand command)
		{
			Send (command.RawString);
		}
		
		public void SendMessage (MsnpMessage message)
		{
			SendCommand (message.Command);
			Send (message.Body);
		}
		
		protected virtual void OnCommandArrived (MsnpCommand command)
		{
			_commandArrived (this, new MsnpCommandArrivedArgs (command));
		}
		
		protected override void OnDataArrived (string data)
		{
			MsnpCommand command = MsnpCommand.Parse (ServerType, data);
			if (command.Type == MsnpCommandType.MSG) {
					processMSG (command);
					return;
			}
			if (command.Type == MsnpCommandType.Unknown)
				Console.WriteLine ("**UNKNOW COMMAND**:{0}", command.RawString); 
			OnCommandArrived (command);
			base.OnDataArrived (data);
		}
		
		protected virtual void OnMessageArrived (MsnpMessage message)
		{
			_message_arrived (this, new MsnpMessageArgs (message));
		}
		
		private void processMSG (MsnpCommand command)
		{
			int size;
			
			if (int.TryParse (command.Arguments [2], out size)) {
				string buffer = Read (size);
				MsnpMessage msg = new MsnpMessage (command, buffer);
				
				OnMessageArrived (msg);
			}
		}
		
		private void onCommandArrived (object sender, MsnpCommandArrivedArgs args)
		{
		}
		
		private void onMessageArrived (object sender, MsnpMessageArgs args)
		{
		}
		
		
		
		public MsnpClientType ServerType {
			get { return _serverType; }
			protected set { _serverType = value; }
		}
		
		// Signals..
		
		public event MsnpCommandArrivedHandler CommandArrived {
			add { _commandArrived += value; }
			remove { _commandArrived -= value; }
		}
		
		public event MsnpMessageHandler MessageArrived {
			add { _message_arrived += value; }
			remove { _message_arrived -= value; }
		}
	}
}
