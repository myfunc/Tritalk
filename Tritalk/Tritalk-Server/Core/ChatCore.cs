using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tritalk.Core
{
    public class ChatCore
    {
        private List<User> m_users;
        private List<Message> m_messages;
        private List<Message> m_unreaded_messages;

        public event EventHandler<NewMessageEventArgs> NewMessage;
        private void OnNewMessage(Message message)
        {
            EventHandler<NewMessageEventArgs> ev = Volatile.Read(ref NewMessage);
            ev?.Invoke(this, new NewMessageEventArgs() {Message = message});
        }

        private void InitFields()
        {
            m_users = new List<User>();
            m_messages = new List<Message>();
            m_unreaded_messages = new List<Message>(); 
        }

        public User GetUserById(string id)
        {
            var users = m_users.Where(p => p.ID == id);
            if (users.Count() == 0) return null;
            return users.First();
        }

        public User GetUserByName(string username)
        {
            var users = m_users.Where(p => p.Name == username);
            if (users.Count() == 0) return null;
            return users.First();
        }

        private void FillUsers()
        {
            Register("Vlad", "123qwe", "Vlad");
            Register("Den", "123", "Denich");
            for (int i = 0; i < 5; i++)
            {
                Register("login" + i*2, "pass" + i*2, "user" + i*2);
            }
        }

        public ChatCore()
        {
            InitFields();
            FillUsers();
        }

        public User Register(string login, string pass, string name)
        {
            if (m_users.Count(p => (p.Login == login) || (p.Name == name)) > 0) return null;
            User user = new User() { Login = login, Password = pass, ID = Guid.NewGuid().ToString(), Name = name };
            m_users.Add(user);
            return user;
        }

        public User Login(string login, string pass)
        {
            var result = m_users.Where(p => p.Login == login && p.Password == pass).ToArray();
            if (result.Length == 0) return null;
            return result.First();
        }

        public bool SendMessageById(string id, string receiver, string message)
        {
            User user = GetUserById(id);
            if (user == null) return false;
            m_unreaded_messages.Add(new Message(user.Name, receiver, message));
            return true;
        }
        public bool SendMessageByName(string name, string receiver, string message)
        {
            User user = GetUserByName(name);
            if (user == null) return false;
            m_unreaded_messages.Add(new Message(user.Name, receiver, message));
            return true;
        }

        public Message[] GetMessages()
        {
            return m_messages.ToArray();
        }

        public Message[] GetAllMessages(string username)
        {
            List<Message> messages = new List<Message>();
            m_unreaded_messages.Where(p => p.To == username).ToList().ForEach(p => messages.Add(p));
            m_messages.Where(p => p.To == username).ToList().ForEach(p => messages.Add(p));
            return messages.ToArray();
        }

        public Message[] GetNewMessages(string username)
        {
            var messages = m_unreaded_messages.Where(p => p.To == username);
            return messages.ToArray();
        }

        public Message GetMessageById(string MessageID)
        {
            var messages = m_unreaded_messages.Where(p => p.MessageID == MessageID);
            if (messages.Count() > 0)
            {
                return messages.First();
            }
            return null;
        }

        public User GetUserByLogin(string login)
        {
            var users = m_users.Where(p => p.Login == login);
            if (users.Count() == 0) return null;
            return users.First();
        }

        public void IReadMessage(string MessageID)
        {
            var messages = m_unreaded_messages.Where(p => p.MessageID == MessageID);
            if (messages.Count() > 0)
            {
                var mess = messages.First();
                m_messages.Add(mess);
                m_unreaded_messages.Remove(mess);
            }
        }

        public string[] GetUsers()
        {
            return m_users.Select(p => p.Name).ToArray();
        }

        public string[] GetUserLogins()
        {
            return m_users.Select(p => p.Login).ToArray();
        }

        public Message[] GetMessages(string user1, string user2)
        {
            throw new Exception();
            List<Message> messages = new List<Message>();
            // 1
            // 2
            return messages.ToArray();
        }
    }
}
