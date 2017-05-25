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
using System.ComponentModel;

namespace EventsManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Event> _events = new List<Event>();
        List<Category> _categories = new List<Category>();
        ICollectionView smth;
        private LoggingTool lt = new LoggingTool();
        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        private LoginWindow LW;
        
        public MainWindow(User user, LoginWindow lw)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try {
                InitializeComponent();
                CurrentUser = user;
                if (CurrentUser.UserName == "")
                {
                    btnToLog.IsEnabled = true;
                    btnAdd.IsEnabled = false;
                    CurrentUser.UserName = "Гость";
                }
                LW = lw;
                userTxtBlock.DataContext = CurrentUser;

                DeserializeData();
                Update();
                smth = CollectionViewSource.GetDefaultView(_events);
                new SearchTool(smth, this.searchBox);
                eventsNumber.Text = Convert.ToString(_events.Count());
                Event next = _events.OrderBy(ev => ev.EventDate).FirstOrDefault();
                nextEvent.Text = next.Name;
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
            }
        }

        private void Update()
        {
            listEvents.ItemsSource = null;
            listEvents.ItemsSource = _events;
            eventsNumber.Text = Convert.ToString(_events.Count());
            if (_events.Count() > 0)
            {
                Event next = _events.OrderBy(ev => ev.EventDate).FirstOrDefault();
                nextEvent.Text = next.Name;
            }
            else nextEvent.Text = "";
        }

        private void GetRelevantData()
        {
            int i = 0;
            foreach (Event eve in _events)
            {
                if (DateTime.Compare(eve.EventDate, DateTime.Now) < 0) { _events.Remove(eve); i++; }
            }
            if (i > 0) MessageBox.Show("Было удалено {0} прошедших события", Convert.ToString(i));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try {
                var window = new NewEventWindow(_categories, _currentUser);
                if (window.ShowDialog().Value)
                {
                    _events.Add(window.NewEvent);
                    if (window.NewCategory != null)
                        _categories.Add(window.NewCategory);
                    Update();
                    SerializeData();
                }
                smth.Refresh();
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
            }
        }


        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (listEvents.SelectedIndex != -1)
                {
                    lt.WriteActionLog(string.Format("Пользователь {0} удалил событие {1}", _currentUser.UserName, _events[listEvents.SelectedIndex].Name));
                    _events.RemoveAt(listEvents.SelectedIndex);
                    Update();
                    SerializeData();
                }
                smth.Refresh();
                
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
            }
        }

        private void listEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_currentUser.UserName!="Гость")
            btnDel.IsEnabled = listEvents.SelectedIndex != -1;
        }

        private void moreInfo_Click(object sender, RoutedEventArgs e)
        {
            try {
                Button cmd = (Button)sender;
                Event thisEvent = null;
                thisEvent = (Event)cmd.Tag;
                int selectedIndex = _events.IndexOf(thisEvent);
                var window = new MoreInfo(thisEvent, _currentUser);
                if (window.ShowDialog().Value)
                {
                    if (selectedIndex!= -1)
                    {
                        _events.Add(window.EditedEvent);
                        _events.RemoveAt(selectedIndex);
                        Update();
                        SerializeData();
                    }
                }
                smth.Refresh();
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
            }
        }

        private void SerializeData()
        {
            try {
                SrlztnTool data = new SrlztnTool();
                data.Events = _events;
                data.Categories = _categories;
                using (var fs = new FileStream("events.xml", FileMode.Create))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(SrlztnTool));
                    xml.Serialize(fs, data);
                }
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
            }
        }

        private void DeserializeData() 
        {
            try {
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
                _categories = data.Categories;
                GetRelevantData();
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
            }
        }

        private void btnToLog_Click(object sender, RoutedEventArgs e)
        {
            LW.MakeLoginVisible(this);
            listEvents.SelectedIndex = -1;  
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (LW != null) LW.Close();
            Close();   
        }
    }
}
