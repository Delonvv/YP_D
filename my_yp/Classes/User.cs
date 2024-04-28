using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForClass.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public int? RequestId { get; set; }
        public Requests Request { get; set; }

        public IEnumerable<History> History { get; set; }
    }
}
