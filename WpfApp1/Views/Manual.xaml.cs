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
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for Manual.xaml
    /// </summary>
    public partial class Manual : UserControl
    {
        ManualViewModel vm;
        public Manual()
        {
            vm = ManualViewModel.Instance;
            InitializeComponent();
            DataContext = vm;
        }
        /*
        /// <summary>Current Aileron</summary>
        public static readonly DependencyProperty ThrottleProperty =
            DependencyProperty.Register("Throttle", typeof(double), typeof(Manual), null);

        /// <summary>Current Elevator</summary>
        public static readonly DependencyProperty RudderProperty =
            DependencyProperty.Register("Rudder", typeof(double), typeof(Manual), null);


        /// <summary>Current Aileron in degrees from 0 to 360</summary>
        public double Throttle
        {
            get { return Convert.ToDouble(GetValue(ThrottleProperty)); }
            set { SetValue(ThrottleProperty, value); }
        }

        /// <summary>current Elevator (or "power"), from 0 to 100</summary>
        public double Rudder
        {
            get { return Convert.ToDouble(GetValue(RudderProperty)); }
            set { SetValue(RudderProperty, value); }
        }
        */
    }
}
