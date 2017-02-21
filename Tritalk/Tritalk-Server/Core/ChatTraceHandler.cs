using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tritalk.Core.ChatProtocolObjects;
using Tritalk.Libs;

namespace Tritalk.Core
{
    public class ChatTraceHandler : ITraceHandler
    {
        Dictionary<string, Func<Trace, Trace>> m_method_list;
        ChatCore chatCore;

        public event EventHandler<DataRequestEventArgs> DataRequest;

        private void OnDataAvailable(Trace trace)
        {
            EventHandler<DataRequestEventArgs> ev = Volatile.Read(ref DataRequest);
            ev?.Invoke(this, new DataRequestEventArgs() { Data = trace });
        }

        //public void NewMessageHandler(object sender, NewMessageEventArgs args)
        //{
        //    Message mess = args.Message;
        //    ProtoMessage proto_message = new ProtoMessage() { message_id = mess.MessageID, from = mess.From, time = mess.Time, text = mess.Text };
        //    Trace trace = new Trace() { Method = "NowNewMessage", Parameters = proto_message.ToJSON() };
        //    OnDataAvailable(trace);
        //}

        public ChatTraceHandler(ChatCore core)
        {
            m_method_list = new Dictionary<string, Func<Trace, Trace>>();
            chatCore = core;
            //chatCore.NewMessage += NewMessageHandler;
            InitMethodList();
        }

        void InitMethodList()
        {
            var m = m_method_list;
            m.Add("Registration", Registration);
            m.Add("Authorization", Authorization);
            m.Add("SendMessage", SendMessage);
            m.Add("GetUsers", GetUsers);
            m.Add("GetNewMessages", GetNewMessages);
            m.Add("IReadMessage", IReadMessage);
        }
        
        public Trace HandleTrace(Trace request)
        {
            if (!m_method_list.Keys.Contains(request.Method)) return request.ToError();
            Trace answer = m_method_list[request.Method](request);
            return answer;
        }

        private Trace Registration(Trace trace)
        {
            ProtoUser data = JsonConvert.DeserializeObject<ProtoUser>(trace.Parameters);

            User user = chatCore.GetUserByLogin(data.login);
            if (user == null) user = chatCore.GetUserByName(data.user);
            if (user != null) return trace.GetAnswer(new ProtoSuccess() { success = false });
            chatCore.Register(data.login.ToString(), data.password.ToString(), data.user.ToString());
            return trace.GetAnswer(new ProtoSuccess() { success = true });
        }
        private Trace Authorization(Trace trace)
        {
            ProtoUser data = JsonConvert.DeserializeObject<ProtoUser>(trace.Parameters);
            User user = chatCore.Login(data.login, data.password);
            if (user != null) return trace.GetAnswer(new ProtoUserAnswer() { success = true, id = user.ID, user = user.Name });
            return trace.GetAnswer(new ProtoUserAnswer() { success = false });
        }

        private Trace SendMessage(Trace trace)
        {
            ProtoMessage data = JsonConvert.DeserializeObject<ProtoMessage>(trace.Parameters);
            return trace.CloneTrace().GetAnswer(new ProtoSuccess() { success = chatCore.SendMessageById(data.id, data.to, data.text) });
        }

        private Trace GetUsers(Trace trace)
        {
            ProtoGetter data = JsonConvert.DeserializeObject<ProtoGetter>(trace.Parameters);
            User user = chatCore.GetUserById(data.id);
            if (user == null) return Trace.Empty;
            Trace answer = trace.CloneTrace();
            ProtoUsers users = new ProtoUsers() { users = chatCore.GetUsers() };
            answer.Parameters = JsonConvert.SerializeObject(users);
            return answer;
        }

        private Trace GetNewMessages(Trace trace)
        {
            ProtoGetter getter = ProtocolObject.FromJSON<ProtoGetter>(trace.Parameters);
            User user = chatCore.GetUserById(getter.id);
            Message[] messagelist = chatCore.GetNewMessages(user.Name);
            ProtoMessage[] protomesss = messagelist.Select(p => new ProtoMessage()
            { from = p.From, to = p.To, message_id = p.MessageID, text = p.Text, time = p.Time }).ToArray();
            Trace answer = trace.GetAnswer(new ProtoMessageList() { messages = protomesss });
            return answer;
        }
        private Trace IReadMessage(Trace trace)
        {
            ProtoMessage readed = ProtocolObject.FromJSON<ProtoMessage>(trace.Parameters);
            Message mess = chatCore.GetMessageById(readed.message_id);
            if (mess == null)
            {
                return trace.GetAnswer(new ProtoSuccess() { success = false });
            }
            chatCore.IReadMessage(mess.MessageID);
            return trace.GetAnswer(new ProtoSuccess() { success = true });
        }
        
    }
}
