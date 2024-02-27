using System;
using System.Collections.Generic;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class GeneralGameRules
    {
        private static readonly Lazy<GeneralGameRules> lazy = new Lazy<GeneralGameRules>(() => new GeneralGameRules());

        public static GeneralGameRules Instance => lazy.Value;

        private GeneralGameRules()
        {
        }

        public int PriceInCent { get; set; } = 10;
        public bool SoloIsAllowed { get; set; } = true;
        public bool WenzIsAllowed { get; set; } = true;

        public bool SauspielIsAllowed { get; set; } = true;

        public bool RamschIsAllowed { get; set; } = true;
        public bool LegenIsAllowed { get; set; } = true;
        public bool KontraIsAllowed { get; set; } = true;
        public bool KontraSauspielIsAllowed { get; set; } = false;
    }
}

