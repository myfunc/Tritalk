using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Libs
{
    static public class TraceJsonAnswerBuilder
    {

        static public Trace GetAnswer(this Trace trace, object structure)
        {
            try
            {
                Trace clone = trace.CloneTrace();
                clone.Parameters = JsonConvert.SerializeObject(structure);
                return clone;
            } catch
            {
                return Trace.Empty;
            }
        }
    }
}
