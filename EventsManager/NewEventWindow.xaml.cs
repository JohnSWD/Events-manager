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
    /// Логика взаимодействия для NewEventWindow.xaml
    /// </summary>
    public partial class NewEventWindow : Window
    {
        public NewEventWindow()
        {
            InitializeComponent();
        }

        private Event _newEvent;

        

        public Event NewEvent
        {
            get { return _newEvent; }
            set { _newEvent = value; }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            int price = 0;
            if(string.IsNullOrWhiteSpace(textBoxName.Text))
                {
                MessageBox.Show("Пожалуйста, введите название события.");
                textBoxName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxLocation.Text))
            {
                MessageBox.Show("Пожалуйста, укажите место проведения события.");
                textBoxLocation.Focus();
                return;
            }

            if (!int.TryParse(textBoxPrice.Text, out price))
            {
                MessageBox.Show("Пожалуйста, введите цену арабскими цифрами.");
                textBoxPrice.Focus();
                return;
            }

            if (price < 0)
            {
                MessageBox.Show("Пожалуйста, введите положительное значение цены.");
                textBoxPrice.Focus();
                return;
            }

            _newEvent = new Event(textBoxName.Text, textBoxLocation.Text, price, textBoxDescription.Text);
            DialogResult = true;
        }
    }
}
