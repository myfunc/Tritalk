using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tritalk.Libs;

namespace Tritalk.Core.ChatProtocolObjects
{
    public static class TraceProtocolBuilder
    {
        public static Trace ToTrace(this ProtocolObject obj, Trace answer)
        {
            Trace trace = answer.CloneTrace();
            trace.Parameters = obj.ToJSON();
            return trace;
        }

        public static Trace ToTrace(this ProtocolObject obj, string method)
        {
            return new Trace() { Method = method, Parameters = obj.ToJSON() };
        }
    }

    [Serializable]
    public abstract class ProtocolObject
    {
        virtual public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
        static public T FromJSON<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    [Serializable]
    public class ProtoGetter : ProtocolObject
    {
        public string id;
    }

    [Serializable]
    public class ProtoUsers : ProtocolObject
    {
        public string[] users;
    }

    [Serializable]
    public class ProtoUser : ProtocolObject
    {
        public string login;
        public string password;
        public string user;
    }

    [Serializable]
    public class ProtoUserAnswer : ProtocolObject
    {
        public string id;
        public bool success;
        public string user;
    }

    [Serializable]
    public class ProtoSuccess : ProtocolObject
    {
        public bool success;
    }

    [Serializable]
    public class ProtoMessage : ProtocolObject
    {
        public string id;
        public string message_id;
        public string from;
        public string to;
        public string time;
        public string text;
    }
    [Serializable]
    public class ProtoMessageList : ProtocolObject
    {
        public ProtoMessage[] messages;
    }
}
