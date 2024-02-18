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
        public bool IsGeber { get; set; } = false;
        public bool IsPlayer { get; set; } = false;
        public bool DidJungfrau { get; set; } = false;
        public bool IsSchneider { get; set; } = false;
        public bool IsSchwarz { get; set; } = false;
        public int AmountOfLaeufer { get; set; } = 0;

        private bool didLegen;
        public bool DidLegen
        {
            get => didLegen;
            set
            {
                if (didLegen != value)
                {
                    didLegen = value;
                    OnPropertyChanged(nameof(DidLegen));
                }
            }
        }

        private bool didKontra { get; set; } = false;
        public bool DidKontra
        {
            get => didKontra;
            set
            {
                if (didKontra != value)
                {
                    didKontra = value;
                    OnPropertyChanged(nameof(DidKontra));
                }
            }
        }

        public Player DeepCopy()
        {
            Player copiedPlayer = new Player
            {
                Name = this.Name,
                BankBalanceInCent = this.BankBalanceInCent,
                DidWin = this.DidWin,
                IsGeber = this.IsGeber,
                IsPlayer = this.IsPlayer,
                DidLegen = this.DidLegen,
                DidKontra = this.DidKontra,
                DidJungfrau = this.DidJungfrau,
                IsSchneider = this.IsSchneider,
                IsSchwarz = this.IsSchwarz,
                AmountOfLaeufer = this.AmountOfLaeufer,
            };

            return copiedPlayer;
        }

        public void ResetPlayValues()
        {
            DidJungfrau = false;
            DidKontra = false;
            DidLegen = false;
            DidWin = false;
            IsPlayer = false;
            IsSchwarz = false;
            IsSchneider = false;
            AmountOfLaeufer = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
