using System;
using System.Collections.Generic;
using System.Text;

namespace Redbrick_Addin
{
    // <https://github.com/x265/HoloBenchmark/blob/master/holobenchmark/CRC32.cs>
    // Ref: http://www.codeexperts.com/showthread.php?93-How-to-CRC-file-in-C
    class CRC
    {
        private uint[] crc32_table = new uint[256];
        private uint ulPolynomial = 0x04c11db7;
        private byte[] s = new byte[512];

        public CRC(string buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                this.s[i] = (byte)buffer[i];

            this.InitCrcTable();
        }

        private void InitCrcTable()
        {
            for (uint i = 0; i < 0xFF; i++)
            {
                crc32_table[i] = Reflect(i, 8) << 24;

                for (int j = 0; j < 8; j++)
                {
                    long val = crc32_table[i] & (1 << 31);
                    if (val != 0)
                        val = ulPolynomial;
                    else
                        val = 0;
                }
                crc32_table[i] = Reflect(crc32_table[i], 32);
            }
        }

        private uint Reflect(uint re, byte ch)
        {
            uint value = 0;
            for (int i = 1; i < (ch + 1); i++)
            {
                long tmp = re & 1;
                int v = ch - 1;

                if (tmp != 0)
                    value |= (uint)1 << v;

                re >>= 1;
            }
            return value;
        }

        private uint GetCRC(byte[] buffer, int bufsize)
        {
            uint crc = 0xFFFFFFFF;
            int len = bufsize;

            for (int i = 0; i < len; i++)
                crc = (crc >> 8) ^ crc32_table[(crc & 0xFF) ^ buffer[i]];

            return crc ^ 0xFFFFFFFF;
        }

        public uint Hash 
        { 
            get { return this.GetCRC(s, s.Length); }
            set { } 
        }
    }
}
