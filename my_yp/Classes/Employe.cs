using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_yp.Classes
{
    public class Employee : User
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public Employee() { }
        public Employee(User user) : base(user.UserId, user.Username, user.Password, user.UserRole)
        {

        }
        public Employee(int employeeID, string name)
        {
            EmployeeID = employeeID;
            Name = name;
        }
    }
}

