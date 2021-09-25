using System;

namespace LA.Unpacker
{
    class LZ4
    {
        public static Byte[] iDecompress(Byte[] cmp, Int32 decLength)
        {
            Byte[] dec = new Byte[decLength];

            Int32 cmpPos = 0;
            Int32 decPos = 0;

            Int32 GetLength(Int32 length)
            {
                Byte sum;

                if (length == 0xf)
                {
                    do
                    {
                        length += (sum = cmp[cmpPos++]);
                    }
                    while (sum == 0xff);
                }

                return length;
            }

            do
            {
                Byte token = cmp[cmpPos++];

                Int32 litCount = token >> 4 & 0xf;
                Int32 encCount = token >> 0 & 0xf;

                // Copy literal chunk
                litCount = GetLength(litCount);

                Buffer.BlockCopy(cmp, cmpPos, dec, decPos, litCount);

                cmpPos += litCount;
                decPos += litCount;

                if (cmpPos >= cmp.Length)
                {
                    break;
                }

                // Copy compressed chunk
                Int32 back = cmp[cmpPos++] << 0 |
                           cmp[cmpPos++] << 8;

                encCount = GetLength(encCount) + 4;

                Int32 encPos = decPos - back;

                if (encCount <= back)
                {
                    Buffer.BlockCopy(dec, encPos, dec, decPos, encCount);

                    decPos += encCount;
                }
                else
                {
                    while (encCount-- > 0)
                    {
                        dec[decPos++] = dec[encPos++];
                    }
                }
            }
            while (cmpPos < cmp.Length &&
                   decPos < dec.Length);

            return dec;
        }
    }
}
