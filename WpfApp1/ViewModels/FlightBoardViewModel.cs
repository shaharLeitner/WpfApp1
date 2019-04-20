using FlightSimulator.Model;
using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private void OnClickCon()
        {
            Console.WriteLine("i am connecting");
            model.ConnectCmd();
            model.Start();
        }
        private void OnClickSet()
        {
            subSet.Show();
        }

        private void OnClickOk ()
        {
            model.UpdateIpPort(Ip, Convert.ToInt32(InfoPort), Convert.ToInt32(CommandPort));
            subSet.Hide();
            DeleteVals();
        }
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

        private void DeleteVals()
        {
            Ip = null;
            InfoPort = null;
            CommandPort = null;
        }
    }
}
