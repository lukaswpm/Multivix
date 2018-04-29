using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Server
{
    class Global
    {
        //Users
        private static string User_Name;
        private static string User_Type;

        public static string Username { get { return User_Name; } set { User_Name = value; } }
        public static string UserType { get { return User_Type; } set { User_Type = value; } }

        //Server
        private static int ServerPort;
        private static int portClient;
        private static string ServerAdrress;
        private static TcpClient client;
        private static TcpClient info;
        private static TcpListener listener;
        private static TcpListener listenC;
        private static Thread thListener;
        private static Hashtable clients;
        private static int Buffer;

        public static int Port { get { return ServerPort; } set { ServerPort = value; } }
        public static int Port_Client { get {return portClient; } set { portClient = value; } }
        public static string IPAddress { get { return ServerAdrress; } set { ServerAdrress = value; } }
        public static TcpClient tcpClient { get { return client; } set { client = value; } }
        public static TcpClient Cinfo { get { return info; } set { info = value; } }
        public static TcpListener tcpListener { get { return listener; } set { listener = value; } }
        public static TcpListener clientInfo { get { return listenC; } set { listenC = value; } }
        public static Thread thListen { get { return thListener; } set { thListener = value; } }
        public static Hashtable clientsList { get { return clients; } set { clients = value; } }
        public static int BufferSize { get { return Buffer; } set { Buffer = value; } }

        //Files
        private static string Error_Log_Path;
        private static MemoryStream msConfigFile;
        public static string Config_File = "config.cfg";

        public static string Log_Path { get { return Error_Log_Path; } set { Error_Log_Path = value; } }
        public static MemoryStream msConfig { get { return msConfigFile; } set { msConfigFile = value; } }

        //Database
        private static MySqlConnection conn;
        private static string strConn;

        public static MySqlConnection connection { get { return conn; } set { conn = value; } }
        public static string strConnection { get { return strConn; } set { strConn = value; } }


        public static string OSVersion()
        {
            var plataformID = Environment.OSVersion.Platform;
            var majorVersion = Environment.OSVersion.Version.Major;
            var minorVersion = Environment.OSVersion.Version.Minor;
            string SOName = "";
            switch (plataformID)
            {
                case PlatformID.Win32Windows:
                    switch (minorVersion)
                    {
                        case 0:
                            SOName = "Windows 95";
                            break;
                        case 10:
                            SOName = "Windows 98";
                            break;
                        case 90:
                            SOName = "Windows Me";
                            break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (majorVersion)
                    {
                        case 4:
                            SOName = "Windows NT 4.0";
                            break;
                        case 5:
                            if (minorVersion == 0)
                                SOName = "Windows 2000";
                            else if (minorVersion == 1)
                                SOName = "Windows XP";
                            else
                                SOName = "Windows 2003";
                            break;
                        case 6:
                            if (minorVersion == 0)
                                SOName = "Windows 2008";
                            else if (minorVersion == 1)
                                SOName = "Windows 7";
                            else if (minorVersion == 2)
                                SOName = "Windows 8";
                            else if (minorVersion == 3)
                                SOName = "Windows 8.1";
                            break;
                        case 10:
                            SOName = "Windows 10";
                            break;
                    }
                    break;
                default:
                    return Environment.OSVersion.VersionString;
            }
            return SOName;
        }
    }
}
