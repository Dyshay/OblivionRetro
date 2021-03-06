using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.IO
{
    public class BigEndianWriter : IDisposable
    {
        public const int INT_SIZE = 32;
        public const int SHORT_SIZE = 16;
        public const int SHORT_MIN_VALUE = -0x8000;
        public const int SHORT_MAX_VALUE = 0x7FFF;
        public const int USHORT_MAX_VALUE = 0xFFFF;
        public const int CHUNCK_BIT_SIZE = 7;
        public static readonly int MAX_ENCODING_LENGHT = (int)Math.Ceiling((double)INT_SIZE / CHUNCK_BIT_SIZE);
        public const int MASK_10000000 = 0x80;
        public const int MASK_01111111 = 0x7F;

        #region Properties

        private BinaryWriter m_writer;

        public Stream BaseStream
        {
            get { return m_writer.BaseStream; }
        }

        /// <summary>
        ///   Gets available bytes number in the buffer
        /// </summary>
        public long BytesAvailable
        {
            get { return m_writer.BaseStream.Length - m_writer.BaseStream.Position; }
        }

        public long Position
        {
            get { return m_writer.BaseStream.Position; }
            set
            {
                m_writer.BaseStream.Position = value;
            }
        }

        public byte[] Data
        {
            get
            {
                var pos = m_writer.BaseStream.Position;

                var data = new byte[m_writer.BaseStream.Length];
                m_writer.BaseStream.Position = 0;
                m_writer.BaseStream.Read(data, 0, (int)m_writer.BaseStream.Length);

                m_writer.BaseStream.Position = pos;

                return data;
            }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Initializes a new instance of the <see cref="BigEndianWriter"/> class.
        /// </summary>
        public BigEndianWriter()
        {
            m_writer = new BinaryWriter(new MemoryStream(), Encoding.UTF8);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BigEndianWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public BigEndianWriter(Stream stream)
        {
            m_writer = new BinaryWriter(stream, Encoding.UTF8);
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///   Reverse bytes and write them into the buffer
        /// </summary>
        private void WriteBigEndianBytes(byte[] endianBytes)
        {
            for (int i = endianBytes.Length - 1; i >= 0; i--)
            {
                m_writer.Write(endianBytes[i]);
            }
        }

        #endregion

        #region Public Methods

        public void WriteVarInt(int @int)
        {
            var value = unchecked((uint)@int);

            if (value <= MASK_01111111)
            {
                m_writer.Write((byte)value);
                return;
            }

            int i = 0;
            while (value != 0)
            {
                var b = (byte)(value & MASK_01111111);
                i++;
                value >>= CHUNCK_BIT_SIZE;
                if (value > 0)
                    b |= MASK_10000000;
                m_writer.Write(b);
            }
        }

        public void WriteVarUhInt(uint @uint)
        {
            WriteVarInt(unchecked((int)@uint));
        }

        public void WriteVarShort(int @short)
        {
            WriteVarShort((short)@short);
        }

        public void WriteVarShort(short @short)
        {
            var value = unchecked((ushort)@short);

            if (value <= MASK_01111111)
            {
                m_writer.Write((byte)value);
                return;
            }

            int i = 0;
            while (value != 0)
            {
                var b = (byte)(value & MASK_01111111);
                i++;
                value >>= CHUNCK_BIT_SIZE;
                if (value > 0)
                    b |= MASK_10000000;
                m_writer.Write(b);
            }
        }

        public void WriteVarUhShort(int @ushort)
        {
            WriteVarUhShort((ushort)@ushort);
        }

        public void WriteVarUhShort(ushort @ushort)
        {
            WriteVarShort(unchecked((short)@ushort));
        }

        public void WriteVarLong(long @long)
        {
            var value = unchecked((ulong)@long);

            if (value >> 32 == 0)
            {
                WriteVarInt((int)value);
                return;
            }

            var low = value & 0xFFFFFFFF;
            var high = value >> 32;
            for (int i = 0; i < 4; i++)
            {
                m_writer.Write((byte)(low & MASK_01111111 | MASK_10000000));
                low >>= 7;
            }
            if ((high & 0xFFFFFFF8) == 0) // only 3 first bits are non zeros
            {
                m_writer.Write((byte)(high << 4 | low));
            }
            else
            {
                m_writer.Write((byte)((high << 4 | low) & MASK_01111111 | MASK_10000000));
                high >>= 3;
                while (high >= 0x80)
                {
                    m_writer.Write((byte)(high & MASK_01111111 | MASK_10000000));
                    high >>= 7;
                }
                m_writer.Write((byte)high);
            }
        }

        public void WriteVarUhLong(ulong @ulong)
        {
            WriteVarLong(unchecked((long)@ulong));
        }

        /// <summary>
        ///   Write a Short into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteShort(short @short)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@short));
        }

        public void WriteShort(int @short)
        {
            WriteShort((short)@short);
        }

        /// <summary>
        ///   Write a int into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteInt(int @int)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@int));
        }

        /// <summary>
        ///   Write a long (as Int64) into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteLong(Int64 @long)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@long.ToNumber()));
        }

        /// <summary>
        ///   Write a long into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteLong(long @long)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@long));
        }

        /// <summary>
        ///   Write a UShort into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteUnsignedShort(ushort @ushort)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@ushort));
        }

        public void WriteUnsignedShort(int @ushort)
        {
            WriteUnsignedShort((ushort)@ushort);
        }

        /// <summary>
        ///   Write a int into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteUnsignedInt(UInt32 @uint)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@uint));
        }

        /// <summary>
        ///   Write a long into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteUnsignedLong(UInt64 @ulong)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@ulong));
        }

        /// <summary>
        ///   Write a byte into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteByte(byte @byte)
        {
            m_writer.Write(@byte);
        }

        public void WriteByte(int @byte)
        {
            WriteByte((byte)@byte);
        }

        public void WriteUnsignedByte(sbyte @sbyte)
        {
            m_writer.Write(@sbyte);
        }

        public void WriteUnsignedByte(int @sbyte)
        {
            WriteUnsignedByte((sbyte)@sbyte);
        }
        /// <summary>
        ///   Write a Float into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteFloat(float @float)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@float));
        }

        /// <summary>
        ///   Write a Boolean into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteBoolean(bool @bool)
        {
            if (@bool)
            {
                m_writer.Write((byte)1);
            }
            else
            {
                m_writer.Write((byte)0);
            }
        }

        /// <summary>
        ///   Write a Char into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteChar(char @char)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@char));
        }

        /// <summary>
        ///   Write a Double into the buffer
        /// </summary>
        public void WriteDouble(double @double)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@double));
        }

        /// <summary>
        ///   Write a Single into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteSingle(Single @single)
        {
            WriteBigEndianBytes(BitConverter.GetBytes(@single));
        }

        /// <summary>
        ///   Write a string into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteUTF(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var len = (ushort)bytes.Length;
            WriteUnsignedShort(len);

            int i;
            for (i = 0; i < len; i++)
                m_writer.Write(bytes[i]);
        }

        /// <summary>
        ///   Write a string into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteUTFBytes(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var len = bytes.Length;
            int i;
            for (i = 0; i < len; i++)
                m_writer.Write(bytes[i]);
        }

        /// <summary>
        ///   Write a bytes array into the buffer
        /// </summary>
        /// <returns></returns>
        public void WriteBytes(byte[] data)
        {
            m_writer.Write(data);
        }


        public void Seek(int offset)
        {
            Seek(offset, SeekOrigin.Begin);
        }

        public void Seek(int offset, SeekOrigin seekOrigin)
        {
            m_writer.BaseStream.Seek(offset, seekOrigin);
        }


        public void Clear()
        {
            m_writer = new BinaryWriter(new MemoryStream(), Encoding.UTF8);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            m_writer.Flush();
            m_writer.Dispose();
            m_writer = null;
        }
        #endregion
    }
}
