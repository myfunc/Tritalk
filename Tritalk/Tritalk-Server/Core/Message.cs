using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Core
{
    [Serializable]
    public class Message
    {
        private string message_id;
        private string time;
        private string text;
        private string from;
        private string to;

        public string MessageID { get { return message_id; } }
        public string Time { get { return time; } }
        public string Text { get { return text; } }
        public string From { get { return from; } }
        public string To { get { return to; } }

        public Message(string sender, string receiver, string message)
        {
            message_id = Guid.NewGuid().ToString();
            time = DateTime.Now.ToString();
            text = message;
            from = sender;
            to = receiver;
        }

        public override string ToString()
        {
            return string.Format("Message id: {0}, Time: {1}, Text: {2}, From: {3}, To: {4}", MessageID, Time, Text, From, To);
        }
    }
}
