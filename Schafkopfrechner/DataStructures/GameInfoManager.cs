using System;
using System.Collections.Generic;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class GameInfoManager
    {
        public List<GameInfo> GameInfo { get; private set; }

        private static readonly Lazy<GameInfoManager> lazy = new Lazy<GameInfoManager>(() => new GameInfoManager());

        public static GameInfoManager Instance => lazy.Value;

        private GameInfoManager()
        {
            GameInfo = new List<GameInfo>();
        }
    }
}
