using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Core
{
    [Serializable]
    public class User
    {
        public User()
        {
            ID = String.Empty;
            Login = String.Empty;
            Password = String.Empty;
            Name = String.Empty;
        }

        public string ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
