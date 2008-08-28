using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Protocols.Msnp.Core;

namespace MsnpTesting
{
	public class Program
	{
		
		static void Main(string[] args)
		{
			//for (int i = 0; i < 10000; i ++)
			//Test ();
			
			//NetTest ();
			//Console.WriteLine ("Main Thread: {0}", Thread.CurrentThread.Name);
			
			MsnpEngine engine = new MsnpEngine (
				"ricardo@innovaciontecnologica.com", 
				"09b9085a");
			
			engine.Connect ();
			
			// This is a responsive application?
			/*
			while (true) {
				Console.WriteLine ("App Active..");
				System.Threading.Thread.Sleep (1000);
			}
			*/
			
			Console.ReadLine ();
		}

	}
}
