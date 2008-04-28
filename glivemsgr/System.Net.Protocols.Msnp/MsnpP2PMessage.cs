// MsnpP2PMessage.cs created with MonoDevelop
// User: ricki at 02:18 03/22/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpP2PMessage
	{
		private string _sender = string.Empty;
		private string _receiver = string.Empty;
		private string _msnSlpVersion = string.Empty;
		private string _branchUid = string.Empty;
		
		private int _cSeq = 0;
		private string _callId = string.Empty;
		private int _maxForwards = 0;
		
		private string _contentType = string.Empty;
		private string _contentLength;
		
		// You sure that's here?
		
		private string _eufGuid;
		private int _sessionId;
		private int _appId;
		
		private string _context;
		
		private const string _disp_or_ce_guid = 
			"{A4268EEC-FEC5-49E5-95C3-F126696BDBF6}";  
		
		private const string _file_trans_guid = 
			"{5D3E02AB-6190-11D3-BBBB-00C04F795683}";
			
		private MsnpObjectHeader _header;
		
		public MsnpP2PMessage ()
		{
		}
		
		private int getContentLength ()
		{
			return 0;
		}
		
		private string getMessageBody ()
		{
			return string.Format (
				"EUF-GUID: {0}\r\n" +
				"SessionID: {1}\r\n" +
				"AppID: {2}\r\n" +
				"Context: {3}\r\n\0",
				EufGUID,
				SessionId,
				AppId,
				Context
			);
		}
		
		public override string ToString ()
		{
			string body = getMessageBody ();
			
			//string invite_header = 
		
			string invite_body = string.Format (
				"INVITE MSNMSGR:{0} Version={1}\r\n" +
				"To: <msnmsgr: {2}>\r\n" +
				"From: <msnmsgr: {3}\r\n" +
				"Via: {4}/TLP;branch={5}\r\n" +
				"CSeq: {6}\r\n" +
				"Call-ID: {7}\r\n" +
				"Max-Forwards: {8}\r\n" +
				"Content-Type: {9}\r\n" +
				"Content-Length: {10}\r\n" +
			/*	"Bridge: TCPv1\r\n" +
				"NetID: 1\r\n" +
				"Conn-Type: Direct-Connect\r\n" +
				"UPnPNat: false\r\n" +
				"ICF: false\r\n" +*/
				"\r\n" +
				"{11}\r\n\0", // message body
				Sender,
				MsnSLPVersion,
				Receiver,
				Sender,
				MsnSLPVersion,
				BranchUID,
				CSeq,
				CallId,
				MaxForwards,
				ContentType,
				body.Length,
				body);
			
			string msg = string.Format (
				"MSG 1 D __MYDATASIZE__\r\n" +
				"MIME-Version: 1.0\r\n" +
				"Content-Type: application/x-msnmsgrp2p\r\n" +
				"P2P Dest: {0}\r\n" +
				"\r\n" +
				"{1}",
				Receiver,
				invite_body
				);
			return msg.Replace ("__MYDATASIZE__", invite_body.Length.ToString ());
		}
		
		public string Sender {
			get { return _sender; }
			set { _sender = value; }
		}
		
		public string Receiver {
			get { return _receiver; }
			set { _receiver = value; }
		}
		
		public string MsnSLPVersion {
			get { return _msnSlpVersion; }
			set { _msnSlpVersion = value; }
		}
		
		public string BranchUID {
			get { return _branchUid; }
			set { _branchUid = value; }
		}
		
		public int CSeq {
			get { return _cSeq; }
			set { _cSeq = value; }
		}
		
		public string CallId {
			get { return _callId; }
			set { _callId = value; }
		}
		
		public int MaxForwards {
			get { return _maxForwards; }
			set { _maxForwards = value; }
		}
		
		public string ContentType {
			get { return _contentType; }
			set { _contentType = value; }
		}
		
		public int ContentLength {
			get { return getContentLength (); }
		}
		
		// That's here?
		
		public string EufGUID {
			get { return _eufGuid; }
			set { _eufGuid = value; }
		}
		
		public int SessionId {
			get { return _sessionId; }
			set { _sessionId = value; }
		}
		
		public int AppId {
			get { return _appId; }
			set { _appId = value; }
		}
		
		public string Context {
			get { return _context; }
			set { _context = value; }
		}
		
		public MsnpObjectHeader Header {
			get { return _header; }
			set { _header = value; }
		}
	}
}
