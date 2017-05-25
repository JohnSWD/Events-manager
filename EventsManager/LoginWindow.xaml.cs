using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        List<User> _users = new List<User>();
        private MainWindow MW;
        private LoggingTool lt = new LoggingTool();
        public LoginWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoadUsers();
            LoginWindow win = this;
        }

        private void logIn_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (loginBox.Text.All(ch => char.IsLetterOrDigit(ch)) && !string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    int i = 0;
                    if (_users.Count == 0) MessageBox.Show("Вы первый пользователь! Вам необходимо прежде зарегистрироваться.");
                    else
                    {

                        while (loginBox.Text != _users[i].UserName || CalculateHash(passwordBox.Password)!= _users[i].Password)
                        {
                            i++;
                            if (i == _users.Count) break;
                        }
                        if (i < _users.Count)
                        {
                            if (MW != null)
                            {
                                MW.CurrentUser = _users[i];
                                MW.userTxtBlock.DataContext = MW.CurrentUser;
                                MW.btnToLog.IsEnabled = false;
                                MW.btnAdd.IsEnabled = true;
                                MW.btnToLog.Visibility = Visibility.Hidden;
                                this.Close();
                            }
                            else
                            {
                                var lw = this;
                                var window = new MainWindow(_users[i], lw);
                                window.Show();
                                window.btnToLog.Visibility = Visibility.Hidden;
                                //this.Visibility = Visibility.Collapsed;
                                this.Close();
                            }
                        lt.WriteActionLog(string.Format("Пользователь {0} успешно авторизировался.", _users[i].UserName));
                    }
                        else MessageBox.Show("Неправильно введен логин или пароль. Попробуйте еще раз.");
                    }
                }
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
                MessageBox.Show("Возникла ошибка при авторизации.");
            }
        }

        private void signUp_Click(object sender, RoutedEventArgs e)
        {
            if (loginBox.Text.Length < 6 || loginBox.Text.Length > 12 || passwordBox.Password.Length > 12 || passwordBox.Password.Length < 6) MessageBox.Show("Длина логина и пароля не может быть меньше 6 и не больше 12 симовлов.");
            else if (loginBox.Text.All(ch => char.IsLetterOrDigit(ch)) && !string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                try {
                    User check = _users.FirstOrDefault(n => n.UserName == loginBox.Text);
                    if (check == null)
                    {
                        User user = new User(loginBox.Text, CalculateHash(passwordBox.Password));
                        _users.Add(user);
                        SaveUsers();
                        if (MW != null)
                        {
                            MW.CurrentUser = user;
                            MW.userTxtBlock.DataContext = MW.CurrentUser;
                            MW.btnToLog.IsEnabled = false;
                            MW.btnAdd.IsEnabled = true;
                            MW.btnToLog.Visibility = Visibility.Hidden;
                            this.Close();
                            lt.WriteActionLog(string.Format("Новый пользователь {0} успешно зарегистрирован.", user.UserName));
                        }
                        else
                        {
                            var lw = this;
                            var window = new MainWindow(user, lw);
                            window.Show();
                            window.btnToLog.Visibility = Visibility.Hidden;
                            this.Close();
                            lt.WriteActionLog(string.Format("Новый пользователь {0} успешно зарегистрирован.", user.UserName));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь с таким именем уже существует.");
                        loginBox.Clear();
                        passwordBox.Clear();
                    }
                }
                catch(Exception ex)
                {
                    lt.WriteErrorLog(ex);
                    MessageBox.Show("При попытке зарегистироваться возникла ошибка");
                }
            }
            else
            {
                MessageBox.Show("Некорректный логин.");
                loginBox.Clear();
                passwordBox.Clear();
            }
        }

        private void guestBtn_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (MW != null)
                {
                    this.Visibility = Visibility.Collapsed;
                }
                else
                {
                    var lw = this;
                    var window = new MainWindow(new User("", ""), lw);
                    window.Show();
                    this.Visibility = Visibility.Collapsed;
                    lt.WriteActionLog("Пользователь вошел в программу без авторизации.");
                }
            }
            catch(Exception ex)
            {
                lt.WriteErrorLog(ex);
                MessageBox.Show("При попытке войти без авторизации возникла ошибка.");
            }
        }

        private string CalculateHash(string password)
        {
            MD5 md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
        public void MakeLoginVisible(MainWindow mw)
        {
            this.Visibility = Visibility.Visible;
            MW = mw;
        }
        
        private void SaveUsers()
        {
            using (var sw = new StreamWriter("users.txt"))
            {
                foreach (User user in _users)
                {
                    sw.WriteLine($"{user.UserName}/{user.Password}");
                }
            }
        }

        private void LoadUsers()
        {
            if (File.Exists("users.txt")) 
            {
                try
                {
                    using (var sr = new StreamReader("users.txt"))
                    {
                        while (!sr.EndOfStream)
                        {
                            string str = sr.ReadLine();
                            var userData = str.Split('/');
                            if (str != null)
                            {
                                if (userData.Count() == 2)
                                    _users.Add(new User(userData[0], userData[1]));
                            }

                        }
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("Возникла ошибка при получении данных о пользователях");
                    lt.WriteErrorLog(e);
                    
                }
            }
        }
    }
}
