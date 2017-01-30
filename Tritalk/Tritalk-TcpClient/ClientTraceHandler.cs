using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Core;
using Tritalk.Libs;

namespace Tritalk.Client
{
    class ClientTraceHandler/* : ITraceHandler*/
    {
        Dictionary<string, Func<Trace, Trace>> m_method_list;

        //public ClientTraceHandler(ChatCore core)
        //{
        //    m_method_list = new Dictionary<string, Func<Trace, Trace>>();
        //    chatCore = core;
        //    InitMethodList();
        //}

        //public Trace HandleTrace(Trace trace)
        //{
        //    throw new NotImplementedException();
        //}

        //void InitMethodList()
        //{
        //    var m = m_method_list;
        //    m.Add("Registration", Registration);
        //    m.Add("Authorization", Authorization);
        //}
    }
}
