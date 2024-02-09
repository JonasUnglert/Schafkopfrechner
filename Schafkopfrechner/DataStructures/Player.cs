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

        public bool DidWin {  get; set; }

        public bool IsGeber {  get; set; } = false;

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

        public bool DidKontra { get; set; }

        public bool DidJungfrau { get; set; }

        public bool IsSchneider { get; set; }

        public bool IsSchwarz { get; set; }

        public int AmountOfLaeufer { get; set; }

        public int positionOnTable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
