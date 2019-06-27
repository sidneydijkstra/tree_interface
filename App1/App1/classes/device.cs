using System;
using System.Collections.Generic;
using System.Text;

public class DeviceController{

    public string id { get; set; }
    public string description { get; set; }

    public List<Command> commands;

    public DeviceController(string _id, string _description) {
        commands = new List<Command>();
        this.id = _id;
        this.description = _description;
    }

    public void addCommand(string[] _formatData) {
        // place data in standart device->server server->device model
        string[] formatData = new string[_formatData.Length-2];
        for (int i = 0; i < formatData.Length; i++){
            formatData[i] = _formatData[i + 2];
        }

        // check commands
        if (_formatData[2] == "REGCOM" || _formatData[2] == "REGRET"){
            commands.Add(new Command(formatData));
        }
    }

}

