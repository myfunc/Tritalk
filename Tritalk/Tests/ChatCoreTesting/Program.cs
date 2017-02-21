using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Core;
using Tritalk.Core.ChatProtocolObjects;
using Tritalk.Libs;

namespace ChatCoreTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatCore chat = new ChatCore();
            ChatTraceHandler handler = new ChatTraceHandler(chat);
            for (int i = 0; i < 5; i++)
            {
                chat.Register("login" + i, "pass" + i, "user" + i);
            }
            chat.SendMessageByName("user1", "user2", "Test from 1 to 2");
            chat.SendMessageByName("user3", "user2", "Test from 3 to 2");
            chat.SendMessageByName("user3", "user1", "Test from 3 to 1");
            chat.SendMessageByName("user4", "user2", "Test from 4 to 2");

            

            string[] users = chat.GetUsers();
            foreach(var i in users)
            {
                Console.WriteLine("User: {0}, All messages: {1}, New messages: {2}", i, chat.GetAllMessages(i).Length, chat.GetNewMessages(i).Length);
            }

            ProtoUserAnswer user = JsonConvert.DeserializeObject<ProtoUserAnswer>(handler.HandleTrace(new Trace() { Method = "Authorization", Parameters = "{\"login\":\"login2\",\"password\":\"pass2\"}" }).Parameters);

            ProtoMessageList mesUser2 = ProtocolObject.FromJSON<ProtoMessageList>(handler.HandleTrace(new Trace()
            {
                Method = "GetNewMessages",
                Parameters = "{\"id\":\"" + user.id + "\"}"
            }).Parameters);

            foreach (var i in mesUser2.messages)
            {
                Console.WriteLine("Message {0} From: {1}, To: {2}, Text: {3}", i.message_id,i.from,i.to,i.text);
                chat.IReadMessage(i.message_id);
            }

            foreach (var i in users)
            {
                Console.WriteLine("User: {0}, All messages: {1}, New messages: {2}", i, chat.GetAllMessages(i).Length, chat.GetNewMessages(i).Length);
            }

        }
    }
}
