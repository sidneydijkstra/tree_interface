using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Setting_Info {
    public string name;
    public string ip;
    public int port;
    public string auth_key;
}

public struct Setting_Tree {
    public Setting_RGB normal;
    public Setting_RGB blink;
}

public struct Setting_RGB {
    public int r;
    public int g;
    public int b;
}

public enum SettingDeviceCommandType {
    SET = 0,
    BLINK = 1
}

public struct Setting_Device_Commmand {
    public SettingDeviceCommandType type { get; set; }
    public string deviceName { get; set; }
    public string id { get; set; }
}

public static class SettingsController{
    public static Setting_Info info;
    public static Setting_Tree tree;
    public static Setting_Device_Commmand[] commands;

    public static void loadInfo(string _json) {
        info = JSONParser.FromJson<Setting_Info>(_json);
        return;
    }
    public static void loadTree(string _json) {
        tree = JSONParser.FromJson<Setting_Tree>(_json);
        return;
    }
    public static void loadCommands(string _json) {
        commands = JSONParser.FromJson<Setting_Device_Commmand[]>(_json);
        return;
    }

    public static void resyncInfo(Setting_Info _info) {
        ServerConnection.send("RESYNCSET;info;" + JSONWriter.ToJson(_info));
        info = _info;
    }
    public static void resyncTree(Setting_Tree _tree) {
        ServerConnection.send("RESYNCSET;tree;" + JSONWriter.ToJson(_tree));
        tree = _tree;
    }
    public static void resyncCommands(Setting_Device_Commmand[] _commands) {
        ServerConnection.send("RESYNCSET;commands;" + JSONWriter.ToJson(_commands));
        commands = _commands;
    }
}

