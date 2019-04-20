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
        private InfoGetter getter;
        private CommandsSender sender;
        private ISettingsModel settingsModel;
        public double Lat {
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
        public double Lon
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

        public event PropertyChangedEventHandler PropertyChanged;

        private SimulatorModel()
        {
            stop = false;
            settingsModel = ApplicationSettingsModel.Instance;
            getter = new InfoGetter(settingsModel);
            sender = new CommandsSender(settingsModel);
        }
        public void sendAsIs(string toSend)
        {
            //sender.send(toSend);
        }
        public void UpdateIpPort(string ip, int infoPort, int commandPort)
        {
            settingsModel.FlightCommandPort = commandPort;
            settingsModel.FlightInfoPort = infoPort;
            settingsModel.FlightServerIP = ip;
            settingsModel.SaveSettings();
            //getter.update(ip, port);
            //sender.update(ip, port);
        }
        public void SendByVal(string variable, int value)
        {
            // create the correct command string
        }
        public void ConnectCmd()
        {
            getter.stop();
            settingsModel.ReloadSettings();
            getter = new InfoGetter(settingsModel);
            getter.initServer();
            sender = new CommandsSender(settingsModel);
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Start()
        {
            stop = false;
            new Thread(delegate () {
                while (!stop)
                {
                    Lat = getter.Lattitude;
                    Lon = getter.Longttude; 
                    Thread.Sleep(250);// read the data in 4Hz
                }
            }).Start();
        }

        public void End()
        {
            stop = true;
        }
    }
}
