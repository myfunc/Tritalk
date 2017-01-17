using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Libs;

namespace Tritalk.Core
{
    class ChatTraceHandler : ITraceHandler
    {
        Dictionary<string, Func<Trace, Trace>> m_method_list;
        ChatCore chatCore;

        public ChatTraceHandler(ChatCore core)
        {
            m_method_list = new Dictionary<string, Func<Trace, Trace>>();
            chatCore = core;
            InitMethodList();
        }

        void InitMethodList()
        {
            var m = m_method_list;
            m.Add("Registration", Registration);
            m.Add("Authorization", Authorization);
        }

        public Trace Error
        {
            get
            {
                return new Trace() { Method = "Answer" , Properties = "Error"};
            }
        }
        
        public Trace HandleTrace(Trace request)
        {
            if (!m_method_list.Keys.Contains(request.Method)) return Error;
            Trace answer = m_method_list[request.Method](request);
            return answer;
        }

        private Trace Registration(Trace trace)
        {
            string[] result = trace.Properties.Split('*');//расшифровую логин и пароль 

            string login = result[0];//получаешь логин 
            string password = result[2];//получаешь пароль 

            User user = chatCore.Register(login, password, login);
            if (user == null) return Error;
            return new Trace() { Method = "Answer", Properties = "Registration complite" };
        }

        private Trace Authorization(Trace trace)
        {
            string[] result = trace.Properties.Split('*');//расшифровую логин и пароль 

            string login = result[0];//получаешь логин 
            string password = result[2];//получаешь пароль 

            string id = chatCore.Login(login, password);
            return new Trace() { Method = "Answer", Properties = id };
        }
    }
}
