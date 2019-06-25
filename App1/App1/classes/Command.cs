using System;
using System.Collections.Generic;
using System.Text;

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
    public CommandParams[] param;

    public Command(string[] _formatData) {

        type = _formatData[0] == "REGCOM" ? CommandType.REGCOM : CommandType.REGRET;

        name = _formatData[1];

        string[] formatParams = _formatData[2].Split(',');
        param = new CommandParams[formatParams.Length];
        for (int i = 0; i < formatParams.Length; i++){
            param[i] = (CommandParams)int.Parse(formatParams[i]);
        }
    }
}

