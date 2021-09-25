using System;
using System.IO;
using System.Text;

namespace LA.Unpacker
{
    class NpkHash
    {
        static UInt32 ROL(UInt32 x, Byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        static UInt32 MurMur3Core(Stream TStream, UInt32 dwSeed)
        {
            UInt32 c1 = 0xCC9E2D51;
            UInt32 c2 = 0x1B873593;

            UInt32 h1 = dwSeed;
            UInt32 k1 = 0;
            UInt32 dl = 0;

            using (BinaryReader TBinaryReader = new BinaryReader(TStream))
            {
                Byte[] lpData = TBinaryReader.ReadBytes(4);
                while (lpData.Length > 0)
                {
                    dl += (UInt32)lpData.Length;
                    switch (lpData.Length)
                    {
                        case 4:
                            k1 = (UInt32)(lpData[0] | lpData[1] << 8 | lpData[2] << 16 | lpData[3] << 24);
                            k1 *= c1;
                            k1 = ROL(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            h1 = ROL(h1, 13);
                            h1 = h1 * 5 + 0xe6546b64;
                            break;
                        case 3:
                            k1 = (UInt32)(lpData[0] | lpData[1] << 8 | lpData[2] << 16);
                            k1 *= c1;
                            k1 = ROL(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                        case 2:
                            k1 = (UInt32)(lpData[0] | lpData[1] << 8);
                            k1 *= c1;
                            k1 = ROL(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                        case 1:
                            k1 = (UInt32)(lpData[0]);
                            k1 *= c1;
                            k1 = ROL(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;

                    }
                    lpData = TBinaryReader.ReadBytes(4);
                }
            }

            h1 ^= dl;
            h1 ^= h1 >> 16;
            h1 *= 0x85ebca6b;
            h1 ^= h1 >> 13;
            h1 *= 0xc2b2ae35;
            h1 ^= h1 >> 16;

            unchecked
            {
                return h1;
            }
        }

        public static UInt32 iGetHash(String m_String, UInt32 dwSeed)
        {
            UInt32 dwHash = 0;
            Byte[] pBuffer = Encoding.ASCII.GetBytes(m_String);

            using (MemoryStream TMemoryStream = new MemoryStream(pBuffer))
            {
                dwHash = MurMur3Core(TMemoryStream, dwSeed);
            }
            return dwHash;
        }
    }
}
