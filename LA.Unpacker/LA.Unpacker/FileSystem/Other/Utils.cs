using System;
using System.IO;

namespace LA.Unpacker
{
    class Utils
    {
        public static void iSetError(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static String iCheckArgumentsPath(String m_Arg)
        {
            if (m_Arg.EndsWith("\\") == false)
            {
                m_Arg = m_Arg + @"\";
            }
            return m_Arg;
        }

        public static void iCreateDirectory(String m_Directory)
        {
            if (!Directory.Exists(Path.GetDirectoryName(m_Directory)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(m_Directory));
            }
        }
    }
}
