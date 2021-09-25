using System;

namespace LA.Unpacker
{
    class NpkHeader
    {
        public UInt64 dwHash { get; set; }
        public UInt32 dwMagic { get; set; }
        public Int32 dwVersion { get; set; }
        public UInt32 dwTableOffset { get; set; }
        public Int32 dwTableSize { get; set; }
        public Int32 dwTotalFiles { get; set; }
    }
}
