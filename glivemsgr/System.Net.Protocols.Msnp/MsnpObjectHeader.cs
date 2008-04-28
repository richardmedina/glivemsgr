// MsnpObjectHeader.cs created with MonoDevelop
// User: ricki at 13:12 02/18/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpObjectHeader
	{
		// DWORD 4 bytes (32 bits). uint
		// QWord 8 bytes (64 bits). ulong
		
		// DWORD SessionID
		uint _sessionId;
		
		// DWORD BaseIdentifier
		uint _baseId;
		
		// QWORD offset pointing to current offset of data sent
		ulong _offset;
		
		// QWORD File size
		ulong _filesize;
		
		// DWORD Size of self message
		uint _chunkSize; // Dont exceed 1202  bytes
		
		// DWORD Flags
		/*
			0x00 - No flags
			0x01 - Unknown
			0x02 - Acknowledgement
			0x04 - Waiting for a reply
			0x08 - Error (Possibly binary level)
			0x10 - Unknown
			0x20 - Data for DP/CE
			0x40 - Bye ack (He who got BYE)
			0x80 - Bye ack (He who sent BYE)
			0x1000030 - Data for FT
		*/
		uint _flags;
		
		// DWORD BaseID previous message
		uint _prevBaseId;
		
		// DWORD Seventh value of previuos message (_prevBaseId from precious message)
		uint _prevPrevBaseId;
		
		// QWORD fourth value of previous message
		ulong _prevFilesize;
		
		// QWORD. data size to send in these message
		ulong _dataSizeToSend;
		
		/*
		This DWord comes AFTER the data, and therefore it’s the Footer. 
		The DWord is written in Little Endian order! This DWord contains 
		the value zero when there is no actual file data (DP’s, CE’s etc) 
		being send. It has the value 1 when you send DP’s, CE’s or other 
		data, and it has the value 3 when sending Ink.
		 
		Other values will be given on the Transfer pages if needed.
		*/
		// Is the footer
		uint _sendType;
		
		
		
		public MsnpObjectHeader () :
			this (10, 50, 10000, 50000, 1050, 2, 0, 0, 0, 1000, 3)
		{
		}
		
		public MsnpObjectHeader (uint sessinId,
			uint baseId,
			ulong offset,
			ulong filesize,
			uint chunkSize,
			uint flags,
			uint prevBaseId,
			uint prevPrevBaseId,
			uint prevFilesize,
			ulong dataSizeToSend,
			uint sendType)
		{
			BaseId= baseId;
			Offset = offset;
			Filesize = filesize;
			ChunkSize = chunkSize;
			Flags= flags;
			PrevBaseId = prevBaseId;
			PrevPrevBaseId = prevPrevBaseId;
			DataSizeToSend = dataSizeToSend;
			SendType = sendType;
			
		}
		
		public MsnpObject Create (string filename, 
			uint sessionId)
		{
			return null;
		}
		
		public byte [] ToByteArray ()
		{
			byte [] bytes = new byte [100];
			int index = 0;
			
			index = SaveToBuffer (bytes, index, SessionId);
			index = SaveToBuffer (bytes, index, BaseId);
			
			index = SaveToBuffer (bytes, index, Offset);
			index = SaveToBuffer (bytes, index, Filesize);
			
			index = SaveToBuffer (bytes, index, ChunkSize);
			index = SaveToBuffer (bytes, index, Flags);
			
			index = SaveToBuffer (bytes, index, PrevBaseId);
			index = SaveToBuffer (bytes, index, PrevPrevBaseId);
			
			//index = SaveToBuffer (bytes, index, PrevFilesize);
			index = SaveToBuffer (bytes, index, DataSizeToSend);
			// This field belong to footer data
			//index = SaveToBuffer (bytes, index, SendType);
			

			Console.WriteLine ("Index is : {0}", index);
			//foreach (byte b in bytes)
			//	Console.Write ("{0} ", b.ToString ("X2"));
			
			return bytes;
		}
		
		private int SaveToBuffer (byte [] buffer, int index, ulong val)
		{
			byte [] bytes = BitConverter.GetBytes (val);
			return SaveToBuffer (buffer, index, bytes);
		}
		
		private int SaveToBuffer (byte [] buffer, int index, uint val)
		{
			byte [] bytes = BitConverter.GetBytes (val);
			return SaveToBuffer (buffer, index, bytes);
		}
		
		private int SaveToBuffer (byte [] buffer, int index, byte [] bytes)
		{
			for (int i = 0; i < bytes.Length; i ++)
				buffer [index + i] = bytes [i];
			Console.WriteLine ("Added {0} bytes, Index = {1}",
				bytes.Length, index + bytes.Length);
			return index + bytes.Length;
		}
		
		public override string ToString ()
		{
			//return base.ToString ();
			
			return string.Format ("{0}-{1}-{2}-{3}-{4}-{5}",
				SessionId.ToString ("X2"),
				BaseId.ToString ("X2"),
				Offset.ToString ("X4"),
				Filesize.ToString ("X4"),
				ChunkSize.ToString ("X2"),
				Flags.ToString ("X2"),
				PrevBaseId.ToString ("X2"),
				PrevPrevBaseId.ToString ("X2"),
				PrevFilesize.ToString ("X2"),
				DataSizeToSend.ToString ("X4"),
				SendType.ToString ("X2")
				);
		}

		
		public uint SessionId {
			get { return _sessionId; }
			set { _sessionId = value; }
		}
		
		public uint BaseId {
			get { return _baseId; }
			set { _baseId = value; }
		}
		
		public ulong Offset {
			get { return _offset; }
			set { _offset = value; }
		}
		
		public ulong Filesize {
			get { return _filesize; }
			set { _filesize = value; }
		}
		
		public uint ChunkSize {
			get { return _chunkSize; }
			set { _chunkSize = value; }
		}
		
		public uint Flags {
			get { return _flags; }
			set { _flags = value; }
		}
		
		public uint PrevBaseId {
			get { return _prevBaseId; }
			set { _prevBaseId = value; }
		}
		
		public uint PrevPrevBaseId {
			get { return _prevPrevBaseId; }
			set { _prevPrevBaseId = value; }
		}
		
		public ulong PrevFilesize {
			get { return _prevFilesize; }
			set { _prevFilesize = value; }
		}
		
		public ulong DataSizeToSend {
			get { return _dataSizeToSend; }
			set { _dataSizeToSend = value; }
		}
		
		public uint SendType {
			get { return _sendType; }
			set { _sendType = value; }
		}
	}
}
