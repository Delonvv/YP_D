using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_yp.Classes
{
    public class IssueStatistics
    {
        public string IssueType { get; set; }
        public int Count { get; set; }

        public IssueStatistics(string issueType, int count)
        {
            IssueType = issueType;
            Count = count;
        }
    }
}

