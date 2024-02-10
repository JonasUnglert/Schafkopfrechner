using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Schafkopfrechner.DataStructures
{
    public class Player : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public int BankBalanceInCent { get; set; } = 0;

        public bool DidWin { get; set; } = false;

        public bool IsGeber {  get; set; } = false;

        public bool IsPlayer { get; set; } = false;

        private bool didLegen;

        public bool DidLegen
        {
            get => didLegen;
            set
            {
                if (didLegen != value)
                {
                    didLegen = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool DidKontra { get; set; } = false;

        public bool DidJungfrau { get; set; } = false;

        public bool IsSchneider { get; set; } = false;

        public bool IsSchwarz { get; set; } = false;

        public int AmountOfLaeufer { get; set; } = 0;

        public int positionOnTable { get; set; } = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
