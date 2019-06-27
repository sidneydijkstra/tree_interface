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

        private ObservableCollection<DeviceController> _devices = new ObservableCollection<DeviceController>();

        public main (){
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            DeviceList.ItemsSource = _devices;
            DeviceList.ItemTapped += deviceClicked;
            foreach (DeviceController device in ServerConnection.devices){
                _devices.Add(device);
            }
            ServerConnection.OnNewDeviceConnection += (DeviceController _device) =>{
                _devices.Add(_device);
            };
            
        }

        protected override void OnAppearing(){
            ServerConnection.refresh();
            _devices.Clear();
            foreach (DeviceController device in ServerConnection.devices){
                _devices.Add(device);
            }
        }

        private void deviceClicked(object sender, ItemTappedEventArgs args){
            string id = ((DeviceController)args.Item).id;
            DeviceController device = _devices.ToList().Find(x => x.id == id) ?? null;
            if (device != null){
                var nextPage = new devicepage(device);
                this.Navigation.PushAsync(nextPage);
            }
        }

        private void clickNavigateSettings(object sender, EventArgs e){
            var nextPage = new settingspage();
            this.Navigation.PushAsync(nextPage);
        }

        private void clickPartyMode(object sender, EventArgs e){

        }
    }
}