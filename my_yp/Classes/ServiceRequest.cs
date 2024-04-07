using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_yp.Classes
{
    public class ServiceRequest
    {
        public int RequestNumber { get; set; }
        public DateTime DateAdded { get; set; }
        public string Equipment { get; set; }
        public string IssueType { get; set; }
        public string Description { get; set; }
        public Client Client { get; set; }
        public string Status { get; set; }
        public DateTime CompletionTime { get; set; }
        public List<Employee> ResponsibleEmployee { get; set; }
        public List<ServiceRequestHistory> History { get; set; } = new List<ServiceRequestHistory>();

        public ServiceRequest(int requestNumber, DateTime dateAdded, string equipment, string issueType, string description, Client client, string status, List<Employee> responsibleEmployee, List<ServiceRequestHistory> history)
        {
            RequestNumber = requestNumber;
            DateAdded = dateAdded;
            Equipment = equipment;
            IssueType = issueType;
            Description = description;
            Client = client;
            Status = status;
            ResponsibleEmployee = responsibleEmployee;
            History = history;
        }
        public ServiceRequest(int requestNumber, DateTime dateAdded, string equipment, string issueType, string description, Client client, string status, List<Employee> responsibleEmployee)
        {
            RequestNumber = requestNumber;
            DateAdded = dateAdded;
            Equipment = equipment;
            IssueType = issueType;
            Description = description;
            Client = client;
            Status = status;
            ResponsibleEmployee = responsibleEmployee;
        }
        public ServiceRequest() { }
    }
}
