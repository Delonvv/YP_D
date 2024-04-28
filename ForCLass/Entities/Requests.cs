using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForCLass.Entities
{
    public class Requests
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string ProblemType { get; set; }
        public string ProblemDescription { get; set; }
        public string Equipment { get; set; }
        public int UserId { get; set; }
        public virtual IEnumerable<User> User { get; set; }
        public string State { get; set; }

        public IEnumerable<History> History { get; set; }
    }
}
