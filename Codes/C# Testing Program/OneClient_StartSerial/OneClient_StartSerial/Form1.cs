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


namespace OneClient_StartSerial
{
    public partial class Form1 : Form
    {
        Stopwatch stopwatch = new Stopwatch();
        Decimal datapoints;
        
        StringBuilder sb = new StringBuilder();
        SerialPort serialPortOut = new SerialPort();
        SerialPort serialPortIn = new SerialPort();
        TcpClient tcpclnt;
        UdpClient udpclnt;
        IPEndPoint ep;
        Timing mytimer = new Timing();

        int baudrate;
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
            if (radiotcp.Checked)
            {
                radioudp.Enabled = false;
                radioserial.Enabled = false;
            }
            else if (radioudp.Checked)
            {
                radioserial.Enabled = false;
                radiotcp.Enabled = false;
            }
            else
            {
                radiotcp.Enabled = false;
                radioudp.Enabled = false;
            }
            comPortOut.Enabled = false;
            startbutton.Enabled = false;
            
            if (serialPortOut.IsOpen) { serialPortOut.Close(); }
            if (serialPortIn.IsOpen) { serialPortIn.Close(); }
            
            baudrate = Int32.Parse(this.baudratebox.SelectedItem.ToString());
              serialPortOut.PortName = comPortOut.SelectedItem.ToString();
            serialPortOut.BaudRate =  baudrate;//
            serialPortOut.Open();
                serialPortOut.DiscardOutBuffer();
            if (radioserial.Checked)
            {
                serialPortIn.PortName = comPortIn.SelectedItem.ToString();
                serialPortIn.BaudRate = baudrate;//
                serialPortIn.Open();
                serialPortIn.DiscardInBuffer();
               
            }
           
           

            //set priority to high
            // Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            //Thread.CurrentThread.Priority = ThreadPriority.Highest;



            //clear stringbuilder
            sb.Clear();
           
            Byte[] bb = new byte[1]; //1 byte of data coming in


            //serialPortOut.DiscardOutBuffer();

            datapoints = numericUpDown1.Value;

            //non loop format - for cppserv


            if (radiotcp.Checked)
            {
                tcpclnt = new TcpClient();
                textBox1.AppendText("TCP Connecting... \n");
                tcpclnt.Connect("192.168.137.99", 8888); //address of RPi on arbitrary non privileged port
                textBox1.AppendText("TCP Connected \n");
                stream = tcpclnt.GetStream();
                int bytes = stream.Read(bb, 0, 1);
                

                

            }
            else if (radioudp.Checked)
            {
                try
                {
                    udpclnt = new UdpClient();
                    ep = new IPEndPoint(IPAddress.Parse("192.168.137.99"), 8888);
                    textBox1.AppendText("UDP Connecting... \n");

                    udpclnt.Connect(ep);
                    textBox1.AppendText("UDP Connected \n");
                    byte[] writebyte = BitConverter.GetBytes(1);

                    

                    udpclnt.Send(writebyte, writebyte.Length);
                    bb = udpclnt.Receive(ref ep);

        

                }
                catch

                {
                    return;
                }

            }
            else
            {
            }
            //intialize network connections

            /*Receive the welcome from server */


            mytimer.Start();
            
            int numback = bb[0];
            textBox1.AppendText("Received initial message from server: " + bb[0] + "\n");

            textBox1.AppendText("Warmup \n");
            if (baudrate == 9600)
            {
                while (mytimer.Duration * 1000 < 1500)
                {

                }
            }
          /*  stopwatch.Restart();
           // while (stopwatch.ElapsedMilliseconds < 1500)
            {
            }
            stopwatch.Stop();*/

            textBox1.AppendText("Beginning Testing");
            textBox1.AppendText(Environment.NewLine);


            ThreadProgram clientObject = new ThreadProgram(udpclnt, ep, stream, mytimer, datapoints, sb, serialPortOut, serialPortIn, radiotcp);
            
            if (radiotcp.Checked)
            {

                if (checkser1.Checked)
                {
                    
                    // receiving = new ThreadStart(clientObject.ser1portrecvData);
                    receiving = new ThreadStart(clientObject.sersendrecvData);
                }
                else
                {
                    sending = new ThreadStart(clientObject.sersendData);
                    receiving = new ThreadStart(clientObject.tcprecvData);
                    sendThread = new Thread(sending);
                }

                    
               
            }
            else if (radioudp.Checked)
            {

                if (checkser1.Checked)
                {
                    sending = new ThreadStart(clientObject.sersendData);
                    receiving = new ThreadStart(clientObject.ser1portrecvData);
                    //receiving = new ThreadStart(clientObject.sersendrecvData);
                    sendThread = new Thread(sending);
                }
                else
                {
                    sending = new ThreadStart(clientObject.sersendData);
                    receiving = new ThreadStart(clientObject.udprecvData);
                    sendThread = new Thread(sending);
                }
                
            }
            else if (radioserial.Checked)

