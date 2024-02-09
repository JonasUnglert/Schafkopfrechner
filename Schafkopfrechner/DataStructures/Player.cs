using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Schafkopfrechner.DataStructures
{
    public class Player
    {
        public string Name { get; set; }

        public int BankBalanceInCent { get; set; } = 0;

        public bool DidWin {  get; set; }

        public bool DidLegen { get; set; }

        public bool DidKontra { get; set; }

        public bool DidJungfrau { get; set; }

        public bool IsSchneider { get; set; }

        public bool IsSchwarz { get; set; }

        public int AmountOfLaeufer { get; set; }

        public int positionOnTable { get; set; }
    }
}
