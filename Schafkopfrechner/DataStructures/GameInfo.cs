using System;
using System.Collections.Generic;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class GameInfo
    {
        public enum GameTypeEnum
        {
            Sauspiel,
            Solo,
            Wenz,
            Wenztout,
            Solotout,
            Ramsch,
            Bettl,
            Farbwenz
        }

        public GameTypeEnum GameType { get; set; }

        public List<Player> Players{ get; set; }

    }
}
