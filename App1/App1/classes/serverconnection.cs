using App1;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xamarin.Forms;

public class NetworkError{
    public bool error;
    public Exception exception;
    public string message;
}

public static class ServerConnection{
    private static Thread _listenThread;

    public static Action<DeviceController> OnNewDeviceConnection;
    public static List<DeviceController> devices;

    public static Action OnConnectionStarted;
    public static Action<string> OnRecieveTcpPackage;
    public static Action OnConnectionEnded;

    public static IPAddress ip;
    public static int port;

    private static IPEndPoint _localEndPoint;
    private static Socket _connection;

    public static NetworkError init(string _ip, int _port) {
        if (isConnected())
            return new NetworkError() { error = false };

        devices = new List<DeviceController>();

        ip = IPAddress.Parse(_ip);
        port = _port;
        _localEndPoint = new IPEndPoint(ip, port);
        _connection = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            _connection.Connect(_localEndPoint);

            setupPackageHandeling();
            _listenThread = new Thread(new ThreadStart(_listen));
            _listenThread.Start();

            OnConnectionStarted?.Invoke();
            send("USERJOIN");
            return new NetworkError() { error = false };
        }
        catch (Exception ex)
        {
            return new NetworkError() { error=true, exception=ex, message=ex.ToString()} ;
        }
    }

    private static void setupPackageHandeling() {
        OnRecieveTcpPackage += (string _data)=>{
            string[] formatData = _data.Split(';');
            if (formatData[0] == "INIT") {
                //send("GETDEV");
            } else if (formatData[0] == "REGDEV") {
                DeviceController dev = new DeviceController(formatData[1], formatData[2]);
                DeviceController tempDev = devices.Find(x => x.id == dev.id) ?? null;
                if (tempDev != null) {
                    devices.Remove(tempDev);
                }
                OnNewDeviceConnection?.Invoke(dev);
                devices.Add(dev);
            } else if (formatData[0] == "SYNCSET") {
                if (formatData[1] == "info") {
                    SettingsController.load(formatData[2]);
                }
            } else if (formatData[0] == "DEVUPD") {
                string[] formatCommand = new string[formatData.Length - 2];
                for (int i = 0; i < formatCommand.Length; i++) {
                    formatCommand[i] = formatData[i + 2];
                }

                DeviceController device = devices.Find(x => x.id == formatData[1]);
                if (device == null)
                    return;


                if (formatCommand[0] == "REGCOM" || formatCommand[0] == "REGRET") {
                    device.addCommand(formatData);
                }
                else if (formatCommand[0] == "RET") {
                    Command comm = device.commands.Find(x => x.name == formatCommand[1]);
                    if (comm != null)
                    {
                        comm.OnReciveRetData?.Invoke(formatCommand[2]);
                    }
                }

            }
        };
    }

    private static void _listen() {
        byte[] bytes = new byte[1024];
        while (true){
            try{
                int byteTemp = _connection.Receive(bytes);
                string data = Encoding.ASCII.GetString(bytes, 0, byteTemp);
                OnRecieveTcpPackage?.Invoke(data);
            }
            catch (Exception){
                stop();
                break;
            }
        }
    }

    public static bool isConnected() {
        return _connection != null && _connection.Connected;
    }

    public static void send(string _data) {
        _connection.Send(Encoding.ASCII.GetBytes(_data));
    }

    public static void refresh() {
        devices.Clear();
        send("GETDEV");
    }

    public static void stop() {
        _listenThread.Abort();
        _connection.Close();
        _listenThread = null;
        _connection = null;
        OnConnectionEnded?.Invoke();

        Application.Current.MainPage = new MainPage();
    }

}
