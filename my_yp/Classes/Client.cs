using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace my_yp.Classes
{
    public class Client : User
    {
        public int ClientID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Client(User user) : base(user.UserId, user.Username, user.Password, user.UserRole)
        {
            // Инициализация свойств клиента, если таковые имеются
        }
        public Client(int clientID, string name, string email)
        {
            ClientID = clientID;
            Name = name;
            Email = email;
        }
    }
}

