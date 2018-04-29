using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Server
{
    class Server
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;
        static int buffer = 0;

        public static void Init()
        {
            try
            {
                Global.thListen = new Thread(new ThreadStart(Start));
                Global.thListen.Start();
                Thread th = new Thread(new ThreadStart(clientInfo));
                th.Start();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Errors.Manage_Errors(ex);
            }
        }
        private static void clientInfo()
        {
            try
            {
                Console.WriteLine("CLIENT PORT: "+ Global.Port_Client);
                Global.clientInfo = new TcpListener(IPAddress.Parse(Global.IPAddress), Global.Port_Client);
                Global.Cinfo = default(TcpClient);
                Global.clientInfo.Start();
                while (true)
                {
                    Global.Cinfo = Global.clientInfo.AcceptTcpClient();
                    buffer = Global.Cinfo.ReceiveBufferSize;
                    byte[] received = new byte[buffer];
                    NetworkStream networkStream = Global.Cinfo.GetStream();
                    networkStream.Read(received, 0, buffer);
                    string Command = Encoding.UTF8.GetString(received);
                    Command = Command.Substring(0, Command.IndexOf("$"));
                    Commands(Command, Global.Cinfo);
                }
            }
            catch(Exception ex)
            {
                Errors.Manage_Errors(ex);
            }
        }
        private static void Commands(string Command, TcpClient client)
        {

        }
        private static void Start()
        {
            try
            {
                Console.WriteLine("Starting listening at port " + Global.Port + "...");
                Global.tcpListener = new TcpListener(IPAddress.Parse(Global.IPAddress), Global.Port);
                Global.tcpClient = default(TcpClient);
                int counter = 0;
                Global.tcpListener.Start();
                Console.WriteLine("Status:       Running!");
                Console.WriteLine("IP ADDRESS:   " + Global.IPAddress);
                Console.WriteLine("SERVER PORT:  " + Global.Port);
                //MSConnection.Open();
                while (true)
                {
                    counter += 1;

                    Global.tcpClient = Global.tcpListener.AcceptTcpClient();
                    Global.BufferSize = Global.tcpClient.ReceiveBufferSize;
                    byte[] received = new byte[Global.BufferSize];
                    string fromClient = null;
                    NetworkStream networkStream = Global.tcpClient.GetStream();
                    networkStream.Read(received, 0, (int)Global.tcpClient.ReceiveBufferSize);
                    fromClient = Encoding.UTF8.GetString(received);
                    fromClient = fromClient.Substring(0, fromClient.IndexOf("$"));


                    if (Global.clientsList.Contains(fromClient))
                    {
                        string response = "Nome de usuario ja esta sendo usado, por favor, escolha outro.";
                        byte[] resp = Encoding.UTF8.GetBytes(response);
                        networkStream.Write(resp, 0, resp.Length);
                        networkStream.Flush();
                        networkStream.Close();
                        Global.tcpClient.Client.Close();
                        Global.tcpClient.Close();
                    }
                    else
                    {
                        Global.clientsList.Add(fromClient, Global.tcpClient);

                        Broadcast(fromClient + " Joined ", fromClient, false);

                        Console.WriteLine(fromClient + " Joined chat room ");
                        Server server = new Server();
                        server.InitializeClient(Global.tcpClient, fromClient);
                    }
                }

                Global.tcpClient.Close();
                Global.tcpListener.Stop();
                Console.WriteLine("exit");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Errors.Manage_Errors(ex);
            }
        }
        public static void Broadcast(string msg, string uName, bool flag)
        {
            foreach (DictionaryEntry Item in Global.clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                if (flag == true)
                {
                    broadcastBytes = Encoding.UTF8.GetBytes(uName + " says : " + msg);
                }
                else
                {
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);
                }

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();

            }
        }
        public static void ServerMessage(string msg)
        {
            foreach (DictionaryEntry Item in Global.clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;
                broadcastBytes = Encoding.UTF8.GetBytes(msg);

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();

            }
        }
        public static void PersonalMessage(string From, string To, string msg)
        {
            if (!Global.clientsList.ContainsKey(To))
            {
                string response = To + " is not connected.";
                TcpClient origin;
                origin = (TcpClient)Global.clientsList[From];
                NetworkStream originStream = origin.GetStream();
                Byte[] message = null;
                message = Encoding.UTF8.GetBytes(response);

                originStream.Write(message, 0, message.Length);
                originStream.Flush();
            }
            else
            {
                foreach (DictionaryEntry Item in Global.clientsList)
                {
                    if (Item.Key.ToString() == To)
                    {
                        TcpClient destiny;
                        destiny = (TcpClient)Item.Value;
                        NetworkStream destinyStream = destiny.GetStream();
                        Byte[] message = null;
                        message = Encoding.UTF8.GetBytes(From + ": " + msg);

                        destinyStream.Write(message, 0, message.Length);
                        destinyStream.Flush();
                    }
                    if (Item.Key.ToString() == From)
                    {
                        TcpClient origin;
                        origin = (TcpClient)Item.Value;
                        NetworkStream originStream = origin.GetStream();
                        Byte[] message = null;
                        message = Encoding.UTF8.GetBytes("You to " + To + ": " + msg);

                        originStream.Write(message, 0, message.Length);
                        originStream.Flush();
                    }

                }
            }
        }
        public void InitializeClient(TcpClient inClientSocket, string clineNo)
        {
            clientSocket = inClientSocket;
            clNo = clineNo;
            //clientsList = cList;
            Thread ctThread = new Thread(InitializeChat);
            ctThread.Start();
        }

        private void InitializeChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[Global.BufferSize];
            string dataFromClient = null;
            string rCount = null;
            requestCount = 0;

            while ((true))
            {
                try
                {
                    if (!isConnected(clientSocket.Client))
                    {
                        string response = clNo + " was disconnected.";
                        ServerMessage(response);
                        clientSocket.Client.Close();
                        clientSocket.Close();
                        Console.WriteLine(clNo + " was disconnected.");
                        Global.clientsList.Remove(clNo);
                        return;
                    }
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine("From client - " + clNo + " : " + dataFromClient);
                    rCount = Convert.ToString(requestCount);
                    if (dataFromClient.Contains("#"))
                    {
                        string To = dataFromClient.Substring(0, dataFromClient.IndexOf("#"));
                        string Message = dataFromClient.Substring(dataFromClient.IndexOf("#")+1);
                        PersonalMessage(clNo, To, Message);
                    }
                    else
                    {
                        Broadcast(dataFromClient, clNo, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //Errors.Manage_Errors(ex);
                    clientSocket.Client.Close();
                    clientSocket.Close();
                    Global.clientsList.Remove(clNo);
                    Console.WriteLine(clNo + " was disconnected.");
                    break;
                }
            }
        }
        private bool isConnected(Socket s)
        {
            bool part1 = s.Poll(0, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            bool part3 = s.Connected;
            if (part1 && part2 || !part3)
                return false;
            else
                return true;
        }
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Manage_Errors(ex);
                return ex.Message;
            }
            return "";
        }
    }
}
