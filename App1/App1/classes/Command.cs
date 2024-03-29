﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

public enum CommandType {
    REGCOM,
    REGRET
}

public enum CommandParams {
    INT    = 0,
    FLOAT  = 1,
    BOOL   = 2,
    STRING = 3
}

public class Command{
    public CommandType type;
    public string name { get; set; }
    public string value { get; set; }
    public CommandParams[] param;

    public Label displayLabel;

    public Action<string> OnReciveRetData;

    public Command(string[] _formatData) {

        type = _formatData[0] == "REGCOM" ? CommandType.REGCOM : CommandType.REGRET;

        name = _formatData[1];

        string[] formatParams = _formatData[2].Split(',');
        param = new CommandParams[formatParams.Length];
        for (int i = 0; i < formatParams.Length; i++){
            param[i] = (CommandParams)int.Parse(formatParams[i]);
        }
    }

    public void send(string _name, params object[] _values) {
        string command = "DEVSEN;" + _name + ";";
        command += type == CommandType.REGCOM ? "COM;" : "RET;";
        command += name + ";";
        for (int i = 0; i < _values.Length; i++){
            command += _values[i].ToString();
            if (i < _values.Length - 1)
                command += ",";
        }

        ServerConnection.send(command);
    }

    public void setValue(String _value) {
        value = _value;
        try{
            if (displayLabel != null)
                displayLabel.Text = _value;
        }
        catch (Exception){
            return;
        }
    }
}

