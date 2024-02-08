using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class PlayerManager
    {
        private static PlayerManager instance;
        public static PlayerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerManager();
                }
                return instance;
            }
        }

        public ObservableCollection<Player> Players { get; private set; }

        private PlayerManager()
        {
            Players = new ObservableCollection<Player>();
        }
    }

}
