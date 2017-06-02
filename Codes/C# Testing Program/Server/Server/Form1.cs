using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HighResTimer;
using System.IO;

namespace Server
{
    public partial class Form1 : Form
    {
        Decimal datapoints;
        NetworkStream stream;
        Thread sendThread;
        Thread recvThread;
        ThreadStart sending;
        ThreadStart receiving;
        TcpClient tcpclnt;
        ThreadProgram clientObject;
        StringBuilder sb = new StringBuilder();
        TcpListener myList;
        UdpClient listener;
        IPEndPoint groupep;

        IPEndPoint ep;
        Timing mytimer = new Timing();

        public Form1()
        {
            InitializeComponent();
        }

        private void startbutton_Click(object sender, EventArgs e)
        {
            datapoints = datapts.Value;
            sb.Clear();
            mytimer.Start();
            if (tcpradio.Checked)
            {
                IPAddress ipAd = IPAddress.Parse("192.168.137.1");
                // use local m/c IP address, and 
                // use the same in the client

                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, 8001);

                /* Start Listeneting at the specified port */
                myList.Start();
                tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect("192.168.137.1", 8001);

                Socket s = myList.AcceptSocket();
                stream = tcpclnt.GetStream();
                clientObject = new ThreadProgram(listener, s, myList, tcpclnt, groupep, ep, stream, mytimer, datapoints, sb);
            }
            else
            {
                IPEndPoint groupep = new IPEndPoint(IPAddress.Any, 11000);
                UdpClient listener = new UdpClient();
                // udpclient.ExclusiveAddressUse = false;
                // udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //listener.Client.Bind(listener);

                listener.Client.Bind(groupep);
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

                IPAddress broadcast = IPAddress.Parse("127.0.0.1");
                IPEndPoint ep = new IPEndPoint(broadcast, 11000);

                //  udpserver.ExclusiveAddressUse = false;
                //  udpserver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //IPEndPoint epin2 = new IPEndPoint(IPAddress.Parse("192.168.0.101"), 5678);
                //udpserver.Client.Bind(epin2);// was epin and ipaddress parse 192.168.137.1, 1234 (after change in network adapter settings)
                clientObject = new ThreadProgram(listener, s, myList, tcpclnt, groupep, ep, stream, mytimer, datapoints, sb);
            }


            if (tcpradio.Checked)
            {
                sending = new ThreadStart(clientObject.tcpsendData);
                receiving = new ThreadStart(clientObject.tcprecvData);

            }
            else
            {
                //while (mytimer.Duration * 1000 < 1500)
               // { }
                sending = new ThreadStart(clientObject.udpsendData);
                receiving = new ThreadStart(clientObject.udprecvData);
            }

            sendThread = new Thread(sending);
            recvThread = new Thread(receiving);
            recvThread.Start();
            sendThread.Start();


            sendThread.Join();
            recvThread.Join();
            this.Invoke(new EventHandler(SaveDialog));

        }
        public void SaveDialog(object sender, EventArgs e)
        { /// When the timer runs out or STOP is pressed, a Save Dialog appears 
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (tcpradio.Checked)
            {

                saveFileDialog1.FileName = "Comp_hosteth-ethTCP_10_";



            }
            else
            {
                saveFileDialog1.FileName = "Comp_eth-ethUDP_10_";



            }

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            { File.WriteAllText(saveFileDialog1.FileName, sb.ToString()); }
        }

        public class ThreadProgram
        {
            public System.Object lockThis = new System.Object();
            public NetworkStream stream;
            Socket s;
            private static Mutex mut = new Mutex();
            int num2send = 1;
            Decimal datapoints;
            int numback;
            bool received = true;
            byte[] recvbyte = new byte[1];
            byte[] sentbyte = new byte[1];
            int counter = 0;
            IPEndPoint listenep;
            IPEndPoint epbroad;
            UdpClient listener;
            TcpClient tcpclnt;
            TcpListener myList;
            StringBuilder sb;
            int correctbyte = 0;

            Timing mytimer = new Timing();
            double sentelapsmil;
            double recvelapsmil;
            double elapsmil;

            public ThreadProgram(UdpClient client, Socket sender, TcpListener tcplist, TcpClient tcpclient, IPEndPoint ep1, IPEndPoint ep2, NetworkStream streamer, Timing timer, Decimal thedatapoints, StringBuilder sbin)
            {
                stream = streamer;


                datapoints = thedatapoints;
                listenep = ep1;
                epbroad = ep2;
                listener = client;
                s = sender;
                myList = tcplist;
                tcpclnt = tcpclient;
                sb = sbin;
                mytimer = timer;


            }

            public void tcpsendData()
            {


                for (int repeat = 0; repeat < datapoints; repeat++)
                {


                    while (!received)
                    {

                    }
                    received = false;

                  

                    mut.WaitOne();

                    sentbyte = BitConverter.GetBytes(num2send);
                    mut.ReleaseMutex();



                    stream.Write(sentbyte, 0, 1);

                    sentelapsmil = mytimer.Duration;
                  

                }
                tcpclnt.Close();
            }
            public void tcprecvData()
            {


                for (int repeat = 0; repeat < datapoints; repeat++)
                {


                    s.Receive(recvbyte);

                    recvelapsmil = mytimer.Duration;

                    numback = recvbyte[0];

                    if (numback == num2send)
                    {
                        correctbyte = 1;
                    }
                    else
                    {
                        correctbyte = 0;
                    }
                   
                    elapsmil = recvelapsmil - sentelapsmil;
                    counter++;
                    sb.AppendLine(counter + "\t" + elapsmil * 1000 + "\t" + correctbyte);
                   
                    mut.WaitOne();
                    num2send += 1;
                    if (num2send > 255) { num2send = 1; }
                    mut.ReleaseMutex();
                    received = true;
                }
                s.Close();
                myList.Stop();
            }
            public void udpsendData()
            {


                for (int repeat = 0; repeat < datapoints; repeat++)
                {
                   =

                    while (!received)
                    {

                    }
                    received = false;

                  
                    mut.WaitOne();

                    sentbyte = BitConverter.GetBytes(num2send);
                    mut.ReleaseMutex();
                    // stopwatch.Reset();

                    s.SendTo(sentbyte, epbroad);
                   

                    
                    sentelapsmil = mytimer.Duration;
                    /


                }
                s.Close();
            }
            public void udprecvData()
            {


                for (int repeat = 0; repeat < datapoints; repeat++)
                {


                    recvbyte = listener.Receive(ref listenep);

                    
                    recvelapsmil = mytimer.Duration;

                    numback = recvbyte[0];

                    if (numback == num2send)
                    {
                        correctbyte = 1;
                    }
                    else
                    {
                        correctbyte = 0;
                    }
                    // sb.AppendLine(elapsedmilli + "\t" + correctbyte + "\n");

                    elapsmil = recvelapsmil - sentelapsmil;
                    counter++;
                    //sb.Append("\t" + recvmil);
                    sb.AppendLine(counter + "\t" + elapsmil * 1000 + "\t" + correctbyte);

                 
                    mut.WaitOne();
                    num2send += 1;
                    if (num2send > 255) { num2send = 1; }
                    mut.ReleaseMutex();
                    received = true;
                }

                listener.Close();
            }
        }
    }
}
