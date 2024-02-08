using System;
using System.Collections.Generic;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class GameOptionManager
    {
        public GameOptions GameOptions { get; private set; }

        private static readonly Lazy<GameOptionManager> lazy = new Lazy<GameOptionManager>(() => new GameOptionManager());

        public static GameOptionManager Instance => lazy.Value;

        private GameOptionManager()
        {
            GameOptions = new GameOptions();
        }
    }
}
