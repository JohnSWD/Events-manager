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
using System.Windows.Shapes;

namespace EventsManager
{
    /// <summary>
    /// Логика взаимодействия для MoreInfo.xaml
    /// </summary>
    public partial class MoreInfo : Window
    {

        private User _user;
        private LoggingTool lt = new LoggingTool();
        public MoreInfo(Event thisEvent, User user)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try {
                Event EditedEvent = new Event(thisEvent.Name, thisEvent.Location, thisEvent.Price, thisEvent.Descirption);
                EditedEvent.Category = thisEvent.Category;
                DataContext = EditedEvent;
                i = thisEvent.Category.Id;
                InitializeComponent();
                btnEdit.Width = this.Width / 2;
                btnReady.Width = btnEdit.Width;
                _user = user;
                _initialEvent = thisEvent;
                textBoxDate.Text = thisEvent.EventDate.ToShortDateString();
                GetAccess(user);
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
            }
        }
        private int i;

        private Event _initialEvent;
        
        private Event _editedEvent;

        public Event EditedEvent
        {
            get { return _editedEvent; }
            set { _editedEvent = value; }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

            try {
                btnReady.IsEnabled = true;
                btnEdit.IsEnabled = false;
                textBoxDate.Focusable=textBoxName.Focusable = textBoxLocation.Focusable = textBoxPrice.Focusable = textBoxDescription.Focusable = true;
                }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
               
            }
        }
        private void btnReady_Click(object sender, RoutedEventArgs e)
        {
            try {
                int price = 0;
                if (string.IsNullOrWhiteSpace(textBoxName.Text))
                {
                    MessageBox.Show("Некорректное название события.");
                    textBoxName.Text = _initialEvent.Name;
                    textBoxName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(textBoxLocation.Text) || !textBoxLocation.Text.All(ch => char.IsLetter(ch)))
                {
                    MessageBox.Show("Неправильно введено навзвание города (возможно, использованы недопустимые символы).");
                    textBoxLocation.Text = _initialEvent.Location;
                    textBoxLocation.Focus();
                    return;
                }

                if (!int.TryParse(textBoxPrice.Text, out price))
                {
                    MessageBox.Show("Некорректная цена.");
                    textBoxPrice.Text = Convert.ToString(_initialEvent.Price);
                    textBoxPrice.Focus();
                    return;
                }

                if (price < 0)
                {
                    MessageBox.Show("Некорректная цена.");
                    textBoxPrice.Text = Convert.ToString(_initialEvent.Price);
                    textBoxPrice.Focus();
                    return;
                }

                var dateChecker = textBoxDate.Text.Split('.');
                if (dateChecker.Count() != 3)
                {
                    MessageBox.Show("Введите значения даты через точку.");
                    textBoxDate.Text = _initialEvent.EventDate.ToShortDateString();
                    return;
                }

                if (string.IsNullOrEmpty(textBoxDate.Text) || DateTime.Compare(Convert.ToDateTime(textBoxDate.Text), DateTime.Now) < 0)
                {
                    MessageBox.Show("Введите корректную дату. Дата события не может быть в прошлом.");
                    return;
                }

                EditedEvent = new Event(textBoxName.Text, textBoxLocation.Text, price, textBoxDescription.Text);
                EditedEvent.Category = new Category(textBoxCategory.Text, i);
                EditedEvent.EventDate = Convert.ToDateTime(textBoxDate.Text);
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
                MessageBox.Show("При редактировании события возникла ошибка");
            }
            lt.WriteActionLog(string.Format("Пользователь {0} изменил событие.", _user.UserName));
            DialogResult = true;
        }

        private void GetAccess(User user)
        {
            if (btnEdit != null)
            {
                if (user.UserName == "Гость") btnEdit.IsEnabled = false;
                else btnEdit.IsEnabled = true;
            }
        }
    }
}
