using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.pages{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class settingspage : ContentPage{
		public settingspage (){
			InitializeComponent ();
            entryName.Text = SettingsController.info.name;
            entryIp.Text = SettingsController.info.ip;
            entryPort.Text = SettingsController.info.port.ToString();
            entryAuthId.Text = SettingsController.info.auth_key;
        }

        private void clickResyncSettingsInfo(object sender, EventArgs e){
            System.Net.IPAddress tempIp;
            int tempInt;

            Setting_Info info = SettingsController.info;
            info.name = entryName.Text == "" ? info.name : entryName.Text;
            info.ip = !System.Net.IPAddress.TryParse(entryIp.Text, out tempIp) ? info.ip : entryIp.Text;
            info.port = int.TryParse(entryPort.Text, out tempInt) ? info.port : int.Parse(entryPort.Text);
            info.auth_key = entryAuthId.Text == "" ? info.auth_key : entryAuthId.Text;

            SettingsController.resyncInfo(info);
        }
	}
}