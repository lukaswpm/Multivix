using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Communication
{
    class StartClient
    {
        TcpClient clientSocket;

        NetworkStream serverStream = default(NetworkStream);

        Thread ctThread;

        string commands;

        public void ConnectClient(string host, int port)
        {
            try
            {
                ServerInfo serverInfo = new ServerInfo()
                {
                    Host = host,
                    PortClient = port
                };
                clientSocket = new TcpClient();
                clientSocket.Connect(serverInfo.Host, serverInfo.PortClient);
                commands = "getClients$";
                Thread sendCommand = new Thread(Commands);
                sendCommand.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ConnectChat()
        {
            try
            {

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Commands()
        {
            try
            {
                //Send Command
                serverStream = clientSocket.GetStream();
                byte[] command = Encoding.UTF8.GetBytes(commands);
                serverStream.Write(command, 0, command.Length);
                serverStream.Flush();

                //Receive command
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
