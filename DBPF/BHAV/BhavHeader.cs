/*
 * This source code is derived from the SimPE project - https://sourceforge.net/projects/simpe/
 */

/*
 * Decompiled with: JetBrains decompiler
 * Assembly: pjse.coder.plugin.dll
 * Type: SimPe.PackedFiles.Wrapper.BhavHeader
 */

using BhavFinder.DBPF.IO;

namespace BhavFinder.DBPF.Bhav
{
    public class BhavHeader
    {
        private ushort format = 32775;
        private ushort count;
        private byte type;
        private byte argc;
        private byte locals;
        private byte headerflag;
        private uint treeversion;
        private byte cacheflags;

        public ushort Format
        {
            get => this.format;
        }

        public ushort InstructionCount
        {
            get => this.count;
        }

        public void Unserialize(IoBuffer reader)
        {
            this.format = reader.ReadUInt16();
            this.count = reader.ReadUInt16();
            this.type = reader.ReadByte();
            this.argc = reader.ReadByte();
            this.locals = reader.ReadByte();
            this.headerflag = reader.ReadByte();
            this.treeversion = reader.ReadUInt32();
            if (this.format > (ushort)32776)
                this.cacheflags = reader.ReadByte();
            else
                this.cacheflags = (byte)0;
        }
    }
}
