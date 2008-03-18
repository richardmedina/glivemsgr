// MsnpObject.cs created with MonoDevelop
// User: ricki at 12:26 02/15/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Xml;

namespace System.Net.Protocols.Msnp
{
	
	
	public class MsnpObject
	{
		private long _clientIdNumber;
		private string _creator;
		private double _size;
		private int _type;
		private string _location;
		private string _friendly;
		private string _sha1d;
		private string _sha1c;
		
		
		private MsnpObject () :
			this (
			0,
			string.Empty,
			0, 0, 
			string.Empty, 
			string.Empty, 
			string.Empty, 
			string.Empty)
		{
		}
				
		public MsnpObject (
			long clientIdNumber,
			string creator,
			double size,
			int type,
			string location,
			string friendly,
			string sha1d,
			string sha1c)
		{
			Creator = creator;
			Size = size;
			Type = type;
			Location = location;
			Friendly = friendly;
			SHA1D = sha1d;
			SHA1C = sha1c;
		}
		
		public static MsnpObject Create (string creator, string filename)
		{
			MsnpObject obj = new MsnpObject ();
			
			FileStream s = new FileStream (filename, FileMode.Open);
			byte [] bytes = new byte [s.Length];
			
			s.Read (bytes, 0, (int) s.Length);
			
			// Now we have data in memory determinated by 'bytes'
			
			// Just for testing..
			//using (FileStream fs = new FileStream (
			//	"/home/ricki/Desktop/file.txt", FileMode.Create)) {
			//	fs.Write (bytes, 0, bytes.Length);
			//}
			
			obj.Creator = creator;
			obj.Size = s.Length;
			obj.Type = 3; //DisplayPicture
			obj.Location = "test.tmp";
			obj.Friendly = "AAA=";
			
			obj._sha1d = Utils.Base64 (Utils.SHA1 (bytes));
			
			string data = String.Format("Creator{0}Size{1}Type{2}Location{3}Friendly{4}SHA1D{5}",
          			obj.Creator,
          			obj.Size, 
          			obj.Type,
          			obj.Location, 
          			obj.Friendly, 
          			obj.SHA1D);
          		
          		obj.SHA1C = Utils.Base64 (
          			Utils.SHA1 (System.Text.Encoding.Default.GetBytes (data)));
			
			return obj;
		}
		
		public static bool Parse (string idNumber, 
			string msnobj,
			out MsnpObject msnpobject)
		{
			msnpobject = null;
			MsnpObject obj = new MsnpObject ();
			
			XmlDocument doc = new XmlDocument ();
			
			try {
				doc.InnerXml = msnobj;
			} catch (XmlException exception) {
				Console.WriteLine ("Error parsing msnobj: {0}", 
					exception.Message);
				return false;
			}
			
			XmlElement element = doc ["msnobj"];
			
			if (element == null)
				return false;
			
			if (!long.TryParse (idNumber, out obj._clientIdNumber))
				return false;
			
			if (!getAttrVal ("Creator", element, ref obj._creator))
				return false;
				
			string auxstr = string.Empty;
			
			if (!getAttrVal ("Size", element, ref auxstr))
				return false;
			
			if (!double.TryParse (auxstr, out obj._size))
				return false;
			
			if (!getAttrVal ("Type", element, ref auxstr))
				return false;
			
			if (!int.TryParse (auxstr, out obj._type))
				return false;
				
			if (!getAttrVal ("Location", element, ref obj._location))
				return false;
			
			if (!getAttrVal ("Friendly", element, ref obj._friendly))
				return false;
			
			if (!getAttrVal ("SHA1D", element, ref obj._sha1d))
				return false;
			
			if (!getAttrVal ("SHA1C", element, ref obj._sha1c))
				return false;
			
			msnpobject = obj;
			return true;
		}
		
		private static bool getAttrVal (
			string attrname,
			XmlElement element, 
			ref string result)
		{
			string val = element.GetAttribute (attrname);
			
			if (val != null) {
				result = val;
				return true;
			}
			
			return false;
		}
		
		public override string ToString ()
		{
			
			return String.Format("<msnobj Creator=\"{0}\" Size=\"{1}\" Type=\"{2}\" " +
				"Location=\"{3}\" Friendly=\"{4}\" SHA1D=\"{5}\" " +
				"SHA1C=\"{6}\"/>",
				Creator,
				Size,
				Type,
				Location,
				Friendly,
				SHA1D, 
				SHA1C);
		}

		
		public long ClientIdNumber {
			get { return _clientIdNumber; }
			set { _clientIdNumber = value; }
		}
		
		public string Creator {
			get { return _creator; }
			set { _creator = value; }
		}
		
		public double Size {
			get { return _size; }
			set { _size = value; }
		}
		
		public int Type {
			get { return _type; }
			set { _type = value; }
		}
		
		public string Location {
			get { return _location; }
			set { _location = value; }
		}
		
		public string Friendly {
			get { return _friendly; }
			set { _friendly = value; }
		}
		
		public string SHA1D {
			get { return _sha1d; }
			set { _sha1d = value; }
		}
		
		public string SHA1C {
			get { return _sha1c; }
			set { _sha1c = value; }
		}
	}
}
