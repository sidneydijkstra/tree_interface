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

public static class SettingsController{
    public static Setting_Info info;

    public static void load(string _json) {
        info = JSONParser.FromJson<Setting_Info>(_json);
        return;
    }

    public static void resyncInfo(Setting_Info _info) {
        ServerConnection.send("RESYNCSET;info;" + JSONWriter.ToJson(_info));
        info = _info;
    }
}

