// MsnpSwitchboard.cs
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
using System.Collections.Generic;
using System.Text;

namespace System.Net.Protocols.Msnp.Core
{
	
	public class MsnpSwitchboard : SBSessionCollection
	{
		private MsnpEngine _engine;
		
		private SBSessionCollection _sessions; 
		
		public MsnpSwitchboard (MsnpEngine engine)
		{
			_sessions = new SBSessionCollection ();
			_engine = engine;
			_engine.CommandArrived += engineCommandArrived;
		}
		
		public bool GetSession (string username, out MsnpSBSession session)
		{
			session = null;
			 foreach (MsnpSBSession sess in this) {
			 	if (sess.Owner == username) {
			 		session = sess;
			 		return true;
			 	}
			 }
			 return false;
		}
		
		public MsnpSBSession CreateSession (MsnpCommand command)
		{
			
			MsnpSBSession session = new MsnpSBSession (command);
			
			return session;
		}
		
		
		public SBSessionCollection Sessions {
			get { return _sessions; }
		}
		
		private void engineCommandArrived (object sender,
			MsnpCommandArrivedArgs args)
		{
//			RNG 38695310 64.4.36.44:1863 CKI 20799108.88152118 karl113@hotmail.com Ricki.

			if (args.Command.Type == MsnpCommandType.RNG) {
				Console.WriteLine ("MsnpSwitchboard. {0}", args.Command.RawString);
				MsnpSBSession session;
				// Fourth argument contains the calling username
				if (GetSession (args.Command.Arguments [4], out session))
					Activate (session);
				else {
					session = CreateSession (args.Command);
					Add (session);
				}
			}
		}
	}
}
