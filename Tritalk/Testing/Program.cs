using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Libs;

namespace Tritalk.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "Hello world";
            byte[] str_byte = Encoding.UTF8.GetBytes(str);
            byte[] byte_str_size = PackageBuider.AddSizeToByteArray(str_byte);
            int size = PackageBuider.GetSizeFromPackage(byte_str_size);
            Console.WriteLine(size);
        }

        static void TestLibs()
        {
            TestLibsPackageBuilder();
        }

        static void TestLibsPackageBuilder()
        {
            TraceBuilderTest();
            PackageBuilderTest();
        }

        static void TraceBuilderTest()
        {
            Logger.Method("TraceBuilder");
            Random rand = new Random(DateTime.Now.GetHashCode());
            for (int i = 0; i < 5; i++)
            {
                Trace trace = new Trace() {
                    ID = rand.Next().ToString(),
                    Method = rand.Next().ToString(),
                    Properties = rand.Next().ToString()
                };

                Trace result = TraceBuilder.Deserialize(TraceBuilder.Serialize(trace));
                Logger.Run(result.CompareTo(trace) == 0, trace, result);
            }
        }
        static void PackageBuilderTest()
        {
            Logger.Method("PackageBuilder");
            Random rand = new Random(DateTime.Now.GetHashCode());
            for (int i = 0; i < 5; i++)
            {
                string str = rand.Next().ToString();
                byte[] inner = Encoding.UTF8.GetBytes(str);
                byte[] output = PackageBuider.AddSizeToByteArray(inner);
                int result = 0;
                if (inner.Length + 4 != output.Length) result = 1;
                //if (BitConverter.ToInt32(Array.S, 0) != inner.Length) result = 2;
                Logger.Run(result == 0, result);
            }
        }
    }
}
