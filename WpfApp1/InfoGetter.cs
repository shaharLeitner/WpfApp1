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
        // the valued for the plane
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
                // make sure the values for the server run are correct
                run = true;
                this.settingsModel = settingsModel;
                ep = new IPEndPoint(IPAddress.Parse(settingsModel.FlightServerIP), settingsModel.FlightInfoPort);
                listner = new TcpListener(ep);
                listner.Start();
            } catch (System.FormatException)
            {
                ep = null;
                listner = null;
                run = false;
            }
        }
        public void InitServer()
        {
            // start the run of the updating thread and server
            thread = new Thread(() =>
            {
                try
                {
                    bool hasClient = false;
                    TcpClient client;
                    // we dont want the accept client to block us and ruin everything
                    while (!hasClient && run) { // run untill we have a client or stoped
                        hasClient = listner.Pending();
                        Thread.Sleep(250);
                    }
                    if(!hasClient) // we were asked to stop
                    {
                        return;
                    }
                    // get a client
                    client = listner.AcceptTcpClient();
                    using (NetworkStream stream = new NetworkStream(client.Client, false))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (run)
                        {
                            // read a line from the server
                            string line = reader.ReadLine();
                            if(line == null) // if there was an error reading try again
                            {
                                break;
                            }
                            // split the line.
                            string[] values = line.Split(',');
                            // if we didn't get all 25 values there is an error in the simulator
                            if (values.Length != 25)
                            {
                                continue;
                            }
                            // save the values
                            Lattitude = Convert.ToDouble(values[1]);
                            Longttude = Convert.ToDouble(values[0]);
                            Thread.Sleep(250);
                        }
                        client.Close();
                    }
                }
                catch (SocketException)
                {
                
                }
            });
            thread.Start();
        }
        // stop the running of the server
        public void Stop()
        {
            run = false;
            listner.Stop();
        }
    }
}
