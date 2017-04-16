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
        public NewEventWindow(List<Category> categories)
        {
            InitializeComponent();
            categories.Sort((a, b) => a.TypeCategory.CompareTo(b.TypeCategory));
            comboBoxCategories.ItemsSource = categories;
            Сategories = categories;
        }
        List<Category> Сategories = new List<Category>();
        

        private Event _newEvent;
        public Event NewEvent
        {
            get { return _newEvent; }
            set { _newEvent = value; }
        }

        private Category _newCategory;

        public Category NewCategory
        {
            get { return _newCategory; }
            set { _newCategory = value; }
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

            if (comboBoxCategories.SelectedItem == null && string.IsNullOrWhiteSpace(textBoxCategory.Text))
            {
                MessageBox.Show("Пожалуста, выберите категорию");
                return;
            }
            _newEvent = new Event(textBoxName.Text, textBoxLocation.Text, price, textBoxDescription.Text);

            if (comboBoxCategories.SelectedIndex > -1 && textBoxCategory.Text.Length > 0) { MessageBox.Show("Необходимо выбрать категорию либо из списка, либо ввести свою."); comboBoxCategories.SelectedIndex = -1; return; }

            else if (textBoxCategory.Text.Length > 0)
            {
                int i = 0;
                while (i < Сategories.Count && Сategories[i].TypeCategory != textBoxCategory.Text)
                    i++;
                if (i >= Сategories.Count)
                {
                    _newCategory = new Category(textBoxCategory.Text);
                    _newEvent.Category = new Category(textBoxCategory.Text);
                }

                else
                {
                    MessageBox.Show("Введеная категория, которая уже есть в списке. Вы можете выбрать ее из списка.");
                    textBoxCategory.Clear();
                    return;

                }
            }

            

            else _newEvent.Category = comboBoxCategories.SelectedItem as Category;



            
            
            DialogResult = true;
        }
    }
}
