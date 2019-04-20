using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model.Interface;

namespace WpfApp1
{
    class CommandsSender
    {
        private ISettingsModel settingsModel;

        public CommandsSender(ISettingsModel settingsModel)
        {
            this.settingsModel = settingsModel;
        }
    }
}
