using System;
using System.IO;
using System.IO.Compression;

namespace LA.Unpacker
{
    class ZLIB
    {
        public static Byte[] iDecompress(Byte[] lpBuffer, Int32 dwOffset)
        {
            var TOutMemoryStream = new MemoryStream();
            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer) { Position = dwOffset })
            {
                using (DeflateStream TDeflateStream = new DeflateStream(TMemoryStream, CompressionMode.Decompress, false))
                {
                    TDeflateStream.CopyTo(TOutMemoryStream);
                    TDeflateStream.Dispose();
                }
                TMemoryStream.Dispose();
            }

            return TOutMemoryStream.ToArray();
        }
    }
}
