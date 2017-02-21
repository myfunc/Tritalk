using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Client;

namespace ClientChatTest
{
    class Program
    {
        static ChatClient chat;
        static async void Test(string login, string pass)
        {
            await chat.Authorization(login, pass);
            Console.WriteLine(chat.AuthUser.Name); 
        }

        static void Main(string[] args)
        {
            TraceClient client = new TraceClient();

            Console.WriteLine("Press enter...");
            Console.ReadLine();
            client.Connect("localhost", 7770);
            client.StartAdapter();
            chat = new ChatClient(client);
            for (int i = 0; i < 20; i++)
            {
                Test("login" + i, "pass" + i);
            }
            Console.ReadLine();
        }
    }
}
