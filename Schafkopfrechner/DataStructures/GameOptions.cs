using System;
using System.Collections.Generic;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class GameOptions
    {
        private static readonly Lazy<GameOptions> lazy = new Lazy<GameOptions>(() => new GameOptions());

        public static GameOptions Instance => lazy.Value;

        private GameOptions()
        {
        }

        public int PriceInCent { get; set; } = 10;
        public bool SoloIsAllowed { get; set; } = false;
        public bool WenzIsAllowed { get; set; } = false;

        public bool SauspielIsAllowed { get; set; } = false;

        public bool RamschIsAllowed { get; set; } = false;
        public bool LegenIsAllowed { get; set; } = false;
        public bool KontraIsAllowed { get; set; } = false;
        public bool KontraSauspielIsAllowed { get; set; } = false;
    }
}

