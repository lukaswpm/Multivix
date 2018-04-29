using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Communication
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket;

        NetworkStream serverStream = default(NetworkStream);

        string readData = null;
        string Messages = "";

        Thread ctThread;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new TcpClient();
                clientSocket.Connect(txtServer.Text, Convert.ToInt32(txtPort.Text));

                serverStream = clientSocket.GetStream();

                byte[] outStream = Encoding.UTF8.GetBytes(txtNickname.Text + "$");

                serverStream.Write(outStream, 0, outStream.Length);

                serverStream.Flush();
                readData = "Conected to Chat Server ...";
                lblStatus.Text = readData;
                msg();

                ctThread = new Thread(getMessage);
                ctThread.Start();
            }
            catch (Exception ex)
            {
                clientSocket.Close();
                //serverStream.Close();
                Messages = "Disconnected by server";
                richTextBox3.Text += Messages + Environment.NewLine;
                MessageBox.Show(ex.Message);
            }
        }
        private void getMessage()

        {
            try
            {
                while (true)

                {
                    serverStream = clientSocket.GetStream();

                    int buffSize = 0;

                    buffSize = clientSocket.ReceiveBufferSize;

                    byte[] inStream = new byte[buffSize];

                    serverStream.Read(inStream, 0, buffSize);

                    string returndata = Encoding.UTF8.GetString(inStream);

                    readData = "" + returndata + "\r\n";

                    msg();
                    Thread thread = new Thread(new ThreadStart(THREAD_LOG));
                    if (clientSocket.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (clientSocket.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            clientSocket.Close();
                            serverStream.Close();
                            Messages = "Disconnected by server";
                            thread.Start();
                            return;
                        }
                    }
                    Messages = "Conected";
                    thread.Start();

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }
        delegate void Log_Error();
        private void THREAD_LOG()
        {
            if (richTextBox3.InvokeRequired)
            {
                Log_Error error = new Log_Error(Log);
                this.Invoke(error);
            }
        }
        private void Log()
        {
            richTextBox3.Text = Messages + Environment.NewLine;
        }


        private void msg()
        {
            if (this.InvokeRequired)

                this.Invoke(new MethodInvoker(msg));

            else

                richTextBox2.Text += Environment.NewLine + " >> " + readData;

        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] outStream = Encoding.UTF8.GetBytes(richTextBox1.Text + "$");

                serverStream.Write(outStream, 0, outStream.Length);

                serverStream.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientSocket.Client.Disconnect(true);
            //clientSocket.Client.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] outStream = Encoding.UTF8.GetBytes(txtTo.Text + "#" + txtMsg.Text + "$");

                serverStream.Write(outStream, 0, outStream.Length);

                serverStream.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
