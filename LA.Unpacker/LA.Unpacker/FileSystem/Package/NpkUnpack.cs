using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace LA.Unpacker
{
    class NpkUnpack
    {
        static List<NpkEntry> m_EntryTable = new List<NpkEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            NpkHashList.iLoadProject();

            using (FileStream TFileStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TFileStream.ReadBytes(32);
                lpHeader = NpkCipher.iDecryptData(lpHeader, lpHeader.Length);

                var m_Header = new NpkHeader();
                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwHash = THeaderReader.ReadUInt64();
                    m_Header.dwMagic = THeaderReader.ReadUInt32(); // NXPK
                    m_Header.dwVersion = THeaderReader.ReadInt32(); // 3
                    m_Header.dwTableOffset = THeaderReader.ReadUInt32();
                    m_Header.dwTotalFiles = THeaderReader.ReadInt32();

                    if (m_Header.dwMagic != 0x4B50584E)
                    {
                        Utils.iSetError("[ERROR]: Invalid magic of NPK archive file");
                        return;
                    }

                    if (m_Header.dwVersion != 3)
                    {
                        Utils.iSetError("[ERROR]: Invalid version of NPK archive file");
                        return;
                    }

                    m_Header.dwTableSize = m_Header.dwTotalFiles * 48; //FFFFFFFFFFFFFFF0

                    THeaderReader.Dispose();
                }

                TFileStream.Seek(m_Header.dwTableOffset, SeekOrigin.Begin);

                var lpEntryTable = TFileStream.ReadBytes(m_Header.dwTableSize);
                lpEntryTable = NpkCipher.iDecryptData(lpEntryTable, lpEntryTable.Length);

                m_EntryTable.Clear();
                using (var TEntryReader = new MemoryStream(lpEntryTable))
                {
                    for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                    {
                        var m_Entry = new NpkEntry();

                        m_Entry.dwHash = TEntryReader.ReadUInt64();
                        m_Entry.dwOffset = TEntryReader.ReadUInt32();
                        m_Entry.dwCompressedSize = TEntryReader.ReadInt32();
                        m_Entry.dwDecompressedSize = TEntryReader.ReadInt32();
                        m_Entry.dwCompressedCRC = TEntryReader.ReadUInt32();
                        m_Entry.dwDecompressedCRC = TEntryReader.ReadUInt32();
                        m_Entry.dwCompressionFlag = TEntryReader.ReadInt32();
                        TEntryReader.Position += 16; //reserved data

                        m_EntryTable.Add(m_Entry);
                    }
                    TEntryReader.Dispose();
                }

                foreach(NpkEntry m_Entry in m_EntryTable)
                {
                    String m_FileName = NpkHashList.iGetNameFromHashList(m_Entry.dwHash.ToString("X16"));
                    
                    Console.WriteLine("[UNPACKING]: {0}", m_FileName);

                    String m_FullPath = m_DstFolder + m_FileName;
                    Utils.iCreateDirectory(m_FullPath);

                    TFileStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    var lpSrcBuffer = TFileStream.ReadBytes(m_Entry.dwCompressedSize);

                    if (m_Entry.dwCompressionFlag == 0)
                    {
                        if (Path.GetFileName(m_Archive) == "script.npk")
                        {
                            UInt16 wScriptMagic = BitConverter.ToUInt16(lpSrcBuffer, 0);
                            if (wScriptMagic != 0x4974)
                            {
                                lpSrcBuffer = NpkCipher.iDecryptData(lpSrcBuffer, lpSrcBuffer.Length);

                                Int64 isCompressedData = BitConverter.ToInt64(lpSrcBuffer, 0);
                                if (isCompressedData == 1)
                                {
                                    Int64 dwCompressedSize = BitConverter.ToInt64(lpSrcBuffer, 8);
                                    var lpDstBuffer = ZLIB.iDecompress(lpSrcBuffer, 18);

                                    File.WriteAllBytes(m_FullPath, lpDstBuffer);
                                }
                                else
                                {
                                    File.WriteAllBytes(m_FullPath, lpSrcBuffer);
                                }
                            }
                        }
                        else
                        {
                            File.WriteAllBytes(m_FullPath, lpSrcBuffer);
                        }
                    }
                    else if (m_Entry.dwCompressionFlag == 2)
                    {
                        var lpDstBuffer = LZ4.iDecompress(lpSrcBuffer, m_Entry.dwDecompressedSize);
                        File.WriteAllBytes(m_FullPath, lpDstBuffer);
                    }
                    else
                    {
                        File.WriteAllBytes(m_FullPath, lpSrcBuffer);
                    }
                }
            }
        }
    }
}
