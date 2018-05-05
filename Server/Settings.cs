using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

namespace Server
{
    class Settings
    {
        public static void Load()
        {
            InitDefines();
        }
        private static void InitDefines()
        {
            Global.Log_Path = "./Logs";
            Global.Port = 7777;
            Global.Port_Client = 9988;
            Global.IPAddress = Server.GetLocalIPAddress();
            Global.strConnection = "Server=" + Global.IPAddress + "; User ID=lucas; Password=998877exe; Port=3306; Database=chatserver; Pooling=false;";
            Global.clientsList = new Hashtable();
            Global.mutex = new Mutex();
        }
    }
}
