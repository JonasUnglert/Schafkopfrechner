using Schafkopfrechner.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schafkopfrechner.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EndOfRoundPage : ContentPage
    {
        private const int MaximumAmountLaeufer = 8;
        private const int MinimumAmountLaeufer = 3;

        ObservableCollection<RoundPlayer> originalPlayerList = new ObservableCollection<RoundPlayer>();
        private bool isOriginalListSaved = false;
        private int amountOfLaeufer = 0;
        private bool isSchneiderGame = false;
        private bool isSchwarzGame = false;
        private int gamePrice = 0;
        private bool isProgrammaticChange = false;

        private bool calculationExecuted = false;
        private PlayEndGameViewModel viewModel = new PlayEndGameViewModel();

        public EndOfRoundPage()
        {
            InitializeComponent();
            viewModel.Players = RoundPlayerManager.Instance.Players;
            viewModel.IsRamschPlayed = GameInfoManager.Instance.GameInfo.Last().GameType == GameInfo.GameTypeEnum.Ramsch ? true : false;
            viewModel.LegenIsAllowed = GeneralGameRules.Instance.LegenIsAllowed;
            viewModel.KontraIsAllowed = GeneralGameRules.Instance.KontraIsAllowed && !viewModel.IsRamschPlayed;
            viewModel.ShowGamePrice = false;
            viewModel.GamePrice = 0;
            this.BindingContext = viewModel;
        }

        private void SchneiderCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.

            isSchneiderGame = isChecked ? true : false;

            var losingPlayers = RoundPlayerManager.Instance.Players.Where(p => p.DidWin == false);
            foreach (var player in losingPlayers)
            {
                int indexPlayer = RoundPlayerManager.Instance.Players.IndexOf(player);
                RoundPlayerManager.Instance.Players[indexPlayer].IsSchneider = isChecked;
            }
        }

        private void SchwarzCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value;

            isSchwarzGame = isChecked ? true : false;
            var losingPlayers = RoundPlayerManager.Instance.Players.Where(p => p.DidWin == false);
            foreach (var player in losingPlayers)
            {
                int indexPlayer = RoundPlayerManager.Instance.Players.IndexOf(player);
                RoundPlayerManager.Instance.Players[indexPlayer].IsSchwarz = isChecked;
            }
        }

        private async void LaeuferEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isProgrammaticChange)
            {
                return;
            }

            if (!int.TryParse(e.NewTextValue, out int inputAmountOfLaeufer) && e.NewTextValue != string.Empty)
            {
                isProgrammaticChange = true;
                await DisplayAlert("Falscher Wert", "Bitte nur Zahlenwerte eingeben", "OK");
                ((Entry)sender).Text = "";
                isProgrammaticChange = false;
                return;
            }

            amountOfLaeufer = inputAmountOfLaeufer;

            foreach (var player in GameInfoManager.Instance.GameInfo.Last().Players)
            {
                player.AmountOfLaeufer = amountOfLaeufer;
            }
        }


        private async void CalculateButton_Clicked(object sender, EventArgs e)
        {

            if (amountOfLaeufer != 0 && (amountOfLaeufer < MinimumAmountLaeufer || amountOfLaeufer > MaximumAmountLaeufer))
            {
                await DisplayAlert("Falscher Wert", "Die Läuferanzahl darf nur zwischen 2 und 8 sein", "OK");
                return;
            }
            
            GameInfoManager.Instance.GameInfo.Last().CalcNewBankBalance();

            viewModel.ShowGamePrice = true;
            viewModel.GamePrice = GameInfoManager.Instance.GameInfo.Last().TotalGamePrice;
            this.calculationExecuted = true;

        }

        private async void NextRoundButton_Clicked(object sender, EventArgs e)
        {
            if (calculationExecuted == false)
            {
                await DisplayAlert("Abrechnung fehlt", "Spiel bitte abrechnen.", "OK");
                return;
            }

            foreach (var player in RoundPlayerManager.Instance.Players)
            {
                player.ResetPlayValues();
            }

            int indexGeber = RoundPlayerManager.Instance.Players.ToList().FindIndex(p => p.IsGeber);

            if (indexGeber == -1)
            {
                throw new Exception("Kein Geber definiert!");
            }

            int newGeberIndex = indexGeber + 1;

            if (RoundPlayerManager.Instance.Players.Count == newGeberIndex)
            {
                newGeberIndex = 0;
            }

            RoundPlayerManager.Instance.Players[indexGeber].IsGeber = false;
            RoundPlayerManager.Instance.Players[newGeberIndex].IsGeber = true;

            await Navigation.PushAsync(new RoundStartPage());
        }

        private async void EndGameButton_clicked(object sender, EventArgs e)
        {
            bool wantsToContinue = await DisplayAlert("Beenden?", "Wenn du das Spiel beendest gehen die Spielstände verloren!", "Nein", "Ja");
            if (wantsToContinue)
            {
                return;
            }

            RoundPlayerManager.Instance.Players.Clear();

            await Navigation.PushAsync(new AppStartPage());
        }
    }


    public class PlayEndGameViewModel : INotifyPropertyChanged
    {
        private bool _isRamschPlayed;
        private bool _LegenIsAllowed;
        private bool _KontraIsAllowed;
        private int _gamePrice;
        private bool _showGamePrice;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<RoundPlayer> Players { get; set; }

        public bool ShowGamePrice
        {
            get => _showGamePrice;
            set
            {
                if (_showGamePrice != value)
                {
                    _showGamePrice = value;
                    OnPropertyChanged(nameof(ShowGamePrice));
                }
            }
        }

        public int GamePrice
        {
            get => _gamePrice;
            set
            {
                if (_gamePrice != value)
                {
                    _gamePrice = value;
                    OnPropertyChanged(nameof(GamePrice));
                }
            }
        }

        public bool IsRamschPlayed
        {
            get => _isRamschPlayed;
            set
            {
                if (_isRamschPlayed != value)
                {
                    _isRamschPlayed = value;
                    OnPropertyChanged(nameof(IsRamschPlayed));
                }
            }
        }

        public bool LegenIsAllowed
        {
            get => _LegenIsAllowed;
            set
            {
                if (_LegenIsAllowed != value)
                {
                    _LegenIsAllowed = value;
                    OnPropertyChanged(nameof(LegenIsAllowed));
                }
            }
        }

        public bool KontraIsAllowed
        {
            get => _KontraIsAllowed;
            set
            {
                if (_KontraIsAllowed != value)
                {
                    _KontraIsAllowed = value;
                    OnPropertyChanged(nameof(KontraIsAllowed));
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}