            {
               
                    sending = new ThreadStart(clientObject.sersendData);
                    receiving = new ThreadStart(clientObject.serrecvData);
                    sendThread = new Thread(sending);
                
            }
        
            
            
            recvThread = new Thread(receiving);
            recvThread.Start();
            if (!checkser1.Checked || radioserial.Checked)
            {  }
            sendThread.Start();

            if (!checkser1.Checked || radioserial.Checked)
            {
               
            }
            sendThread.Join();
            recvThread.Join();
            if (radiotcp.Checked)
            { tcpclnt.Close(); }
            else if (radioudp.Checked)
            {
                udpclnt.Close();
            }
            else
            {
                
                serialPortIn.Close();
            }
            comPortOut.Enabled = true;
            startbutton.Enabled = true;
            radiotcp.Enabled = true;
            radioudp.Enabled = true;
            radioserial.Enabled = true;
            textBox1.Clear();
            // Close Com ports 
            if (serialPortOut.IsOpen) { serialPortOut.Close(); }
            this.Invoke(new EventHandler(SaveDialog));
        }

        private void radioserial_CheckedChanged(object sender, EventArgs e)
        {
            if (radioserial.Checked)
            {
                comPortIn.Enabled = true;
                List<String> tList = new List<String>();
                comPortIn.Items.Clear();
                foreach (string s in SerialPort.GetPortNames())
                {
                    tList.Add(s);
                }
                tList.Sort();

                comPortIn.Items.AddRange(tList.ToArray());
                comPortIn.SelectedIndex = 0;
              


            }
            else
            {
                comPortIn.Enabled = false;
            }
        }
        public void SaveDialog(object sender, EventArgs e)
        { /// When the timer runs out or STOP is pressed, a Save Dialog appears 
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (radiotcp.Checked)
            {             
                saveFileDialog1.FileName = "DeviceTest_ser-ethTCP_10";                                            
            }
            else
            {
                saveFileDialog1.FileName = "DeviceTest_ser-ethUDP_10";              
            }

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            { File.WriteAllText(saveFileDialog1.FileName, sb.ToString()); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<String> tList = new List<String>();
            comPortOut.Items.Clear();
            foreach (string s in SerialPort.GetPortNames())
            {
                tList.Add(s);
            }
            tList.Sort();

            comPortOut.Items.AddRange(tList.ToArray());
            comPortOut.SelectedIndex = 0;
            comPortIn.Enabled = false;
            comPortOut.Enabled = true;

            baudratebox.Items.Add("9600");
            baudratebox.Items.Add("115200");
            baudratebox.SelectedIndex = 0;
            
        }

        public class ThreadProgram
        {
            public System.Object lockThis = new System.Object();
            public NetworkStream stream;
            public UdpClient udpin;
            public IPEndPoint ep;
            private static Mutex mut = new Mutex();
            Decimal datapoints;
            bool received = true;
            bool sent = false;
            int num2send=1;
            int numback;
            StringBuilder sb;
            int correctbyte = 0;
            int counter = 0;
            SerialPort serialPortOut;
            SerialPort serialPortIn;
            byte[] sentbyte = new byte[1];
            byte[] recvbyte = new byte[64];
            byte[] ackbyte = new byte[64];
           
            RadioButton radiotcp;
            Timing mytimer = new Timing();
            double sent1;
            double sentelapsmil;
            double recv1;
            double recvelapsmil;
            double elapsmil;
           
            public ThreadProgram(UdpClient udp, IPEndPoint epin, NetworkStream streamer, Timing timer, Decimal thedatapoints, StringBuilder stringb, SerialPort serialPort, SerialPort serialPort2, RadioButton radio_tcp)
            {
                stream = streamer;
               
                sb = stringb;
                datapoints = thedatapoints;
                
                serialPortOut = serialPort;
                radiotcp = radio_tcp;
                mytimer = timer;
                udpin = udp;
                ep = epin;
                serialPortIn = serialPort2;
                
            }

            public void sersendData()
            {
               /* if (serialPortOut.BaudRate == 115200)
                {
                    while (mytimer.Duration * 1000 < 1500)
                    {

                    }
                }*/
                for (int repeat = 0; repeat < datapoints; repeat++)
                {
                   
                    while (!received)
                    {

                    }
                    received = false;

                    mut.WaitOne();
                    sentbyte = BitConverter.GetBytes(num2send);
                    mut.ReleaseMutex();

                    
                        
                        if (radiotcp.Checked) {  }
                        //serialPortOut.DiscardOutBuffer();

                        serialPortOut.Write(sentbyte, 0, 1);
                       
                    
                     //Send the byte
                                                          //stopwatch.Reset();

                  
                  
                    sentelapsmil = mytimer.Duration;
                    sent = true;


                }
                
            }

            public void sersendrecvData()
            {
                

                if (serialPortOut.BaudRate == 115200)
                {
                    while (mytimer.Duration * 1000 < 1500)
                    {

                    }
                }
                for (int repeat = 0; repeat < datapoints; repeat++)
                {

                
                   
                    sentbyte = BitConverter.GetBytes(num2send);
                    

                    if (radiotcp.Checked) { serialPortOut.Write(sentbyte, 0, 1); }
                    else
                    {
                        serialPortOut.Write(sentbyte, 0, 1);
                        //serialPortOut.DiscardOutBuffer();


                    }
                    //Send the byte
                    //stopwatch.Reset();



                    sentelapsmil = mytimer.Duration;

                    numback = serialPortOut.ReadByte();
                    recvelapsmil = mytimer.Duration;
                    serialPortOut.DiscardInBuffer();


                    
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
                    sb.AppendLine(counter + "\t" + elapsmil * 1000 + "\t" + correctbyte);
                    //textbox1.AppendText("Received after:" + elapsedmilli + "\t" + correctbyte+"\n");

                    num2send += 1;
                    if (num2send > 255) { num2send = 1; }
                    
                

                  
                   
                    

                }

            }
           
            public void tcprecvData()
            {
                ackbyte = BitConverter.GetBytes(0);
               
                //ackbyte = BitConverter.GetBytes(0);
                for (int repeat = 0; repeat < datapoints; repeat++)
                {
                    

                    
                    //stream.ReadAsync(recvbyte, 0, 1);
                    
                    stream.Read(recvbyte, 0, 1);
                    stream.Write(ackbyte, 0, 1);
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
                    sb.AppendLine(counter + "\t" + elapsmil*1000+ "\t" + correctbyte);

                    mut.WaitOne();
                    num2send += 1;
                    if (num2send > 255) { num2send = 1; }
                    mut.ReleaseMutex();
                    received = true;
                }
            }
            public void serrecvData()
            {
                
                for (int repeat = 0; repeat < datapoints; repeat++)
                {

                    while (sent==false)
                    { }

                    recv1 = mytimer.Duration;
                      
                        numback = serialPortIn.ReadByte();

                        
          
                    recvelapsmil = mytimer.Duration;
                    sent = false;
                    
                    
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
                    sb.AppendLine(counter + "\t" + elapsmil*1000 + "\t"+ correctbyte);

                    //textbox1.AppendText("Received after:" + elapsedmilli + "\t" + correctbyte+"\n");
                    mut.WaitOne();
                    num2send += 1;
                    if (num2send > 255) { num2send = 1; }
                    mut.ReleaseMutex();
                    received = true;
                }
                //Thread.Sleep(100);
                // recvloop = false;
            }
            public void serrecv2Data()
            {

                for (int repeat = 0; repeat < datapoints; repeat++)
                {

                    for (int i = 0; i < 2; i++)
                    {
                    numback = serialPortIn.ReadByte();
                 
                    }
                     
                        
                    
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
                    sb.AppendLine(counter + "\t" + elapsmil * 1000 + "\t" + correctbyte);
                    //textbox1.AppendText("Received after:" + elapsedmilli + "\t" + correctbyte+"\n");
                    mut.WaitOne();
                    num2send += 1;
                    if (num2send > 255) { num2send = 1; }
                    mut.ReleaseMutex();
                    received = true;
                }
                //Thread.Sleep(100);
                // recvloop = false;
            }
            public void ser1portrecvData()
            {
                
                for (int repeat = 0; repeat < datapoints; repeat++)
                {

                    serialPortOut.DiscardInBuffer();


                    numback = serialPortOut.ReadByte();
                    recvelapsmil = mytimer.Duration;
                 //   serialPortOut.DiscardInBuffer();
                 //  serialPortOut.DiscardOutBuffer();
                   

                   



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
                    sb.AppendLine(counter + "\t" + elapsmil * 1000 + "\t" + correctbyte);
                    //textbox1.AppendText("Received after:" + elapsedmilli + "\t" + correctbyte+"\n");
                    mut.WaitOne();
                    num2send += 1;
                    if (num2send > 255) { num2send = 1; }
                   
                    mut.ReleaseMutex();
                    Thread.Sleep(10);
                    received = true;
                }
                //Thread.Sleep(100);
                // recvloop = false;
            }
            public void udprecvData()
            {
                   
                for (int repeat = 0; repeat < datapoints; repeat++)
                {


                   
                   recvbyte = udpin.Receive(ref ep);

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
                    sb.AppendLine(counter + "\t" + elapsmil * 1000 + "\t" + correctbyte);
                    //textbox1.AppendText("Received after:" + elapsedmilli + "\t" + correctbyte+"\n");
                    received = true;
                }
            }
        }

        
    }
}
