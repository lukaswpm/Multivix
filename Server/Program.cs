using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings.Load();
            Server.Init();
            /*MSConnection.Open();
            Console.WriteLine(MSConnection.Connection_State());
            MSConnection.Close();
            Console.WriteLine(MSConnection.Connection_State());*/
            string Exit = Console.ReadLine();
            while (Exit == null || Exit == "");
        }
    }
}
