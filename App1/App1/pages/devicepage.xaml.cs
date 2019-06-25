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
	public partial class devicepage : ContentPage{

        private Device _device;
        private ObservableCollection<Command> _commands = new ObservableCollection<Command>();

        public devicepage(Device _device){
			InitializeComponent();
            this._device = _device;
            CommandList.ItemsSource = this._commands;
            foreach (Command command in _device.commands){
                _commands.Add(command);
            }
            initPage();

        }

        private void initPage() {
            deviceName.Text = "Device: " + _device.id;
            deviceDesc.Text = _device.description;
        }
	}
}