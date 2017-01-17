﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Core
{
    public class ChatCore
    {
        private ObservableCollection<User> m_users;
        private ObservableCollection<Message> m_messages;

        private void InitFields()
        {
            m_users = new ObservableCollection<User>();
            m_messages = new ObservableCollection<Message>();
        }

        private void FillUsers()
        {
            Register("Vlad", "123qwe", "Vlad");
        }

        public ChatCore()
        {
            InitFields();
            FillUsers();
        }

        public User Register(string login, string pass, string name)
        {
            if (m_users.Where(p => p.Login == login || p.Name == name).ToArray().Length > 0) return null;
            User user = new User() { Login = login, Password = pass, ID = Guid.NewGuid().ToString() };
            m_users.Add(user);
            return user;
        }

        public string Login(string login,string pass)
        {
            var result = m_users.Where(p => p.Login == login && p.Password == pass).ToArray();
            if (result.Length == 0) return "Error";
            return result[0].ID ;
        }

        public bool SendMessage(string ID, string message)
        {
            var user = m_users.Where(p => p.ID == ID).First();
            if (user == null) return false;
            m_messages.Add(new Message() { Text = message, Sender = user });
            return true;
        }

        public Message[] GetMessages()
        {
            return m_messages.ToArray();
        }

        public string[] GetUsers()
        {
            return m_users.Select(p => p.Name).ToArray();
        }
    }
}
