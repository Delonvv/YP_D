using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace my_yp.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        MainWindow mainWindow;
        MainContext mainContext;
        public Authorization(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            mainContext = new MainContext();
        }

        private void Auth(object sender, RoutedEventArgs e)
        {
            if (login.Text != "")
            {
                if (password.Text != "")
                {
                    var user = mainContext.Users.FirstOrDefault(c => c.Password == password.Text && c.Login == login.Text);

                    if (user != null)
                    {
                        if (user.Role == "User")
                            mainWindow.OpenPage(MainWindow.pages.userAddReq, user.Id);
                        if (user.Role == "Worker")
                            mainWindow.OpenPage(MainWindow.pages.workerReq);
                        if (user.Role == "Manager")
                            mainWindow.OpenPage(MainWindow.pages.managerPage);
                    }
                    else MessageBox.Show("пользователь не найден");
                }
                else
                    MessageBox.Show("не введен пароль");
            }
            else
                MessageBox.Show("не введен логин");

        }
    }
}
}
