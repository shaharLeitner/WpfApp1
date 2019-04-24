using FlightSimulator.ViewModels;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.ViewModels
{
    public class ManualViewModel : BaseNotify
    {
        #region Singleton
        private static ManualViewModel m_Instance = null;
        public static ManualViewModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ManualViewModel(SimulatorModel.Instance);
                }
                return m_Instance;
            }
        }
        #endregion
        // values for the binding
        private double throtle;
        private double rudder;
        public double Throttle {
            get { return throtle; }
            set
            {
                throtle = value;
                model.SendByVal("throttle", value);
            }
        }
        public double Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                model.SendByVal("rudder", value);
            }
        }
        private SimulatorModel model;
        private ManualViewModel(SimulatorModel m)
        {
            // we need to save the model and use the observing
            model = m;
            model.PropertyChanged += (object Sender, PropertyChangedEventArgs e) =>
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }
        // when the joystick was moved we need to move the plane
        public void WhenMoved(Joystick sender, FlightSimulator.Model.EventArgs.VirtualJoystickEventArgs args)
        {
            model.SendByVal("aileron", args.Aileron);
            model.SendByVal("elevator", args.Elevator);
        }
    }
}
