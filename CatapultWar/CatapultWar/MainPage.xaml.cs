using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using com.shephertz.app42.gaming.multiplayer.client;
using CatapultWar.AppWarp;
using System.Windows.Threading;
using System.Windows.Navigation;

namespace CatapultWar
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Initialize the SDK with your applications credentials that you received
            // after creating the app from http://apphq.shephertz.com
            WarpClient.initialize(GlobalContext.API_KEY, GlobalContext.SECRET_KEY);
            WarpClient.setRecoveryAllowance(60);
            // Keep a reference of the SDK singleton handy for later use.
            GlobalContext.warpClient = WarpClient.GetInstance();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            MessagePopup.Visibility = Visibility.Collapsed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                WarpClient.GetInstance().Disconnect();
            }
            catch (Exception e1)
            { 
            
            }

            base.OnBackKeyPress(e);
        }
        // Simple button Click event handler to take us to the second page
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch(Convert.ToInt32(btn.Tag.ToString()))
            {
                case 0:
                    if (GlobalContext.IsConnectedToAppWarp)
                    {
                        PlayVsHumanPopup.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        JoinPoupup.Visibility = Visibility.Visible;
                    }
                    break;
                case 1:
                     messageTB.Text = "Please wait..";
                     MessagePopup.Visibility = Visibility.Visible;
                     App.g_isTwoHumanPlayers=false;
                    NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case 2: 
                    if(String.IsNullOrEmpty(userName.Text))
                    {
                    showMessage("please fill remote user name..");
                    return;
                    }
                     messageTB.Text = "Please wait..";
                     MessagePopup.Visibility = Visibility.Visible;
                    if (GlobalContext.IsConnectedToAppWarp)
                    {
                        JoinRoomWithRemoteUserID();
                    }
                    else
                    {
                        GlobalContext.warpClient.RemoveConnectionRequestListener(GlobalContext.conListenObj);
                        GlobalContext.conListenObj = new ConnectionListener(showResult, JoinRoomWithRemoteUserID);
                        GlobalContext.warpClient.AddConnectionRequestListener(GlobalContext.conListenObj);
                        WarpClient.GetInstance().Connect(GlobalContext.localUsername);
                    }
                      break;
                case 3:
                     messageTB.Text = "Please wait..";
                     MessagePopup.Visibility = Visibility.Visible;
                    if (GlobalContext.IsConnectedToAppWarp)
                    {
                        JoinRoomWithRandomPlayer();
                    }
                    else
                    {
                        GlobalContext.warpClient.RemoveConnectionRequestListener(GlobalContext.conListenObj);
                        GlobalContext.conListenObj = new ConnectionListener(showResult, JoinRoomWithRandomPlayer);
                        GlobalContext.warpClient.AddConnectionRequestListener(GlobalContext.conListenObj);
                        WarpClient.GetInstance().Connect(GlobalContext.localUsername);
                    }
                      break;
            }
        }
        internal void moveToPlayScreen()
        {
            GlobalContext.fireNumber = 1;
            App.g_isTwoHumanPlayers = true;
            Dispatcher.BeginInvoke(() =>
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.RelativeOrAbsolute)));
        }
        private void AddListeners()
        {
            if (GlobalContext.roomReqListenerObj == null)
            {
                GlobalContext.roomReqListenerObj = new RoomReqListener(showResult, moveToPlayScreen);
                GlobalContext.warpClient.AddRoomRequestListener(GlobalContext.roomReqListenerObj);
            }
            if (GlobalContext.notificationListenerObj == null)
            {
                GlobalContext.notificationListenerObj = new NotificationListener();
                WarpClient.GetInstance().AddNotificationListener(GlobalContext.notificationListenerObj);
            }
            if (GlobalContext.zoneRequestListenerobj == null)
            {
                GlobalContext.zoneRequestListenerobj = new ZoneRequestListener();
                WarpClient.GetInstance().AddZoneRequestListener(GlobalContext.zoneRequestListenerobj);
            }
        }
        private  void JoinRoomWithRemoteUserID()
        {
            AddListeners();
            //here we are looking for location-ID by his username
            WarpClient.GetInstance().GetLiveUserInfo(userName.Text);
        }
        private void JoinRoomWithRandomPlayer()
        {
           AddListeners();
           Dictionary<string, object>  tableProperties = new Dictionary<string, object>();
           tableProperties.Add("IsPrivateRoom", "false");
           WarpClient.GetInstance().JoinRoomWithProperties(tableProperties);
        }
        public void showResult(String result)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                showMessage(result);
            });
        }
        private void joinButton_Click(object sender, RoutedEventArgs e)
        {
            int tag = Convert.ToInt32((sender as FrameworkElement).Tag.ToString());
            switch(tag)
            {   case 0:
                    if (string.IsNullOrWhiteSpace(txtUserName.Text))
                        MessageBox.Show("Please Specifiy user name");
                    else
                    {
                        messageTB.Text = "Please wait..";
                        MessagePopup.Visibility = Visibility.Visible;
                        // Initiate the connection
                        // Create and add listener objects to receive callback events for the APIs used
                        GlobalContext.conListenObj = new ConnectionListener(JoinedCallback);
                        GlobalContext.warpClient.AddConnectionRequestListener(GlobalContext.conListenObj);
                        GlobalContext.localUsername = txtUserName.Text;
                        WarpClient.GetInstance().Connect(GlobalContext.localUsername);
                    }
                    break;
            case 1: JoinPoupup.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        internal void JoinedCallback(string message)
        {
            if (message.Equals(""))
            {
                PlayVsHumanPopup.Visibility = Visibility.Visible;
                JoinPoupup.Visibility = Visibility.Collapsed;
                MessagePopup.Visibility = Visibility.Collapsed;
                GlobalContext.IsConnectedToAppWarp = true;
            }
            else
            {
                GlobalContext.IsConnectedToAppWarp = false;
                Dispatcher.BeginInvoke(() => showMessage("Error,Please try again later"));
            }
        }
        void showMessage(string message)
        {
            messageTB.Text = message;
            MessagePopup.Visibility = Visibility.Visible;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            MessagePopup.Visibility = Visibility.Collapsed;
            (sender as DispatcherTimer).Stop();
        }
    }
}