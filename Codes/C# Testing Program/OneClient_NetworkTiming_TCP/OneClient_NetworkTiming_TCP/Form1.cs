using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using HighResTimer;


namespace OneClient_NetworkTiming_TCP
{


    public partial class Form1 : Form
    {
        
        Decimal datapoints;
        StringBuilder sb = new StringBuilder();
        SerialPort serialPortIn = new SerialPort();
        TcpClient tcpclnt;
        UdpClient udpclnt;
        IPEndPoint ep;
        Timing mytimer = new Timing();
        
        int baudrate; //9600;
       
        Thread sendThread;
        Thread recvThread;
        ThreadStart sending;
        ThreadStart receiving;
        NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
        }



        private void startbutton_Click(object sender, EventArgs e)
        {
            comPortIn.Enabled = false;
            startbutton.Enabled = false;
            
            
            if (serialPortIn.IsOpen) { serialPortIn.Close(); }
            if (checkserial.Checked)
            {
                
                serialPortIn.PortName = comPortIn.SelectedItem.ToString();
                baudrate = Int32.Parse(this.baudratebox.SelectedItem.ToString());
                serialPortIn.BaudRate = baudrate; //was 9600
                serialPortIn.Open();
                serialPortIn.DiscardInBuffer();
            }
            

            //set priority to high
            // Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            //Thread.CurrentThread.Priority = ThreadPriority.Highest;



            //clear stringbuilder
            sb.Clear();
            Random seed = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Byte[] bb = new byte[1]; //1 byte of data coming in

            
            //serialPortIn.DiscardOutBuffer();

            datapoints = numericUpDown1.Value;
            

            if (radiotcp.Checked)
            {
                tcpclnt = new TcpClient();
                textBox1.AppendText("TCP Connecting... \n");
                tcpclnt.Connect("192.168.137.99", 8888); //address of RPi on arbitrary non privileged port
                textBox1.AppendText("TCP Connected \n");
                stream = tcpclnt.GetStream();
                int bytes = stream.Read(bb, 0, bb.Length);

            }
            else
            {
                try
                {
                    udpclnt = new UdpClient();
                    ep = new IPEndPoint(IPAddress.Parse("192.168.137.99"), 8888);
                    textBox1.AppendText("UDP Connecting... \n");

                    udpclnt.Connect(ep);
                    textBox1.AppendText("UDP Connected \n");
                    byte[] writebyte = BitConverter.GetBytes(99);

                    

                    udpclnt.Send(writebyte, writebyte.Length);
                    bb = udpclnt.Receive(ref ep);
                    
                }
                catch

                {
                    return;
                }
                
            }
            //intialize network connections

            /*Receive the welcome from server */
           

            mytimer.Start();


            int numback = bb[0];
            textBox1.AppendText("Received initial message from server: " + bb[0] + "\n");

            textBox1.AppendText("Warmup \n");
            while (mytimer.Duration *1000 < 1500)
            {

            }
           

            textBox1.AppendText("Beginning Testing");
            textBox1.AppendText(Environment.NewLine);


            ThreadProgram clientObject = new ThreadProgram(udpclnt, ep, stream, mytimer, datapoints, sb, serialPortIn);
            
            if (radiotcp.Checked)
            {
                sending = new ThreadStart(clientObject.tcpsendData);
                if (checkserial.Checked)
                {
                    
                    receiving = new ThreadStart(clientObject.tcprecvserData);
                }
                else
                {
                    receiving = new ThreadStart(clientObject.tcprecvData);
                }
            }
            else
            {
                sending = new ThreadStart(clientObject.udpsendData);
                if (checkserial.Checked)
                {
                    receiving = new ThreadStart(clientObject.udprecvserData);
                }
                else
                {
                    receiving = new ThreadStart(clientObject.udprecvData);
                }
            }

            sendThread = new Thread(sending);
            recvThread = new Thread(receiving);
            recvThread.Start();
            sendThread.Start();
            

            sendThread.Join();
            recvThread.Join();
            if (radiotcp.Checked)
            { tcpclnt.Close(); }
            else
            {
                udpclnt.Close();
            }
                comPortIn.Enabled = true;
            startbutton.Enabled = true; // Diable the stop button 
          
            textBox1.Clear();
            // Close Com ports 
            if (serialPortIn.IsOpen) { serialPortIn.Close(); }
            this.Invoke(new EventHandler(SaveDialog));


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            baudratebox.Items.Add("9600");
            baudratebox.Items.Add("115200");
            baudratebox.SelectedIndex = 0;


            comPortIn.Enabled = false;

        }
        public void SaveDialog(object sender, EventArgs e)
        { /// When the timer runs out or STOP is pressed, a Save Dialog appears 
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (radiotcp.Checked)
            {
                if (checkserial.Checked)
                {
                    saveFileDialog1.FileName = "DeviceTest_eth-serTCP_10";
                }
                else
                {
                    saveFileDialog1.FileName = "DeviceTest_eth-ethTCP_10";
                }
                
            }
            else
            {
                if (checkserial.Checked)
                {
                    saveFileDialog1.FileName = "DeviceTest_eth-serUDP_10";
                }
                else
                {
                    saveFileDialog1.FileName = "DeviceTest_eth-ethUDP_10";
                }
                
            }
               
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            { File.WriteAllText(saveFileDialog1.FileName, sb.ToString()); }
        }

