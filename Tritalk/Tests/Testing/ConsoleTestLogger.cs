using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Testing
{
    static class Logger
    {
        static readonly ConsoleColor m_method_color = ConsoleColor.Yellow;
        static readonly ConsoleColor m_valid_test = ConsoleColor.Green;
        static readonly ConsoleColor m_invalid_test = ConsoleColor.Red;
        static readonly ConsoleColor m_default = ConsoleColor.White;

        static ConsoleColor m_buffer = m_default;

        static public void SaveColor()
        {
            m_buffer = Console.ForegroundColor;
        }

        static public void LoadColor()
        {
            Console.ForegroundColor = m_buffer;
        }

        static public void Method(string name)
        {
            SaveColor();
            Console.ForegroundColor = m_default;
            Console.Write("Test Instance: ");
            Console.ForegroundColor = m_method_color;
            Console.Write("{0} ",name);
            Console.WriteLine();

            LoadColor();
        }

        static public void Run(bool result, params object[] args)
        {
            SaveColor();
            Console.ForegroundColor = m_default;
            Console.WriteLine("{ Arguments:");
            for (int i = 0; i < args.Length; i++)
            {
                if (i == args.Length - 1)
                {
                    Console.WriteLine("\tWaitable result: {0}", args[args.Length - 1]);
                    break;
                }
                Console.WriteLine("\t{0}: {1}", i+1, args[i]);
            }
            Console.Write("} ");
            Result(result);
            LoadColor();
        }
        static private void Result(bool result)
        {
            SaveColor();
            string valid = "Test complited";
            string invalid = "Test have wrong result";
            if (result)
            {
                Console.ForegroundColor = m_valid_test;
                Console.WriteLine(valid);
            } else
            {
                Console.ForegroundColor = m_invalid_test;
                Console.WriteLine(invalid);
            }
            LoadColor();
        }
    }
}
