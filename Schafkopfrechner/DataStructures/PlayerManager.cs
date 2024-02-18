using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class PlayerManager
    {
        private static PlayerManager instance;

        public ObservableCollection<Player> MakeCopyPlayerCollection()
        {
            var copiedPlayerCollection = new ObservableCollection<Player>();
            
            foreach (var player in this.Players)
            {
                copiedPlayerCollection.Add(player.DeepCopy());
            }

            return copiedPlayerCollection;
        }

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
