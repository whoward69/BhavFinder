/*
 * This source code is derived from the SimPE project - https://sourceforge.net/projects/simpe/
 */

/*
 * Decompiled with: JetBrains decompiler
 * Assembly: pjse.coder.plugin.dll
 * Type: SimPe.PackedFiles.Wrapper.Instruction
 */

using BhavFinder.DBPF.IO;
using BhavFinder.DBPF.Utils;

namespace BhavFinder.DBPF.Bhav
{
    public class Instruction
    {
        private ushort format;

        private ushort opcode;
        private ushort addr1;
        private ushort addr2;
        private byte nodeversion;
        private WrappedByteArray operands;
        private static readonly byte[] nooperands = new byte[16]
        {
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue
        };

        public ushort OpCode
        {
            get => this.opcode;
        }

        public byte NodeVersion
        {
            get => this.nodeversion;
        }

        public WrappedByteArray Operands => this.operands;

        public Instruction(IoBuffer reader, ushort format)
        {
            this.format = format;

            this.operands = new WrappedByteArray((byte[])Instruction.nooperands.Clone());

            this.Unserialize(reader);
        }

        private ushort formatSpecificSetAddr(ushort addr)
        {
            if (format >= (ushort)32775)
                return addr;
            switch (addr)
            {
                case 253:
                    return 65532;
                case 254:
                    return 65533;
                case (ushort)byte.MaxValue:
                    return 65534;
                default:
                    return addr;
            }
        }

        private void Unserialize(IoBuffer reader)
        {
            this.opcode = reader.ReadUInt16();
            if (format < (ushort)32775)
            {
                this.addr1 = this.formatSpecificSetAddr((ushort)reader.ReadByte());
                this.addr2 = this.formatSpecificSetAddr((ushort)reader.ReadByte());
            }
            else
            {
                this.addr1 = this.formatSpecificSetAddr(reader.ReadUInt16());
                this.addr2 = this.formatSpecificSetAddr(reader.ReadUInt16());
            }

            if (format < (ushort)32771)
            {
                this.nodeversion = (byte)0;
                this.operands = new WrappedByteArray(reader, 8);
            }
            else if (format < (ushort)32773)
            {
                this.nodeversion = (byte)0;
                this.operands = new WrappedByteArray(reader, 16);
            }
            else
            {
                this.nodeversion = reader.ReadByte();
                this.operands = new WrappedByteArray(reader, 16);
            }
        }
    }
}
