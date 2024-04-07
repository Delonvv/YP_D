using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_yp.Classes
{
    public class ServiceRequestHistory
    {
        public int HistoryID { get; set; }
        public int RequestNumber { get; set; }
        public DateTime DateChanged { get; set; }
        public int OldEmployeeID { get; set; }
        public int NewEmployeeID { get; set; }
        public string Comments { get; set; }

        public ServiceRequestHistory(int historyID, int requestNumber, DateTime dateChanged, int oldEmployeeID, int newEmployeeID, string comments)
        {
            HistoryID = historyID;
            RequestNumber = requestNumber;
            DateChanged = dateChanged;
            OldEmployeeID = oldEmployeeID;
            NewEmployeeID = newEmployeeID;
            Comments = comments;
        }
    }
}
