using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_yp.Classes
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public User() { }
        public User(int userId, string username, string password, string userRole)
        {
            UserId = userId;
            Username = username;
            Password = password;
            UserRole = userRole;
        }
    }
}

