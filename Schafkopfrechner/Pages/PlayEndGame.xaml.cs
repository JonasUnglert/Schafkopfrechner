using Schafkopfrechner.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class PlayEndGame : ContentPage
    {
        ObservableCollection<Player> originalPlayerList = new ObservableCollection<Player>();
        private bool isOriginalListSaved = false;
        private int amountOfLaeufer = 0;
        private bool isSchneiderGame = false;
        private bool isSchwarzGame = false;
        private int gamePrice = 0;
        private bool isProgrammaticChange = false;

        private bool calculationExecuted = false;
        private PlayEndGameViewModel viewModel = new PlayEndGameViewModel();

        public PlayEndGame()
        {
            InitializeComponent();
            viewModel.Players = PlayerManager.Instance.Players;
            viewModel.IsRamschPlayed = GameInfoManager.Instance.GameInfo.Last().GameType == GameInfo.GameTypeEnum.Ramsch ? true : false;
            viewModel.LegenIsAllowed = GameOptions.Instance.LegenIsAllowed;
            viewModel.KontraIsAllowed = GameOptions.Instance.RamschIsAllowed;
            viewModel.ShowGamePrice = false;
            viewModel.GamePrice = 0;
            this.BindingContext = viewModel;
        }

        private void SaveOriginalPlayerList()
        {
            if (!isOriginalListSaved)
            {
                originalPlayerList.Clear();
                originalPlayerList = PlayerManager.Instance.MakeCopyPlayerCollection();
                isOriginalListSaved = true;
            }
        }

        private void SchneiderCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.

            isSchneiderGame = isChecked ? true : false;

            var losingPlayers = PlayerManager.Instance.Players.Where(p => p.DidWin == false);
            foreach (var player in losingPlayers)
            {
                int indexPlayer = PlayerManager.Instance.Players.IndexOf(player);
                PlayerManager.Instance.Players[indexPlayer].IsSchneider = isChecked;
            }
        }

        private void SchwarzCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value;

            isSchwarzGame = isChecked ? true : false;
            var losingPlayers = PlayerManager.Instance.Players.Where(p => p.DidWin == false);
            foreach (var player in losingPlayers)
            {
                int indexPlayer = PlayerManager.Instance.Players.IndexOf(player);
                PlayerManager.Instance.Players[indexPlayer].IsSchwarz = isChecked;
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
                isProgrammaticChange=true;
                await DisplayAlert("Falscher Wert", "Bitte nur Zahlenwerte eingeben", "OK");
                ((Entry)sender).Text = e.OldTextValue;
                isProgrammaticChange = false;
                return;
            }

            amountOfLaeufer = inputAmountOfLaeufer;

            for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
            {
                PlayerManager.Instance.Players[i].AmountOfLaeufer = amountOfLaeufer;
            }
        }

        private async void CalculateButton_Clicked(object sender, EventArgs e)
        {

            if (amountOfLaeufer != 0 && (amountOfLaeufer < 2 || amountOfLaeufer > 8))
            {
                await DisplayAlert("Falscher Wert", "Die Läuferanzahl darf nur zwischen 2 und 8 sein", "OK");
                return;
            }

            this.SaveOriginalPlayerList();

            var playersToCalculate = new ObservableCollection<Player>(originalPlayerList);
            int gamePrice = 0;
            bool isSolo = false;
            bool isRamsch = false;
            gamePrice = GameOptions.Instance.PriceInCent;

            if (GameInfoManager.Instance.GameInfo.Last().GameType == GameInfo.GameTypeEnum.Ramsch)
            {
                isRamsch = true;
            }
            else if (GameInfoManager.Instance.GameInfo.Last().GameType != GameInfo.GameTypeEnum.Sauspiel)
            {
                isSolo = true;
                gamePrice = gamePrice * 5;
            }

            int schneiderPrice = isSchneiderGame ? GameOptions.Instance.PriceInCent : 0;
            int schwarzPrice = isSchwarzGame ? GameOptions.Instance.PriceInCent : 0;

            int läuferPrice = GameOptions.Instance.PriceInCent * PlayerManager.Instance.Players.Max(p => p.AmountOfLaeufer);

            läuferPrice = isRamsch ? 0 : läuferPrice;
            schneiderPrice = isRamsch ? 0 : schneiderPrice;
            schwarzPrice = isRamsch ? 0 : schwarzPrice;

            int tempPrice = gamePrice + schneiderPrice + schwarzPrice + läuferPrice;


            int amountLeger = PlayerManager.Instance.Players.Where(p => p.DidLegen == true).Count();
            int amountJungfrauen = PlayerManager.Instance.Players.Where(p => p.DidJungfrau == true).Count();
            int amountKontra = PlayerManager.Instance.Players.Where(p => p.DidKontra == true).Count();

            int legerDoubler = (int)Math.Pow(2, amountLeger);
            int jungFrauDoubler = (int)Math.Pow(2, amountJungfrauen);
            int kontraDoubler = (int)Math.Pow(2, amountKontra);

            kontraDoubler = isRamsch ? 1 : kontraDoubler;

            int totalGamePrice = tempPrice * kontraDoubler * legerDoubler;

            PlayerManager.Instance.Players.Clear();

            foreach (var player in originalPlayerList)
            {
                var playerCopy = player.DeepCopy();

                if (playerCopy.DidWin == true)
                {
                    int soloFactor = isSolo ? 3 : 1;

                    int ramschFactor = isRamsch && player.DidKontra ? 2 : 1;
                    ramschFactor = isRamsch ? -3 * ramschFactor : 1;

                    playerCopy.BankBalanceInCent += (totalGamePrice * soloFactor * ramschFactor);
                }
                else
                {
                    int ramschFactor = isRamsch && player.DidKontra ? 2 : 1;
                    ramschFactor = isRamsch ? -1 : 1;

                    playerCopy.BankBalanceInCent -= totalGamePrice * ramschFactor;
                }

                PlayerManager.Instance.Players.Add(playerCopy);
            }

            viewModel.ShowGamePrice = true;//this.isProgrammaticChange;
            viewModel.GamePrice = totalGamePrice;//this.gamePrice;
            this.calculationExecuted = true;
            
        }

        private async void NextRoundButton_Clicked(object sender, EventArgs e)
        {
            if (calculationExecuted == false)
            {
                await DisplayAlert("Abrechnung fehlt", "Spiel muss noch abgerechnet werden.", "OK");
                return;
            }

            foreach (var player in PlayerManager.Instance.Players)
            {
                PlayerHistoryManager.Instance.AddHistoryGame(player, GameInfoManager.Instance.GameInfo.Last());
                player.ResetPlayValues();
            }

            int indexGeber = PlayerManager.Instance.Players.ToList().FindIndex(p => p.IsGeber);

            if (indexGeber == -1)
            {
                throw new Exception("Kein Geber definiert!");
            }

            int newGeberIndex = indexGeber + 1;

            if (PlayerManager.Instance.Players.Count == newGeberIndex)
            {
                newGeberIndex = 0;
            }

            PlayerManager.Instance.Players[indexGeber].IsGeber = false;
            PlayerManager.Instance.Players[newGeberIndex].IsGeber = true;

            await Navigation.PushAsync(new RoundStartPage());
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

        public ObservableCollection<Player> Players { get; set; }

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