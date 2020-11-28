/*
 * This source code is derived from the SimPE project - https://sourceforge.net/projects/simpe/
 */

/*
 * Decompiled with: JetBrains decompiler
 * Assembly: pjse.coder.plugin.dll
 * Type: SimPe.PackedFiles.Wrapper.Bhav
 */

using BhavFinder.DBPF.IO;
using System.Collections.Generic;

namespace BhavFinder.DBPF.Bhav
{
    public class Bhav
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x42484156;

        private uint group;
        private byte[] filename = new byte[64];
        private BhavHeader header;

        private List<Instruction> items;

        public Bhav(uint group, IoBuffer reader) {
            this.group = group;
            this.header = new BhavHeader();

            Unserialize(reader);
        }


        public string FileName
        {
            get => SimPe.Helper.ToString(this.filename);
        }

        public uint Group
        {
            get => this.group;
        }

        public BhavHeader Header => this.header;

        public List<Instruction> Instructions => this.items;

        protected void Unserialize(IoBuffer reader)
        {
            this.filename = reader.ReadBytes(64);
            this.header.Unserialize(reader);
            this.items = new List<Instruction>();
            while (this.items.Count < (int)this.Header.InstructionCount)
                this.items.Add(new Instruction(reader, header.Format));
        }
    }
}
