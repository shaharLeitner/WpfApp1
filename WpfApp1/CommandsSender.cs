using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using FlightSimulator.Model.Interface;

namespace WpfApp1
{
    class CommandsSender
    {
        private ISettingsModel settingsModel;
        // we need to keep the settings model to connect
        public CommandsSender(ISettingsModel settingsModel)
        {
            this.settingsModel = settingsModel;
        }

        public int Send(string toSend)
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(settingsModel.FlightServerIP), settingsModel.FlightCommandPort);
                TcpClient client = new TcpClient();
                client.Connect(ep);
                using (NetworkStream stream = client.GetStream())
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    // Send data to server
                    toSend += Environment.NewLine;
                    writer.Write(toSend);
                    // make sure the data gets to the server right now.
                    writer.Flush();
                }
                // close the connection.
                client.Close();
            } catch (Exception)
            {
                return 0;
            }
            return 1;

        }
    }
}
