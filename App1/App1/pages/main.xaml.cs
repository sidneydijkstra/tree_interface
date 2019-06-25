using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.pages{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class main : ContentPage{

        private ObservableCollection<Device> _devices = new ObservableCollection<Device>();

        public main (){
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            DeviceList.ItemsSource = _devices;
            DeviceList.ItemTapped += deviceClicked;
            foreach (Device device in ServerConnection.devices){
                _devices.Add(device);
            }
            ServerConnection.OnNewDeviceConnection += (Device _device) =>{
                _devices.Add(_device);
            };
        }

        private void deviceClicked(object sender, ItemTappedEventArgs args){
            string id = ((Device)args.Item).id;
            Device device = _devices.ToList().Find(x => x.id == id) ?? null;
            if (device != null){
                var nextPage = new devicepage(device);
                this.Navigation.PushAsync(nextPage);
            }
        }
    }
}