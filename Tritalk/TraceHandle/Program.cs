using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Core;
using Tritalk.Libs;

namespace TraceHandle
{
    class Program
    {
        static void ShowAnswer(Trace trace)
        {
            Console.WriteLine(trace);
        }

        static void Main(string[] args)
        {
            ChatTraceHandler core = new ChatTraceHandler(new ChatCore());
            ShowAnswer(core.HandleTrace(new Trace() { Method = "Registration", Parameters = JsonConvert.SerializeObject(
                new { login="Ivan",password="qwerty",user="Ivanich" }) }));
            Trace ivan = core.HandleTrace(new Trace() { Method = "Authorization", Parameters = "{login:'Ivan',password:'qwerty'}" });
            ShowAnswer(ivan);
            dynamic divan = JsonConvert.DeserializeObject(ivan.Parameters);
            string id = divan.id.ToString();
            dynamic answ = core.HandleTrace(new Trace() { Method = "GetUsers", Parameters = ivan.Parameters });

        }
    }
}
