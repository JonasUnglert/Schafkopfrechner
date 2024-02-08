using System;
using System.Collections.Generic;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    public class GameOptions
    {
        public int PriceInCent { get; set; } = 10;

        public bool SoloIsAllowed { get; set; } = true;

        public bool WenzIsAllowed { get; set; } = true;

        public bool LegenIsAllowed { get; set; } = false;

        public bool KontraIsAllowed { get; set; } = false;

        public bool KontraSausupielIsAllowed { get; set; } = false;
    }
}
