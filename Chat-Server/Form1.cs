using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Server_Chat
{
    public partial class Form1 : Form
    {
        Socket SocServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket SocClient = null;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        public void GetMsg()
        {
            try
            {
                while (true)
                {
                    byte[] barray = new byte[1024];
                    int RecB = SocClient.Receive(barray);
                    if (RecB > 0)
                    {
                        ListMessages.Items.Add("Cilent: " + Encoding.Unicode.GetString(barray, 0, RecB));
                    }
                }
            }
            catch
            {
                ;
            }
        }

        public void StartServer()
        {
            IPEndPoint ipendpointserver = new IPEndPoint(IPAddress.Any, int.Parse(txtPort.Text));
            SocServer.Bind(ipendpointserver);
            SocServer.Listen(1);
            MessageBox.Show("Start server");
            SocClient = SocServer.Accept();
            Thread trGetMsg = new Thread(new ThreadStart(GetMsg));
            trGetMsg.Start();
        }

        private void SbynStart_Click(object sender, EventArgs e)
        {
            Thread TrStart = new Thread(new ThreadStart(StartServer));
            TrStart.Start();

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (SocClient != null)
                {
                    SocClient.Shutdown(SocketShutdown.Both);
                }
                if (SocServer != null)
                {
                    SocServer.Shutdown(SocketShutdown.Both);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] barray = new byte[1024];
            barray = Encoding.Unicode.GetBytes(txtMessage.Text);
            SocClient.Send(barray);
            ListMessages.Items.Add("Server: " + txtMessage.Text);

            txtMessage.Clear();
        }
    }
}
