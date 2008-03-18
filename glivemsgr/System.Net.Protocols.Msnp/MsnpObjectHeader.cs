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
		// DWORD 4 bytes. uint
		// QWord 8 bytes. ulong
		
		uint _sessionId;
		// DWORD SessionID
		
		uint _baseId;
		// DWORD BaseIdentifier
		
		ulong _offset;
		// QWORD offset pointing to current offset of data sent
		
		ulong _filesize;
		// QWORD File size
		
		uint _size;
		// DWORD Size of self message
		
		uint _flags;
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
		
		uint _prevBaseId;
		// DWORD BaseID previous message
		
		uint _prevPrevBaseId;
		// DWORD Seventh value of previuos message
		
		ulong _prevFilesize;
		// QWORD fourth value of previous message
		
		ulong _dataSizeToSend;
		// DWORD. data size to send in these message
		
		uint _sendType;
		/*
		This DWord comes AFTER the data, and therefore it’s the Footer. 
		The DWord is written in Little Endian order! This DWord contains 
		the value zero when there is no actual file data (DP’s, CE’s etc) 
		being send. It has the value 1 when you send DP’s, CE’s or other 
		data, and it has the value 3 when sending Ink.
		 
		Other values will be given on the Transfer pages if needed.
		*/
		
		public MsnpObjectHeader () :
			this (10, 50, 10000, 50000, 1050, 2, 0, 0, 0, 1000, 3)
		{
		}
		
		public MsnpObjectHeader (uint sessinId,
			uint baseId,
			ulong offset,
			ulong filesize,
			uint size,
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
			Size = size;
			Flags= flags;
			prevBaseId = prevBaseId;
			PrevPrevBaseId = prevPrevBaseId;
			PrevFilesize = prevFilesize;
			DataSizeToSend = dataSizeToSend;
			SendType = sendType;
		}
		
		public override string ToString ()
		{
			//return base.ToString ();
			return string.Format ("{0}-{1}-{2}-{3}-{4}-{5}",
				SessionId.ToString ("X2"),
				BaseId.ToString ("X2"),
				Offset.ToString ("X4"),
				Filesize.ToString ("X4"),
				Size.ToString ("X2"),
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
		
		public uint Size {
			get { return _size; }
			set { _size = value; }
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