        private void checkserial_CheckedChanged(object sender, EventArgs e)
        {
            if (checkserial.Checked)
            {
                List<String> tList = new List<String>();
                comPortIn.Items.Clear();
                foreach (string s in SerialPort.GetPortNames())
                {
                    tList.Add(s);
                }
                tList.Sort();

                comPortIn.Items.AddRange(tList.ToArray());
                comPortIn.SelectedIndex = 0;
                comPortIn.Enabled = false;
                comPortIn.Enabled = true;
            }
            else
            {
                comPortIn.Enabled = false;
            }
        }
        public class ThreadProgram
        {
            public System.Object lockThis = new System.Object();
            public NetworkStream stream;
            public UdpClient udpin;
            public IPEndPoint ep;
            private static Mutex mut = new Mutex();
            int num2send = 1;
            Decimal datapoints;
            int numback;
            bool received = true;
            byte[] recvbyte = new byte[1];
            byte[] sentbyte = new byte[1];
            int counter = 0;
          
            StringBuilder sb;
            int correctbyte=0;
            
            SerialPort serialPortIn;
            
            
            Timing mytimer = new Timing();
            double sentelapsmil;
            double recvelapsmil;
            double elapsmil;
           
            public ThreadProgram(UdpClient udp, IPEndPoint epin, NetworkStream streamer, Timing timer, Decimal thedatapoints, StringBuilder stringb, SerialPort serialPort)
            {
                stream = streamer;
               
                sb = stringb;
                datapoints = thedatapoints;
             
                serialPortIn = serialPort;
                
               
                mytimer = timer;
                udpin = udp;
                ep = epin;
                
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
                
            }

            public void tcprecvserData()
            {
                
                
                for (int repeat = 0; repeat < datapoints; repeat++)
                {


                    //1 byte of data coming in

                    numback = serialPortIn.ReadByte();
                   
                    recvelapsmil = mytimer.Duration;

                    
                    
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
                  
                     sb.AppendLine(counter+"\t"+elapsmil*1000 + "\t" + correctbyte);
                    
                    mut.WaitOne();
                    num2send += 1;
                    if (num2send > 255) { num2send = 1; }
                    mut.ReleaseMutex();
                    received = true;

                }
            }
            public void tcprecvData()
            {
                
                
                for (int repeat = 0; repeat < datapoints; repeat++)
                {
                   

                    stream.Read(recvbyte, 0, 1);
                   
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
            }
            public void udpsendData()
            {
                
                for (int repeat = 0; repeat < datapoints; repeat++)
                {
                   

                    while (!received)
                    {

                    }
                    received = false;

                   
                    mut.WaitOne();

                    sentbyte =  BitConverter.GetBytes(num2send);
                    mut.ReleaseMutex();
                 
                    
                    udpin.Send(sentbyte, 1);

                   
                    sentelapsmil = mytimer.Duration;
                   


                }
                
            }

            public void udprecvserData()
            {
              
                for (int repeat = 0; repeat < datapoints; repeat++)
                {

                    numback = serialPortIn.ReadByte();
         
                   recvelapsmil = mytimer.Duration;
                                                      
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
            }
            public void udprecvData()
            {
                
                for (int repeat = 0; repeat < datapoints; repeat++)
                {                

                    recvbyte= udpin.Receive(ref ep);
                   
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
            }
        }

    
    }
}