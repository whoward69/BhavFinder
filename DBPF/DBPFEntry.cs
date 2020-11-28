/*
 * This source code is derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 */
 
/*
 * This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
 * If a copy of the MPL was not distributed with this file, You can obtain one at
 * http://mozilla.org/MPL/2.0/. 
 */

namespace BhavFinder.DBPF
{
    /// <summary>
    /// Represents an entry in a DBPF archive.
    /// </summary>
    public class DBPFEntry
    {
        //A 4-byte integer describing what type of file is held
        public uint TypeID;

        //A 4-byte integer identifying what resource group the file belongs to
        public uint GroupID;

        //A 4-byte ID assigned to the file which, together with the Type ID and the second instance ID (if applicable), is assumed to be unique all throughout the game
        public uint InstanceID;
        public uint InstanceID2;

        //A 4-byte unsigned integer specifying the offset to the entry's data from the beginning of the archive
        public uint FileOffset;

        //A 4-byte unsigned integer specifying the size of the entry's data
        public uint FileSize;

        public DBPFFile file;

        // public bool global = true;

        public uint uncompressedSize = 0;
    }
}
