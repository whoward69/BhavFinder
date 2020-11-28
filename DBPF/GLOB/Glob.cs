/*
 * This source code is derived from the SimPE project - https://sourceforge.net/projects/simpe/
 */

/*
 * {SimPE Source}/fullsimpe/SimPE FileHandler/GlobWrapper.cs
*/

using BhavFinder.DBPF.IO;

namespace BhavFinder.DBPF.GLOB
{
	public class Glob
	{
		// See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
		public const uint TYPE = 0x474C4F42;

		private uint group;
		byte[] filename;

		public string FileName
		{
			get { return SimPe.Helper.ToString(filename); }
		}

		byte[] semiglobal;

		public string SemiGlobalName
		{
			get { return SimPe.Helper.ToString(semiglobal); }
		}

		public uint SemiGlobalGroup
		{
			get
			{
				uint grp = SimPe.Hashes.GroupHash(SemiGlobalName);
				return grp;
			}
		}

		public Glob(uint group, IoBuffer reader)
		{
			this.group = group;

			semiglobal = new byte[0];
			filename = new byte[64];

			Unserialize(reader);
		}

		/// <summary>
		/// Unserializes a BinaryStream into the Attributes of this Instance
		/// </summary>
		/// <param name="reader">The Stream that contains the FileData</param>
		protected void Unserialize(IoBuffer reader)
		{
			filename = reader.ReadBytes(64);
			byte len = reader.ReadByte();
			semiglobal = reader.ReadBytes(len);
		}
	}
}
