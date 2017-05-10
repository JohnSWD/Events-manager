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
using System.Xml.Serialization;

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
            _categories.Add(new Category("Киберспорт", 0));
            Event test = new Event("LCS LoL", "NY", 0, "Ежегодная континентальная лига.");
            test.Category = _categories[0];
            test.CategoryId = test.Category.Id;
            GetList();
            DeserializeData();
            Update();
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
                    if (i >= _categories.Count)
                    {
                        _categories.Add(new Category(name, _categories.Count));
                    }


                }
            }
            Update();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewEventWindow(_categories);
            if (window.ShowDialog().Value)
            {
                _events.Add(window.NewEvent);
                if (window.NewCategory != null)
                _categories.Add(window.NewCategory);
                Update();
                SerializeData();
            }
        }


        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (listEvents.SelectedIndex != -1)
            {
                _events.RemoveAt(listEvents.SelectedIndex);
                Update();
                SerializeData();
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

        private void SerializeData()
        {
            SrlztnTool data = new SrlztnTool();
            data.Events = _events;
            data.Categories = _categories;
            using (var fs = new FileStream("events.xml", FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(SrlztnTool));
                xml.Serialize(fs, data);
            }
        }

        private SrlztnTool DeserializeData()
        {
            SrlztnTool data;
            using (var fs = new FileStream("events.xml", FileMode.Open))
            {
                XmlSerializer xml = new XmlSerializer(typeof(SrlztnTool));
                data = (SrlztnTool)xml.Deserialize(fs);
            }

            foreach (var evnt in data.Events)
            {
                int i = 0;
                while (evnt.CategoryId != data.Categories[i].Id) i++;
                evnt.Category = data.Categories[i];
                _events.Add(evnt);
            }
            return data;
        }

    }
}
