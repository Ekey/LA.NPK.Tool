using System;
using System.IO;
using System.Reflection;

namespace LA.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LifeAfter NPK Unpacker");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    LA.Unpacker <m_File> <m_Directory>");
                Console.WriteLine("    m_File - Source of NPK archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    LA.Unpacker E:\\Games\\LifeAfter\\res.npk D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_Input = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_Input))
            {
                Utils.iSetError("[ERROR]: Input file -> " + m_Input + " <- does not exist!");
                return;
            }

            NpkUnpack.iDoIt(m_Input, m_Output);
        }
    }
}
