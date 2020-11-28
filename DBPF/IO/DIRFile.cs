/*
 * This source code is derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 */

using BhavFinder.DBPF.Utils;
using System.IO;

namespace BhavFinder.DBPF.IO
{
    public class DIRFile
    {
        public static void Read(DBPFFile package, byte[] file)
        {
            var stream = new MemoryStream(file);
            var reader = IoBuffer.FromStream(stream, ByteOrder.LITTLE_ENDIAN);

            while (stream.Position < file.Length)
            {
                var TypeID = reader.ReadUInt32();
                var GroupID = reader.ReadUInt32();
                var InstanceID = reader.ReadUInt32();
                uint  InstanceID2 = 0x00000000;
                if (package.IndexMinorVersion >= 2)
                    InstanceID2 = reader.ReadUInt32();
                var idEntry2 = Hash.TGIRHash(InstanceID, InstanceID2, TypeID, GroupID);
                package.GetEntryByFullID(idEntry2).uncompressedSize = reader.ReadUInt32();
            }

            reader.Dispose();
            stream.Dispose();
        }
    }
}