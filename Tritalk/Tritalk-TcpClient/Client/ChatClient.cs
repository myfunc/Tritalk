using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tritalk.Core;
using Tritalk.Core.ChatProtocolObjects;
using Tritalk.Libs;

namespace Tritalk.Client
{
    public class ChatClient
    {
        User user = null;
        ITraceAdapter client = null;

        public event EventHandler<ReceivedDataEventArgs> ReceivedData;
        private void OnReceivedData(ReceivedDataEventArgs args)
        {
            EventHandler<ReceivedDataEventArgs> ev = Volatile.Read(ref ReceivedData);
            ev?.Invoke(this, args);
        }

        public ChatClient(ITraceAdapter traceAdapter)
        {
            client = traceAdapter;
            InitAdapter();
            InitHandlers();
        }

        void InitAdapter()
        {
            
            client.DataAvailable += ClientDataAvailable;
        }

        void InitHandlers()
        {

        }

        void ClientDataAvailable(object sender, AcceptTcpDataEventArgs args)
        {
            Trace trace = args.Trace;
            ProcedureTrace(trace);
        }

        void ProcedureTrace(Trace trace)
        {
            ReceivedDataEventArgs data = new ReceivedDataEventArgs() { Trace = trace };
            OnReceivedData(data);
        }

        public User AuthUser { get { return user; } }

        public async Task<T_out> DoitAsync<T_in, T_out>(string method, T_in data) 
            where T_out : ProtocolObject 
            where T_in : ProtocolObject
        {
            T_out result = null;
            Task task = new Task(() => { });
            Doit(method,data, (p) =>
            {
                result = ProtocolObject.FromJSON<T_out>(p.Parameters);
                task.Start();
            });
            await task;
            return result;
        }

        public void Doit<T>(string method, T data, Action<Trace> callback = null) where T : ProtocolObject
        {
            Trace trace_data = data.ToTrace(method);
            client.SendMessage(trace_data);
            EventHandler<AcceptTcpDataEventArgs> calldec = null;
            calldec = (s, a) => {
                if (a.Trace.PackageId == trace_data.PackageId)
                {
                    callback(a.Trace);
                    if (callback != null) client.DataAvailable -= calldec;
                }
            };
            if (callback != null) client.DataAvailable += calldec;
        }

        public async Task<bool> Authorization(string login, string password)
        {
            ProtoUserAnswer proto_user = await DoitAsync<ProtoUser, ProtoUserAnswer>("Authorization", new ProtoUser() { login = login, password = password });
            if (!proto_user.success) return false;
            user = new User() { ID = proto_user.id, Name = proto_user.user };
            return true;
        }

        public async Task<ProtoSuccess> Registration(string name, string login, string password)
        {
            return await DoitAsync<ProtoUser, ProtoSuccess>("Registration", new ProtoUser() { user = name, login = login, password = password });
        }

        public async Task<ProtoSuccess> SendMessage(string to, string text)
        {
            return await DoitAsync<ProtoMessage, ProtoSuccess>("SendMessage", new ProtoMessage() { id = user.ID, to = to, text = text });
        }
        public async Task<ProtoUsers> GetUsers()
        {
            return await DoitAsync<ProtoGetter, ProtoUsers>("GetUsers", new ProtoGetter() { id = user.ID });
        }
        public async Task<ProtoMessageList> GetNewMessages()
        {
            return await DoitAsync<ProtoGetter, ProtoMessageList>("GetNewMessages", new ProtoGetter() { id = user.ID });
        }
        public async Task<ProtoSuccess> IReadMessage(string message_id)
        {
            return await DoitAsync<ProtoMessage, ProtoSuccess>("IReadMessage", new ProtoMessage() { id = user.ID, message_id = message_id });
        }

        public void Logout()
        {
            user = null;
        }
        //public void Authorization(string login, string password, Action<Trace> callback)
        //{
        //    Doit<ProtoUser, ProtoUserAnswer>("Authorization",)

        //    ProtoUser data = new ProtoUser() { login = login, password = password };
        //    Trace trace_data = data.ToTrace("Authorization");
        //    client.SendMessage(trace_data);
        //    EventHandler<ReceivedDataEventArgs> calldec = null;
        //    calldec = (s, a) =>
        //    {
        //        if (a.Trace.PackageId == trace_data.PackageId)
        //        {
        //            callback(a.Trace);
        //            ReceivedData -= calldec;
        //        }
        //    };

        //    ReceivedData += calldec;
        //}

        //public async Task<ProtoSuccess> RegistrationAsync(string name, string login, string password)
        //{
        //    ProtoSuccess result = null;
        //    Task task = new Task(() => { });
        //    Registration(name, login, password, (p) =>
        //    {
        //        result = ProtocolObject.FromJSON<ProtoSuccess>(p.Parameters);
        //        task.Start();
        //    });
        //    await task;
        //    return result;
        //}

        //public void Registration(string name, string login, string password, Action<Trace> callback)
        //{
        //    ProtoUser data = new ProtoUser() { login = login, password = password, user = name };
        //    Trace trace_data = data.ToTrace("Registration");
        //    client.SendMessage(trace_data);
        //    EventHandler<ReceivedDataEventArgs> calldec = null;
        //    calldec = (s, a) =>
        //    {
        //        if (a.Trace.PackageId == trace_data.PackageId)
        //        {
        //            callback(a.Trace);
        //            ReceivedData -= calldec;
        //        }
        //    };

        //    ReceivedData += calldec;
        //}
    }
}
  
