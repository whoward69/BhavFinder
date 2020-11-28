/*
 * This source code is derived from the SimPE project - https://sourceforge.net/projects/simpe/
 */

/*
 * {SimPE Source}/fullsimpe/SimPE Clst/StrWrapper.cs
*/

using BhavFinder.DBPF.IO;
using System.Collections;

namespace BhavFinder.DBPF.STR
{
	public class Str
	{
		// See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
		public const uint TYPE = 0x53545223;

		private uint group;
		byte[] filename;

		SimPe.Data.MetaData.FormatCode format;

		Hashtable lines;

		int limit = 0;

		public Str(uint group, IoBuffer reader)
        {
			this.group = group;

			Unserialize(reader);
		}

		/// <summary>
		/// Gets or Sets the list of languages in the file
		/// </summary>
		/// <remarks>Adds empty lists when setting for missing languages</remarks>
		public StrLanguageList Languages
		{
			get
			{
				StrLanguageList lngs = new StrLanguageList();
				foreach (byte k in lines.Keys) lngs.Add(k);
				lngs.Sort();

				return lngs;
			}
		}

		/// <summary>
		/// Returns all Language-specific Strings
		/// </summary>
		/// <param name="l">the Language</param>
		/// <returns>List of Strings</returns>
		public StrItemList LanguageItems(StrLanguage l)
		{
			if (l == null) return new StrItemList();
			return LanguageItems((SimPe.Data.MetaData.Languages)l.Id);
		}

		/// <summary>
		/// Returns all Language-specific Strings
		/// </summary>
		/// <param name="l">the Language</param>
		/// <returns>List of Strings</returns>
		public StrItemList LanguageItems(SimPe.Data.MetaData.Languages l)
		{

			StrItemList items = (StrItemList)lines[(byte)l];
			if (items == null) items = new StrItemList();

			return items;
		}

		protected void Unserialize(IoBuffer reader)
		{
			lines = new Hashtable();
			if (reader.Length <= 0x40) return;

			byte[] fi = reader.ReadBytes(0x40);

			SimPe.Data.MetaData.FormatCode fo = (SimPe.Data.MetaData.FormatCode)reader.ReadUInt16();
			if (fo != SimPe.Data.MetaData.FormatCode.normal) return;

			ushort count = reader.ReadUInt16();

			filename = fi;
			format = fo;
			lines = new Hashtable();

			if ((limit != 0) && (count > limit)) count = (ushort)limit; // limit number of StrItem entries loaded
			for (int i = 0; i < count; i++) StrToken.Unserialize(reader, lines);
		}
	}
}
