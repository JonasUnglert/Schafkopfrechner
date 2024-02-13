using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    class PlayerHistory
    {
        private static PlayerHistory instance;
        public static PlayerHistory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerHistory();
                }
                return instance;
            }
        }

        public ObservableCollection<Player> Players { get; private set; }

        private PlayerHistory()
        {
            Players = new ObservableCollection<Player>();
        }
    }
}
