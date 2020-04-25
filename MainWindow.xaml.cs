using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TwitchChatBot_WPF_
{
    public partial class MainWindow : Window
    {
        Logger logger;
        TwitchChatBot bot;
        //инит окно (Ну тут ясно)
        public MainWindow()
        {
            InitializeComponent();
            bot = new TwitchChatBot();
            logger = new Logger();
            //Ебучий таймер
            var aTimer = new System.Timers.Timer(100);
            aTimer.AutoReset = true;
            aTimer.Elapsed += LogEvent;
            aTimer.Enabled = true;
        }
        //Тот самый логивент
        private void LogEvent(object sender, ElapsedEventArgs e)
        {
            string txtlog = logger.Get();
            this.Dispatcher.Invoke(() => TextLog.Text += txtlog + Environment.NewLine);
        }
        //Чисто кнопки
        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            bot.Connect();
            ConnectBtn.IsEnabled = false;
            if (ConnectBtn.IsEnabled == false)
            {
                DisBtn.IsEnabled = true;
            }
        }

        private void DisBtn_Click(object sender, RoutedEventArgs e)
        {
            bot.Disconnect();
            ConnectBtn.IsEnabled = true;
            if (ConnectBtn.IsEnabled == true)
            {
                DisBtn.IsEnabled = false;
            }
        }
    }
}
