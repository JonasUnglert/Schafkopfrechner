using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class RoundPlayerManager
    {
        private static RoundPlayerManager instance;

        public static RoundPlayerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoundPlayerManager();
                }
                return instance;
            }
        }

        public ObservableCollection<RoundPlayer> Players { get; private set; }

        private RoundPlayerManager()
        {
            Players = new ObservableCollection<RoundPlayer>();
        }
    }

}
