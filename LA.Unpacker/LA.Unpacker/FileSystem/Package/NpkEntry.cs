using System;

namespace LA.Unpacker
{
    class NpkEntry
    {
        public UInt64 dwHash { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public UInt32 dwCompressedCRC { get; set; }
        public UInt32 dwDecompressedCRC { get; set; }
        public Int32 dwCompressionFlag { get; set; } // 2 > lz4
    }
}
