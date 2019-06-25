using App1.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void connectionStart(object sender, EventArgs e){
            NetworkError status = ServerConnection.init("192.168.1.3", 11000);
            if (!status.error) {
                connectionStatus.Text = "Connected to Tree Of Life";

                Application.Current.MainPage = new NavigationPage(new main());
            }
            else{
                connectionStatus.Text = "Connection failed, error: " + status.message;
            }
        }
    }
}
