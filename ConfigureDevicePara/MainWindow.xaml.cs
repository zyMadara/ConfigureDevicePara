using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Windows.Threading;
using System.Net;

namespace ConfigureDevicePara
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort serialPort = new SerialPort();
        private bool OfforOn = false;
        private DispatcherTimer clockTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            ComBoxPort.ItemsSource = SerialPort.GetPortNames();

            //InitStartTimer();
        }


        //private void RealTime(object sender, EventArgs e)
        //{
        //    string year;
        //    string month;
        //    string day;
        //    string hour;
        //    string minute;
        //    string second;

        //    year = System.DateTime.Now.Year.ToString();
        //    month = System.DateTime.Now.Month.ToString();
        //    day = System.DateTime.Now.Day.ToString();
        //    hour = System.DateTime.Now.Hour.ToString();
        //    minute = System.DateTime.Now.Minute.ToString();
        //    second = System.DateTime.Now.Second.ToString();

        //    if (month.Length == 1)
        //    {
        //        month = "0" + month;
        //    }
        //    if (day.Length == 1)
        //    {
        //        day = "0" + day;
        //    }
        //    if (hour.Length == 1)
        //    {
        //        hour = "0" + hour;
        //    }
        //    if (minute.Length == 1)
        //    {
        //        minute = "0" + minute;
        //    }
        //    if (second.Length == 1)
        //    {
        //        second = "0" + second;
        //    }

        //    //TextBoxSend.Text = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
        //}

        //private void InitStartTimer()
        //{
        //    clockTimer.IsEnabled = true;
        //    clockTimer.Interval = TimeSpan.FromSeconds(1);
        //    clockTimer.Tick += RealTime;
        //    clockTimer.Start();
        //}

        private void Btn_Open_Click(object sender, RoutedEventArgs e)
        {
            if (OfforOn == false)
            {
                int baud = 0;

                int.TryParse(ComBoxBaudRate.Text, out baud);

                serialPort.PortName = ComBoxPort.Text;
                serialPort.BaudRate = baud;
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;

                try
                {
                    serialPort.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                OfforOn = true;
                btn_Open.Content = "Close";
                el_Stat.Fill = Brushes.Green;
            }
            else
            {
                try
                {
                    serialPort.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                OfforOn = false;
                btn_Open.Content = "Open";
                el_Stat.Fill = Brushes.Red;
            }

        }

        private void Btn_Check_Click(object sender, RoutedEventArgs e)
        {
            ComBoxPort.ItemsSource = SerialPort.GetPortNames();
        }

        //private void BtnGetTime_Click(object sender, RoutedEventArgs e)
        //{
        //    string year = System.DateTime.Now.Year.ToString();
        //    string month = System.DateTime.Now.Month.ToString();
        //    string day = System.DateTime.Now.Day.ToString();
        //    string hour = System.DateTime.Now.Hour.ToString();
        //    string minute = System.DateTime.Now.Minute.ToString();
        //    string second = System.DateTime.Now.Second.ToString();

        //    if (month.Length == 1)
        //    {
        //        month = "0" + month;
        //    }
        //    if (day.Length == 1)
        //    {
        //        day = "0" + day;
        //    }
        //    if (hour.Length == 1)
        //    {
        //        hour = "0" + hour;
        //    }
        //    if (minute.Length == 1)
        //    {
        //        minute = "0" + minute;
        //    }
        //    if (second.Length == 1)
        //    {
        //        second = "0" + second;
        //    }

        //    //TextBoxSend.Text = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
        //}

        //static byte HEXtoBCD(byte c)
        //{
        //    byte t;

        //    t = (byte)((c % 100) / 10);
        //    t <<= 4;
        //    t |= (byte)(c % 10);

        //    return t;
        //}

        private void Btn_Send_Click(object sender, RoutedEventArgs e)
        {
            byte[] sendBuff = new byte[53];

            IPAddress iPAddress = null; //IPAddress.Parse(tb_UpdateIP.Text);
            UInt16 port = 0;// UInt16.Parse(tb_UpdatePort.Text);
            byte[] ip = new byte[4];

            string str = null;

            sendBuff[0] = 0x01;
            sendBuff[1] = 0x10;

            if (cb_Update.IsChecked == true)
            {
                sendBuff[2] = 0x01;

                if (tb_UpdateIP.Text == "" || tb_UpdatePort.Text == "")
                {
                    return;
                }

                iPAddress = IPAddress.Parse(tb_UpdateIP.Text);
                port = UInt16.Parse(tb_UpdatePort.Text);
                ip = iPAddress.GetAddressBytes();

                ip.CopyTo(sendBuff, 3);

                sendBuff[7] = (byte)(port >> 8);
                sendBuff[8] = (byte)(port & 0xff);

                str += tb_UpdateIP.Text + ":" + tb_UpdatePort.Text + "\r\n";
            }

            if (cb_DataCh1.IsChecked == true)
            {
                sendBuff[9] = 0x01;

                if (tb_CompantIP.Text == "" || tb_CompantPort.Text == "")
                {
                    return;
                }

                iPAddress = IPAddress.Parse(tb_CompantIP.Text);
                port = UInt16.Parse(tb_CompantPort.Text);
                ip = iPAddress.GetAddressBytes();

                ip.CopyTo(sendBuff, 10);

                sendBuff[14] = (byte)(port >> 8);
                sendBuff[15] = (byte)(port & 0xff);

                str += tb_CompantIP.Text + ":" + tb_CompantPort.Text + "\r\n";
            }

            if (cb_DataCh2.IsChecked == true)
            {
                sendBuff[16] = 0x01;

                if (tb_ClientIP.Text == "" || tb_ClientPort.Text == "")
                {
                    return;
                }

                iPAddress = IPAddress.Parse(tb_ClientIP.Text);
                port = UInt16.Parse(tb_ClientPort.Text);
                ip = iPAddress.GetAddressBytes();

                ip.CopyTo(sendBuff, 17);

                sendBuff[21] = (byte)(port >> 8);
                sendBuff[22] = (byte)(port & 0xff);

                str += tb_ClientIP.Text + ":" + tb_ClientPort.Text + "\r\n";
            }

            if (cb_Cycle.IsChecked == true)
            {
                sendBuff[23] = 0x01;

                if (tb_Cycle.Text == "")
                {
                    return;
                }

                UInt16 cycle = UInt16.Parse(tb_Cycle.Text);

                sendBuff[24] = (byte)(cycle >> 8);
                sendBuff[25] = (byte)(cycle & 0xff);

                str += tb_Cycle.Text + "\r\n";
            }

            if (cb_Para.IsChecked == true)
            {
                sendBuff[26] = 0x01;

                if (tb_ST.Text == "" || tb_PWD.Text == "" || tb_ID.Text == "" || tb_MN.Text == "")
                {
                    return;
                }

                byte[] st = Encoding.ASCII.GetBytes(tb_ST.Text);
                byte[] pwd = Encoding.ASCII.GetBytes(tb_PWD.Text);
                byte[] mn = Encoding.ASCII.GetBytes(tb_MN.Text);
                UInt16 id = UInt16.Parse(tb_ID.Text);

                st.CopyTo(sendBuff, 27);
                pwd.CopyTo(sendBuff, 27 + st.Length);


                sendBuff[27 + st.Length + pwd.Length] = (byte)(id >> 8);
                sendBuff[27 + st.Length + pwd.Length + 1] = (byte)(id & 0xff);

                mn.CopyTo(sendBuff, 39);


                str += tb_ST.Text + " " + tb_PWD.Text + " " + tb_ID.Text + " " + tb_MN.Text + "\r\n";
            }

            tb_Recv.Text = str + tb_Recv.Text;
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            tb_Recv.Text = "";
        }
    }
}
