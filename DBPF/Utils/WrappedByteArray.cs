/*
 * This source code is derived from the SimPE project - https://sourceforge.net/projects/simpe/
 */

/*
 * Decompiled with: JetBrains decompiler
 * Assembly: pjse.coder.plugin.dll
 * Type: SimPe.PackedFiles.Wrapper.wrappedByteArray
 */

using BhavFinder.DBPF.IO;

namespace BhavFinder.DBPF.Utils
{
    public class WrappedByteArray
    {
        private byte[] array;

        public WrappedByteArray(byte[] array)
        {
            this.array = array;
        }

        public WrappedByteArray(IoBuffer reader, int size)
        {
            this.array = new byte[16];
            this.Unserialize(reader, size);
        }

        public byte this[int index]
        {
            get => this.array[index];
        }

        internal WrappedByteArray Clone() => new WrappedByteArray((byte[])this.array.Clone());

        public static implicit operator byte[](WrappedByteArray a) => (byte[])a.array.Clone();

        private void Unserialize(IoBuffer reader, int size) {
            int i = 0;

            while (i < size)
            {
                this.array[i++] = reader.ReadByte();
            }

            while (i < 16)
            {
                this.array[i++] = byte.MaxValue;
            }
        }

    }
}
