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

        private DeviceController _device;
        private ObservableCollection<Command> _commandsCom = new ObservableCollection<Command>();
        private ObservableCollection<Command> _commandsRet = new ObservableCollection<Command>();

        public devicepage(DeviceController _device){
			InitializeComponent();
            this._device = _device;
            CommandComList.ItemsSource = this._commandsCom;
            CommandRetList.ItemsSource = this._commandsRet;
            foreach (Command command in _device.commands.FindAll(x => x.type == CommandType.REGCOM)){
                _commandsCom.Add(command);
            }
            foreach (Command command in _device.commands.FindAll(x => x.type == CommandType.REGRET)){
                _commandsRet.Add(command);
            }
            initPage();

        }

        private void initPage() {
            deviceName.Text = "Device: " + _device.id;
            deviceDesc.Text = _device.description;
        }

        private void clickComSend(object sender, EventArgs e){
            Button b = (Button)sender;
            Command command = _commandsCom.ToList().Find(x => x.name == b.CommandParameter.ToString());
            if (command != null) {
                Picker entry = b.Parent.FindByName<Picker>("_valueInput");
                bool value;
                if (entry.SelectedItem.ToString() == "" || entry.SelectedItem.ToString() == null) // TODO: add all values support
                    return;
                if(bool.TryParse(entry.SelectedItem.ToString(), out value)) { 
                    command.send(_device.id, value);
                }
            }
        }

        private void clickRetSend(object sender, EventArgs e) {
            Button b = (Button)sender;
            Command command = _commandsRet.ToList().Find(x => x.name == b.CommandParameter.ToString());
            if (command != null) {
                Label label = b.Parent.FindByName<Label>("_valueOutput");
                command.displayLabel = label;
                command.OnReciveRetData += (string _data)=>{
                    Device.BeginInvokeOnMainThread(()=> { command.setValue(_data); });
                };
                ServerConnection.send("DEVSEN;" + _device.id + ";RET;" + command.name);
            }
        }

        
    }
}