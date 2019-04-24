using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class SimulatorModel : INotifyPropertyChanged
    {
        #region Singleton
        private double _lat;
        private double _lon;
        private static SimulatorModel m_Instance = null;
        public static SimulatorModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new SimulatorModel();
                }
                return m_Instance;
            }
        }
        #endregion
        bool stop;
        private bool allSent;
        private InfoGetter getter;
        private CommandsSender sender;
        private ISettingsModel settingsModel;
        public double Lat { // the lattiude property of the plane
            get
            {
                return _lat;
            }
            set
            {
                _lat = value;
                NotifyPropertyChanged("Lan");
            }
        }
        public double Lon // longtitude property of the plane
        {
            get
            {
                return _lon;
            }
            set
            {
                _lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        public bool Sent // keep track if all the data was sent
        {
            get
            {
                return allSent;
            }
            set
            {
                allSent = value;
                NotifyPropertyChanged("Sent");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private SimulatorModel()
        {
            stop = false;
            settingsModel = ApplicationSettingsModel.Instance;
            getter = new InfoGetter(settingsModel);
            sender = new CommandsSender(settingsModel);
        }
        // send the data as is to the command serever
        public void SendAsIs(string toSend)
        {
            int answer = sender.Send(toSend);
        }
        // update the ip and port of the settings model.
        public void UpdateIpPort(string ip, int infoPort, int commandPort)
        {
            settingsModel.FlightCommandPort = commandPort;
            settingsModel.FlightInfoPort = infoPort;
            settingsModel.FlightServerIP = ip;
            settingsModel.SaveSettings();
        }
        // send the command to the server by the name of the variable
        public void SendByVal(string variable, double value)
        {
            // set the string right.
            string temp = "set ";
            if(variable == "aileron")
            {
                temp += "/controls/flight/aileron ";
            } else if (variable == "elevator")
            {
                temp += "/controls/flight/elevator ";
            } else if(variable == "rudder")
            {
                temp += "/controls/flight/rudder ";
            } else if (variable == "throttle" )
            {
                temp += "/controls/engines/current-engine/throttle ";
            }
            temp += value.ToString();
            SendAsIs(temp);
        }

        // send multiple commands to the server as they are all in the input string
        public void SendMultipleCommands(string str)
        {
            // run in a new thread so the gui wont freeze.
            new Thread(() =>
            {
                // get each line and send it
                foreach (var myString in str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    SendAsIs(myString);
                    Thread.Sleep(2000);
                }
                Sent = true;
                // notify the sent was changed
                NotifyPropertyChanged("Sent");
            }).Start(); // run the thread
        }
        public void ZeroAllSent()
        {
            allSent = false;
        }
        
        // create a new connection with the flight simulator
        public void ConnectCmd()
        {
            getter.Stop();
            settingsModel.ReloadSettings();
            getter = new InfoGetter(settingsModel);
            getter.InitServer();
            sender = new CommandsSender(settingsModel);
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // run the server that updated the values
        public void Start()
        {
            stop = false;
            // run in a new thread so it will run at all time
            new Thread(delegate () {
                while (!stop)
                {
                    Lat = getter.Lattitude;
                    NotifyPropertyChanged("Lat");
                    Lon = getter.Longttude;
                    NotifyPropertyChanged("Lon");
                    Thread.Sleep(250);// read the data in 4Hz
                }
            }).Start();
        }

        // end the running of the update
        public void End()
        {
            stop = true;
        }
    }
}
