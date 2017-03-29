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

namespace EventsManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Event> _events = new List<Event>();
        public MainWindow()
        {
            InitializeComponent();
            _events.Add(new Event("LCS LoL", "NY", 0, "Ежегодная континентальная лига."));
            Update();
            
        }

        private void Update()
        {
            listEvents.ItemsSource = null;
            listEvents.ItemsSource = _events;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewEventWindow();
            if (window.ShowDialog().Value)
            {
                _events.Add(window.NewEvent);
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
    }
}
