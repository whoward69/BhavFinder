/*
 * This source code is derived from the SimPE project - https://sourceforge.net/projects/simpe/
 */

/*
 * {SimPE Source}/fullsimpe/SimPE FileHandler/ObjdWrapper.cs
*/

using BhavFinder.DBPF.IO;
using System.Collections;
using System.IO;

namespace BhavFinder.DBPF.OBJD
{
	public class Objd
	{
		// See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
		public const uint TYPE = 0x4F424A44;

		private uint group;
		private byte[] filename;

		private ushort type;

		uint guid, proxyguid, originalguid;
		ushort ctssid;

		public Objd(uint group, IoBuffer reader)
		{
			this.group = group;

			Unserialize(reader);
		}

		public string FileName
		{
			get
			{
				return SimPe.Helper.ToString(filename);
			}
		}

		protected void Unserialize(IoBuffer reader)
		{
			attr = new Hashtable();
			filename = reader.ReadBytes(0x40);
			long pos = reader.Position;
			if (reader.Length >= 0x54)
			{
				reader.Seek(SeekOrigin.Begin, 0x52);
				type = reader.ReadUInt16();
			}
			else type = 0;

			if (reader.Length >= 0x60)
			{
				reader.Seek(SeekOrigin.Begin, 0x5C);
				guid = reader.ReadUInt32();
			}
			else guid = 0;

			if (reader.Length >= 0x7E)
			{
				reader.Seek(System.IO.SeekOrigin.Begin, 0x7A);
				proxyguid = reader.ReadUInt32();
			}
			else proxyguid = 0;

			if (reader.Length >= 0x94)
			{
				reader.Seek(System.IO.SeekOrigin.Begin, 0x92);
				ctssid = reader.ReadUInt16();
			}
			else ctssid = 0;

			if (reader.Length >= 0xD0)
			{
				reader.Seek(System.IO.SeekOrigin.Begin, 0xCC);
				originalguid = reader.ReadUInt32();
			}
			else originalguid = 0;

			reader.Seek(System.IO.SeekOrigin.Begin, pos);
		}

		Hashtable attr;
	}
}
