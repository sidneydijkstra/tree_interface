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
        private ObservableCollection<Setting_Device_Commmand> _commandsDisplay = new ObservableCollection<Setting_Device_Commmand>();

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

            // set picket items
            List<string> _names = new List<string>();
            foreach (Command command in _commandsRet){
                _names.Add(command.name);
            }
            bindCommandPicker.ItemsSource = _names;

            // set command display
            CommandBindList.ItemsSource = _commandsDisplay;
            syncCommands();
        }

        private void syncCommands() {
            _commandsDisplay.Clear();
            if (SettingsController.commands != null) { 
                foreach (Setting_Device_Commmand comm in SettingsController.commands){
                    _commandsDisplay.Add(comm);
                }
            }
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

        private void clickBindNewCommand(object sender, EventArgs e){
            int type = bindTypePicker.SelectedIndex;
            string _id = bindCommandPicker.SelectedItem.ToString();
            Setting_Device_Commmand[] temp = SettingsController.commands;
            Setting_Device_Commmand[] commands;

            if (temp == null)
            {
                commands = new Setting_Device_Commmand[1];
            }
            else {
                int size = temp.Length == 0 ? 1 : temp.Length;
                commands = new Setting_Device_Commmand[size];
                for (int i = 0; i < temp.Length; i++){
                    commands[i] = temp[i];
                }
            }

            
            commands[commands.Length - 1] = new Setting_Device_Commmand() { type = (SettingDeviceCommandType)type, deviceName = _device.id, id = _id };

            SettingsController.resyncCommands(commands);
            Device.BeginInvokeOnMainThread(()=> { syncCommands(); });
        }

        private void clickRemoveCommand(object sender, EventArgs e){
            string id = ((Button)sender).Parent.FindByName<Label>("_idLabel").Text;

            Setting_Device_Commmand[] temp = SettingsController.commands;
            Setting_Device_Commmand[] list = new Setting_Device_Commmand[temp.Length-1];
            int count = 0;
            foreach (Setting_Device_Commmand comm in temp){
                if (comm.id != id) {
                    list[count] = comm;
                    count++;
                }
            }

            SettingsController.resyncCommands(list);
            Device.BeginInvokeOnMainThread(() => { syncCommands(); });
        }
    }
}