using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace my_yp.Classes
{
    internal class DatabaseManager
    {
        private string connectionString = "Server=192.168.1.6;Database=ServiceDataBase;User=diplomUser;Password=12332155;Encrypt=false;trusted_connection=false";

        public DatabaseManager(string connectionString)
        {
            this.connectionString = "Server=192.168.1.6;Database=ServiceDataBase;User=sa;Password=12332155;Encrypt=false;trusted_connection=false";
        }

        // Метод для получения списка всех заявок
        public List<ServiceRequest> GetAllServiceRequests()
        {
            List<ServiceRequest> requests = new List<ServiceRequest>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ServiceRequests";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ServiceRequest request = ReadServiceRequestFromReader(reader);
                    requests.Add(request);
                }
            }

            return requests;
        }

        // Метод для получения отдельной заявки по её номеру
        public ServiceRequest GetServiceRequest(int requestNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ServiceRequests WHERE RequestNumber = @RequestNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestNumber", requestNumber);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadServiceRequestFromReader(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        // Метод для добавления новой заявки в базу данных
        public void AddServiceRequest(ServiceRequest request)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO ServiceRequests ( DateAdded, Equipment, IssueType, Description, ClientID, Status) OUTPUT INSERTED.RequestNumber " +
                               "VALUES (@DateAdded, @Equipment, @IssueType, @Description, @ClientID, @Status)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DateAdded", request.DateAdded);
                command.Parameters.AddWithValue("@Equipment", request.Equipment);
                command.Parameters.AddWithValue("@IssueType", request.IssueType);
                command.Parameters.AddWithValue("@Description", request.Description);
                command.Parameters.AddWithValue("@ClientID", request.Client.ClientID);
                command.Parameters.AddWithValue("@Status", request.Status);

                connection.Open();
                int requestId = (int)command.ExecuteScalar();

                command = new SqlCommand($"Insert into ResponceEmployye(Employye, Request) values({request.ResponsibleEmployee[0].EmployeeID},{requestId})", connection);
                command.ExecuteNonQuery();
            }
        }

        public void ChangeEmployee(int newEmployee, int requestId, int oldEmployee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand($"Update ResponceEmployye set Employye = {newEmployee} where [Request] = {requestId} and [Employye] = {oldEmployee}", connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void AddRequestEmployee(int newEmployee, int requestId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand($"Insert into ResponceEmployye(Employye, Request) values({newEmployee},{requestId})", connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        // Метод для изменения заявки в базе данных
        public void UpdateServiceRequest(ServiceRequest request)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE ServiceRequests SET DateAdded = @DateAdded, Equipment = @Equipment, IssueType = @IssueType, " +
                               "Description = @Description, ClientID = @ClientID, Status = @Status WHERE RequestNumber = @RequestNumber";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@RequestNumber", request.RequestNumber);
                command.Parameters.AddWithValue("@DateAdded", request.DateAdded);
                command.Parameters.AddWithValue("@Equipment", request.Equipment);
                command.Parameters.AddWithValue("@IssueType", request.IssueType);
                command.Parameters.AddWithValue("@Description", request.Description);
                command.Parameters.AddWithValue("@ClientID", request.Client.ClientID);
                command.Parameters.AddWithValue("@Status", request.Status);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Метод для удаления заявки из базы данных
        public void DeleteServiceRequest(int requestNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM ServiceRequests WHERE RequestNumber = @RequestNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestNumber", requestNumber);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Вспомогательный метод для чтения заявки из SqlDataReader
        public List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clients";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Client client = ReadClientFromReader(reader);
                    clients.Add(client);
                }
            }

            return clients;
        }

        private Client ReadClientFromReader(SqlDataReader reader)
        {
            int clientID = Convert.ToInt32(reader["ClientID"]);
            string name = reader["Name"].ToString();
            string email = reader["Email"].ToString();

            return new Client(clientID, name, email);
        }

        private ServiceRequest ReadServiceRequestFromReader(SqlDataReader reader)
        {
            int requestNumber = Convert.ToInt32(reader["RequestNumber"]);
            DateTime dateAdded = Convert.ToDateTime(reader["DateAdded"]);
            string equipment = reader["Equipment"].ToString();
            string issueType = reader["IssueType"].ToString();
            string description = reader["Description"].ToString();
            int clientID = Convert.ToInt32(reader["ClientID"]);
            string status = reader["Status"].ToString();

            Client client = GetClientByID(clientID);
            List<ServiceRequestHistory> history = GetHistoryByRequestNumber(requestNumber); // Загрузка истории изменений
            List<Employee> responsibleEmployees = GetResponsableEmployyes(requestNumber); // Загрузка истории изменений

            return new ServiceRequest(requestNumber, dateAdded, equipment, issueType, description, client, status, responsibleEmployees, history);
        }

        private Client GetClientByID(int clientID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clients WHERE ClientID = @ClientID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ClientID", clientID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadClientFromReader(reader);
                }
                else
                {
                    return null;
                }
            }
        }
        private Client GetClientUserId(int clientID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clients WHERE UserId = @ClientID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ClientID", clientID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadClientFromReader(reader);
                }
                else
                {
                    return null;
                }
            }
        }
        public void AddHistoryRecord(ServiceRequestHistory hostoryRecord)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO ServiceRequestHistories (RequestNumber, DateChanged, OldEmployeeID, NewEmployeeID, Comments) " +
                               "VALUES (@RequestNumber, @DateChanged, @OldEmployeeID, @NewEmployeeID, @Comments)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestNumber", hostoryRecord.RequestNumber);
                command.Parameters.AddWithValue("@DateChanged", DateTime.Now);
                command.Parameters.AddWithValue("@OldEmployeeID", hostoryRecord.OldEmployeeID);
                command.Parameters.AddWithValue("@NewEmployeeID", hostoryRecord.NewEmployeeID);
                command.Parameters.AddWithValue("@Comments", hostoryRecord.Comments);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public List<Employee> GetResponsableEmployyes(int reqId)
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM ResponceEmployye where Request  = {reqId}";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    Employee employee = GetEmployeeByID(reader.GetInt32(1));
                    employees.Add(employee);
                }
            }
            return employees;
        }

        public List<ServiceRequestHistory> GetHistoryByRequestNumber(int requestNumber)
        {
            List<ServiceRequestHistory> history = new List<ServiceRequestHistory>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ServiceRequestHistories WHERE RequestNumber = @RequestNumber ORDER BY DateChanged DESC";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestNumber", requestNumber);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int historyID = Convert.ToInt32(reader["HistoryID"]);
                    DateTime dateChanged = Convert.ToDateTime(reader["DateChanged"]);
                    int oldEmployeeID = Convert.ToInt32(reader["OldEmployeeID"]);
                    int newEmployeeID = Convert.ToInt32(reader["NewEmployeeID"]);
                    string comments = reader["Comments"].ToString();

                    history.Add(new ServiceRequestHistory(historyID, requestNumber, dateChanged, oldEmployeeID, newEmployeeID, comments));
                }
            }

            return history;
        }
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int employeeID = Convert.ToInt32(reader["EmployeeID"]);
                    string name = reader["Name"].ToString();

                    employees.Add(new Employee(employeeID, name));
                }
            }

            return employees;
        }

        public Employee GetEmployeeByID(int employeeID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string name = reader["Name"].ToString();
                    return new Employee(employeeID, name);
                }
                else
                {
                    return null;
                }
            }
        }
        public Employee GetEmployeeByUserId(int employeeID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees WHERE UserId = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string name = reader["Name"].ToString();
                    return new Employee(Id, name);
                }
                else
                {
                    return null;
                }
            }
        }
        public Manager GetManagerByUserId(int employeeID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Managers WHERE UserId = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string name = reader["Name"].ToString();
                    return new Manager(Id, name);
                }
                else
                {
                    return null;
                }
            }
        }

        public void UpdateRequestStatus(int requestNumber, string newStatus)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE ServiceRequests SET Status = @Status WHERE RequestNumber = @RequestNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Status", newStatus);
                command.Parameters.AddWithValue("@RequestNumber", requestNumber);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void UpdateResponsibleEmployee(int requestNumber, string responsibleEmployee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE ServiceRequests SET ResponsibleEmployee = @ResponsibleEmployee WHERE RequestNumber = @RequestNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ResponsibleEmployee", responsibleEmployee);
                command.Parameters.AddWithValue("@RequestNumber", requestNumber);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void AddCommentToRequest(int requestNumber, string comment)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE ServiceRequests SET Comments = Comments + @Comment WHERE RequestNumber = @RequestNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Comment", comment);
                command.Parameters.AddWithValue("@RequestNumber", requestNumber);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public List<ServiceRequest> GetServiceRequestsByParameter(string parameterName, string parameterValue)
        {
            List<ServiceRequest> serviceRequests = new List<ServiceRequest>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query;
                if (parameterName == "ClientName")
                {
                    query = "SELECT * FROM ServiceRequests INNER JOIN Clients ON ServiceRequests.ClientID = Clients.ClientID WHERE Clients.Name = @ParameterValue";
                }
                else
                {
                    query = $"SELECT * FROM ServiceRequests WHERE {parameterName} = @ParameterValue";
                }

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParameterValue", parameterValue);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ServiceRequest request = ReadServiceRequestFromReader(reader);
                    serviceRequests.Add(request);
                }
            }

            return serviceRequests;
        }
        public void AddClient(Classes.Client client)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int UserId = CreateUserAndGetId(client);
                string query = "INSERT INTO Clients (Name, Email, UserId) VALUES (@Name, @Email,@UserId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@UserId", UserId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public int CreateUserAndGetId(User user)
        {
            int userId = -1; // Инициализируем идентификатор пользователя значением по умолчанию

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Username, Password, UserRole) OUTPUT INSERTED.UserId VALUES (@Username, @Password, @UserRole)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@UserRole", user.UserRole);

                connection.Open();

                userId = (int)command.ExecuteScalar();
            }

            return userId;
        }
        public void AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int UserId = CreateUserAndGetId(employee);
                string query = "INSERT INTO Employees (Name,UserId) VALUES (@Name,@UserId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@UserId", UserId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Employees SET Name = @Name WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", employee.Name);
                command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public int GetCompletedServiceRequestsCount()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM ServiceRequests WHERE Status = 'выполнено'";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count;
            }
        }
        public TimeSpan GetAverageCompletionTime()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT AVG(DATEDIFF(MINUTE, DateAdded, CompletionTime)) FROM ServiceRequests WHERE Status = 'выполнено'";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                var res = command.ExecuteScalar();
                double resDoub = 0;
                if (double.TryParse(res.ToString(), out resDoub))
                {

                }
                TimeSpan averageCompletionTime = TimeSpan.FromMinutes((double)resDoub);
                return averageCompletionTime;
            }
        }
        public List<IssueStatistics> GetIssueStatistics()
        {
            List<IssueStatistics> issueStatistics = new List<IssueStatistics>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IssueType, COUNT(*) AS Count FROM ServiceRequests GROUP BY IssueType";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string issueType = reader["IssueType"].ToString();
                    int count = Convert.ToInt32(reader["Count"]);
                    IssueStatistics statistics = new IssueStatistics(issueType, count);
                    issueStatistics.Add(statistics);
                }
            }

            return issueStatistics;
        }
        public User AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Создаем объект пользователя
                    User user = new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        UserRole = reader["UserRole"].ToString() // Предположим, что UserRole хранит роль пользователя
                    };

                    // Возвращаем соответствующий объект пользователя в зависимости от его роли
                    switch (user.UserRole)
                    {
                        case "Manager":
                            return GetManagerByUserId(user.UserId);
                        case "Client":
                            return GetClientUserId(user.UserId);
                        case "Employee":
                            return GetEmployeeByUserId(user.UserId);
                        default:
                            return null;
                    }
                }
                else
                {
                    // Пользователь с таким логином и паролем не найден
                    return null;
                }
            }
        }
    }
}

