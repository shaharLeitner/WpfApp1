using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1.ViewModels
{
    class AutoPilotViewModel : BaseNotify
    {
        private SimulatorModel model;
        private string data;
        private SolidColorBrush _color;
        public AutoPilotViewModel(SimulatorModel m)
        {
            // we need to save the model and use the right color for the background
            model = m;
            MyColor = Brushes.White;
            // add a delegate to the property changed of the model
            model.PropertyChanged += (object Sender, PropertyChangedEventArgs e) =>
            {
                NotifyPropertyChanged(e.PropertyName);
            };
            // update our color based on the change in properties.
            PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "Sent" || e.PropertyName == "Data")
                {
                    if (model.Sent) // if all was sent
                    {
                        MyColor = Brushes.White;
                    }
                    else
                    {
                        MyColor = Brushes.PaleVioletRed;
                    }
                }
            };
        }

        // the color of the background
        public SolidColorBrush MyColor
        {
            get
            {
                return _color;
            }
            set
            {
                // we need to notify when we change the color
                _color = value;
                NotifyPropertyChanged("MyColor");
            }
        }
        public string Data
        {
            get
            {
                return data;
            }
            set
            {
                // when we chenge the data the model is zeroed
                model.ZeroAllSent();
                data = value;
                NotifyPropertyChanged("Data");
            }
        }

        private ICommand _okCommand;
        private ICommand _clearCommand;

        // the command for the ok botton
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new CommandHandler(() => OnClickOk()));
            }
        }
        // the command for the clear botton
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new CommandHandler(() => OnClickClear()));
            }
        }

        // when we click ok we want to send all the data to the model.
        private void OnClickOk()
        {
            model.SendMultipleCommands(data);
        }
        // clear the data and change the color to white.
        private void OnClickClear()
        {
            Data = "";
            MyColor = Brushes.White;
            // set color back to white
        }
    }
}
