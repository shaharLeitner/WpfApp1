using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using FlightSimulator.Model.Interface;

namespace WpfApp1
{
    public class InfoGetter
    {
        public double Lattitude { get; set; }
        public double Longttude { get; set; }
        private bool run;
        private Thread thread;
        private IPEndPoint ep;
        private TcpListener listner;
        private ISettingsModel settingsModel;

        public InfoGetter(ISettingsModel settingsModel)
        {
            try
            {
                run = true;
                this.settingsModel = settingsModel;
                ep = new IPEndPoint(IPAddress.Parse(settingsModel.FlightServerIP), settingsModel.FlightInfoPort);
                listner = new TcpListener(ep);
            } catch (System.FormatException)
            {
                ep = null;
                listner = null;
                run = false;
            }
        }
        public void initServer()
        {
            thread = new Thread(() =>
            {
                try
                {
                    listner.Start();
                    TcpClient client = listner.AcceptTcpClient();
                    while(run)
                    {
                        using (NetworkStream stream = client.GetStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = reader.ReadLine();
                            string[] values = line.Split(',');
                            Lattitude = Convert.ToDouble(values[1]);
                            Longttude = Convert.ToDouble(values[0]);
                            Console.WriteLine("Lattitude is: " + Lattitude + " Longttude is: " + Longttude);
                            Thread.Sleep(250);
                        }
                    }
                }
                catch (SocketException)
                {
                
                }
            });
            thread.Start();
        }
        
        public void stop()
        {
            run = false;
        }
    }
}
