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
using System.IO;

namespace EventsManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Event> _events = new List<Event>();
        List<Category> _categories = new List<Category>();

        public MainWindow()
        {
            InitializeComponent();
            Event test = new Event("LCS LoL", "NY", 0, "Ежегодная континентальная лига.");
            test.Category = new Category("Киберспорт");
            _events.Add(test);
            Update();
            GetList();

        }
        
        private void Update()
        {
            listEvents.ItemsSource = null;
            listEvents.ItemsSource = _events;
        }

        private void GetList()
        {
            using (StreamReader sr = new StreamReader("CategoryList.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var name = sr.ReadLine();
                    int i = 0;
                    while (i < _categories.Count && _categories[i].TypeCategory != name)
                        i++;
                    if(i >= _categories.Count)
                    {
                        _categories.Add(new Category(name));
                    }
                    
                    
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewEventWindow(_categories);
            if (window.ShowDialog().Value)
            {
                _events.Add(window.NewEvent);
                _categories.Add(window.NewCategory);
                Update();
            }
        }
            

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if(listEvents.SelectedIndex != -1)
            {
                _events.RemoveAt(listEvents.SelectedIndex);
                Update();
            }
        }

        private void listEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDel.IsEnabled = listEvents.SelectedIndex != -1;
        }

        private void moreInfo_Click(object sender, RoutedEventArgs e)
        {
            
            Button cmd = (Button)sender;
            Event thisEvent = null;
            listEvents.SelectedItem = thisEvent;
            thisEvent = (Event)cmd.Tag;
            var window = new MoreInfo(thisEvent);
            if (window.ShowDialog().Value)
            {
                if (listEvents.SelectedIndex != -1)
                {
                    _events.RemoveAt(listEvents.SelectedIndex);
                    Update();
                }
                
            }

        }
    }
}
