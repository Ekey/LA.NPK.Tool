using System;
using System.Security.Cryptography;

namespace LA.Unpacker
{
    class NpkCipher
    {
        public static Byte[] iDecryptData(Byte[] lpBuffer, Int32 dwSize)
        {
            RijndaelManaged TAES = new RijndaelManaged();

            Byte[] m_Key = new Byte[] { 0x60, 0x63, 0x08, 0xD8, 0xA3, 0x2C, 0x78, 0x20, 0x13, 0xD2, 0x6C, 0x2F, 0x22, 0x6F, 0x68, 0x6D };
            Byte[] m_IV = new Byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            TAES.KeySize = 128;
            TAES.BlockSize = 128;
            TAES.Key = m_Key;
            TAES.IV = m_IV;
            TAES.Mode = CipherMode.ECB;
            TAES.Padding = PaddingMode.None;

            Int32 dwBlockSize = 16;
            Int32 dwBlockOffset = 0;
            Int32 dwBlockCount = dwSize / dwBlockSize;

            for (Int32 i = 0; i < dwBlockCount; i++)
            {
                ICryptoTransform TICryptoTransform = TAES.CreateDecryptor();
                TICryptoTransform.TransformBlock(lpBuffer, dwBlockOffset, dwBlockSize, lpBuffer, dwBlockOffset);
                dwBlockOffset += dwBlockSize;
                TICryptoTransform.Dispose();
            }

            TAES.Dispose();

            return lpBuffer;
        }
    }
}
