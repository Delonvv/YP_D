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
using my_yp.Classes;

namespace my_yp.Pages.RequestPage
{
    /// <summary>
    /// Логика взаимодействия для AddRequest.xaml
    /// </summary>
    public partial class AddRequest : Page
    {
        MainWindow mainWindow;
        MainContext mainContext;
        int UserId;
        public AddRequest(MainWindow mainWindow, int? id)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            mainContext = new MainContext();
            UserId = id.Value;
        }

        private void Auth(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPage(MainWindow.pages.auth);
        }

        private async void Add(object sender, RoutedEventArgs e)
        {
            if (tbnumberreq.Text != "")
            {
                if (tbDate.Text != "")
                {
                    if (tbEquipment.Text != "")
                    {
                        if (tbDesc.Text != "")
                        {
                            if (tbTypePromlem.Text != "")
                            {
                                if (tbState.Text != "")
                                {
                                    var date = DateTime.TryParse(tbDate.Text, out DateTime time) ? time : DateTime.MinValue;
                                    if (date == DateTime.MinValue)
                                    {
                                        MessageBox.Show("Введите правильно дату!");
                                        return;
                                    }

                                    var request = new Requests
                                    {
                                        Date = date,
                                        Number = tbnumberreq.Text,
                                        ProblemDescription = tbDesc.Text,
                                        ProblemType = tbTypePromlem.Text,
                                        State = tbState.Text,
                                        UserId = UserId,
                                        Equipment = tbEquipment.Text,
                                    };

                                    await mainContext.Requests.AddAsync(request);
                                    await mainContext.SaveChangesAsync();

                                    MessageBox.Show("Заявка принята в работу");
                                }
                                else MessageBox.Show("Введите статус");
                            }
                            else MessageBox.Show("Введите тип проблемы");
                        }
                        else MessageBox.Show("Введите описание");
                    }
                    else MessageBox.Show("Введите оборудование");
                }
                else MessageBox.Show("Введите дату");
            }
            else MessageBox.Show("Введите номер заявки");
        }
    }
}
