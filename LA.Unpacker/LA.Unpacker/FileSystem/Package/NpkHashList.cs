using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace LA.Unpacker
{
    class NpkHashList
    {
        static String m_Path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static String m_ProjectFile = @"\Projects\FileNames.list";
        static String m_ProjectFilePath = m_Path + m_ProjectFile;

        static Dictionary<String, String> m_HashList = new Dictionary<String, String>();

        public static void iLoadProject()
        {
            String m_Line = null;
            if (!File.Exists(m_ProjectFilePath))
            {
                Utils.iSetError("[ERROR]: Unable to load project file " + m_ProjectFile);
            }

            Int32 i = 0;
            m_HashList.Clear();

            StreamReader TProjectFile = new StreamReader(m_ProjectFilePath);
            while ((m_Line = TProjectFile.ReadLine()) != null)
            {
                m_Line = m_Line.Replace("/", @"\");
                UInt32 dwHashA = NpkHash.iGetHash(m_Line, 0x66666666);
                UInt32 dwHashB = NpkHash.iGetHash(m_Line, 0x77777777);
                String m_Hash = dwHashB.ToString("X8") + dwHashA.ToString("X8");

                if (m_HashList.ContainsKey(m_Hash))
                {
                    String m_Collision = null;
                    m_HashList.TryGetValue(m_Hash, out m_Collision);
                    Console.WriteLine("[COLLISION]: {0} <-> {1}", m_Collision, m_Line);
                }

                m_HashList.Add(m_Hash, m_Line);
                i++;
            }

            TProjectFile.Close();
            Console.WriteLine("[INFO]: Project File Loaded: {0}", i);
            Console.WriteLine();
        }

        public static String iGetNameFromHashList(String m_Hash)
        {
            String m_FileName = null;

            if (m_HashList.ContainsKey(m_Hash))
            {
                m_HashList.TryGetValue(m_Hash, out m_FileName);
            }
            else
            {
                m_FileName = @"__Unknown\" + m_Hash;
            }

            return m_FileName;
        }
    }
}
