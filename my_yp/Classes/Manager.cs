using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_yp.Classes
{
    public class Manager : User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Manager(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Manager(User user) : base(user.UserId, user.Username, user.Password, user.UserRole)
        {

        }
    }
}

