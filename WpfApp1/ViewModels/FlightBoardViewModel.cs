using FlightSimulator.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;
using WpfApp1;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {
        private SimulatorModel model;
        private Setting subSet;
        public FlightBoardViewModel(SimulatorModel m)
        {
            model = m;
            // set the settings window
            subSet = new Setting(this);
            model.PropertyChanged += (object Sender, PropertyChangedEventArgs e) =>
            {
                NotifyPropertyChanged(e.PropertyName);
            };

        }
        private ICommand _connectCommand;
        private ICommand _settingCommand;
        private ICommand _okCommand;
        private ICommand _cancelCommand;

        public ICommand ConnectCommand
        {
           get
            {
                return _connectCommand ?? (_connectCommand = new CommandHandler(() => OnClickCon()));
            }
        }
        public ICommand SettingCommand
        {
            get
            {
                return _settingCommand ?? (_settingCommand = new CommandHandler(() => OnClickSet()));
            }
        }
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new CommandHandler(() => OnClickOk()));
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new CommandHandler(() => OnClickCancel()));
            }
        }
        // end the current run of the model and restart it
        private void OnClickCon()
        {
            model.End();
            model.ConnectCmd();
            model.Start();
        }
        // show the setting window
        private void OnClickSet()
        {
            subSet.Show();
        }

        // update the ip and port from the setting window
        private void OnClickOk ()
        {
            model.UpdateIpPort(Ip, Convert.ToInt32(InfoPort), Convert.ToInt32(CommandPort));
            subSet.Hide();
            DeleteVals();
        }
        // when we click cancel delete the values and close the setting window
        private void OnClickCancel()
        {
            DeleteVals();
            subSet.Hide();
        }


        public double Lon
        {
            get
            {
                return model.Lon;
            }
        }

        public double Lat
        {
            get
            {
                return model.Lat;
            }

        }
        public string Ip
        {
            get; set;
        }
        public string InfoPort
        {
            get; set;
        }
        public string CommandPort
        {
            get; set;
        }

        // delete the values we have kept
        private void DeleteVals()
        {
            Ip = null;
            InfoPort = null;
            CommandPort = null;
        }
    }
}
