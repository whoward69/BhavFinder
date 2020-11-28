/*
 * This source code is derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 */
 
/*
 * This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at
 * http://mozilla.org/MPL/2.0/. 
 */

using System;
using System.Collections.Generic;
using System.IO;
using BhavFinder.DBPF.IO;
using BhavFinder.DBPF.Utils;

namespace BhavFinder.DBPF
{
    public class DBPFReference
    {
        public byte[] fileBytes;
        public DBPFFile file;

        public DBPFReference(byte[] bytes, DBPFFile file)
        {
            this.fileBytes = bytes;
            this.file = file;
        }
    }

    /// <summary>
    /// The database-packed file (DBPF) is a format used to store data for pretty much all Maxis games after The Sims, 
    /// including The Sims Online (the first appearance of this format), SimCity 4, The Sims 2, Spore, The Sims 3, and 
    /// SimCity 2013.
    /// </summary>
    public class DBPFFile : IDisposable
    {
        public string fname = "";
        public DIRFile DIR = null;
        //public uint groupID;

        public int DateCreated;
        public int DateModified;

        private uint IndexMajorVersion;
        public uint IndexMinorVersion;

        public uint NumEntries;
        private IoBuffer m_Reader;

        private List<DBPFEntry> m_EntriesList = new List<DBPFEntry>();
        private Dictionary<int, DBPFEntry> m_EntryByID = new Dictionary<int, DBPFEntry>();
        private Dictionary<int, DBPFEntry> m_EntryByFullID = new Dictionary<int, DBPFEntry>();
        private Dictionary<uint, List<DBPFEntry>> m_EntriesByType = new Dictionary<uint, List<DBPFEntry>>();
        private Dictionary<string, DBPFEntry> m_EntryByName = new Dictionary<string, DBPFEntry>();

        private IoBuffer Io;

        public DBPFFile(string file)
        {
            var stream = File.OpenRead(file);
            fname = file;
            Read(stream);
        }

        public void Read(Stream stream, bool global = false, bool isDownload = false)
        {
            // groupID = Hash.GroupHash(Path.GetFileNameWithoutExtension(fname));
            m_EntryByFullID = new Dictionary<int, DBPFEntry>();
            m_EntriesList = new List<DBPFEntry>();

            var io = IoBuffer.FromStream(stream, ByteOrder.LITTLE_ENDIAN);
            m_Reader = io;
            this.Io = io;

            var magic = io.ReadCString(4);
            if (magic != "DBPF")
            {
                throw new Exception("Not a DBPF file");
            }

            var majorVersion = io.ReadUInt32();
            var minorVersion = io.ReadUInt32();
            var version = majorVersion + (((double)minorVersion) / 10.0);

            /** Unknown, set to 0 **/
            io.Skip(12);
            if (version <= 1.2)
            {
                this.DateCreated = io.ReadInt32();
                this.DateModified = io.ReadInt32();
            }

            if (version < 2.0)
            {
                IndexMajorVersion = io.ReadUInt32();
            }

            NumEntries = io.ReadUInt32();
            uint indexOffset = 0;
            if (version < 2.0)
            {
                indexOffset = io.ReadUInt32();
            }
            var indexSize = io.ReadUInt32();

            if (version < 2.0)
            {
                var trashEntryCount = io.ReadUInt32();
                var trashIndexOffset = io.ReadUInt32();
                var trashIndexSize = io.ReadUInt32();
                IndexMinorVersion = io.ReadUInt32();
            }
            else if (version == 2.0)
            {
                IndexMinorVersion = io.ReadUInt32();
                indexOffset = io.ReadUInt32();
                io.Skip(4);
            }

            /** Padding **/
            io.Skip(32);

            io.Seek(SeekOrigin.Begin, indexOffset);
            for (int i = 0; i < NumEntries; i++)
            {
                var entry = new DBPFEntry();
                entry.file = this;
                entry.TypeID = io.ReadUInt32();
                entry.GroupID = io.ReadUInt32();
                entry.InstanceID = io.ReadUInt32();
                if (IndexMinorVersion >= 2)
                    entry.InstanceID2 = io.ReadUInt32();
                entry.FileOffset = io.ReadUInt32();
                entry.FileSize = io.ReadUInt32();
                
                var id = Hash.TGIHash(entry.InstanceID, entry.TypeID, entry.GroupID);
                var fullID = Hash.TGIRHash(entry.InstanceID, entry.InstanceID2, entry.TypeID, entry.GroupID);

                m_EntryByID[id] = entry;
                m_EntryByFullID[fullID] = entry;

                if (!m_EntriesByType.ContainsKey(entry.TypeID))
                    m_EntriesByType.Add(entry.TypeID, new List<DBPFEntry>());
                m_EntriesByType[entry.TypeID].Add(entry);
            }
            
            var dirEntry = GetItemByFullID(Hash.TGIRHash((uint)0x286B1F03, (uint)0x00000000, (uint)0xE86B1EEF, (uint)0xE86B1EEF));
            if (dirEntry != null)
            {
                DIRFile.Read(this, dirEntry);
            }
        }

        public IoBuffer GetIoBuffer(DBPFEntry entry)
        {
            if (entry.uncompressedSize != 0)
            {
                byte[] data = GetItem(entry);

                return IoBuffer.FromStream(new MemoryStream(data), ByteOrder.LITTLE_ENDIAN);
            }

            m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);
            return m_Reader;
        }

        public byte[] GetItem(DBPFEntry entry)
        {
            m_Reader.Seek(SeekOrigin.Begin, entry.FileOffset);

            if (entry.uncompressedSize != 0)
            {
                return DIREntry.Decompress(m_Reader.ReadBytes((int)entry.FileSize), entry.uncompressedSize);
            }

            return m_Reader.ReadBytes((int)entry.FileSize);
        }

        public byte[] GetItemByFullID(int tgir)
        {
            if (m_EntryByFullID.ContainsKey(tgir))
                return GetItem(m_EntryByFullID[tgir]);
            else
                return null;
        }

        public DBPFEntry GetEntryByFullID(int tgir)
        {
            if (m_EntryByFullID.ContainsKey(tgir))
                return m_EntryByFullID[tgir];
            else
                return null;
        }

        public List<DBPFEntry> GetEntriesByType(uint Type)
        {

            var result = new List<DBPFEntry>();

            if (m_EntriesByType.ContainsKey(Type))
            {
                var entries = m_EntriesByType[Type];
                for (int i = 0; i < entries.Count; i++)
                {
                    result.Add(entries[i]);
                }
            }

            return result;
        }

        public void Dispose()
        {
            Io.Dispose();
        }
    }
}
