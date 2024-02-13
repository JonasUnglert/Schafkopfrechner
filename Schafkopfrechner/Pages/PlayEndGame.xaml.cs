using Schafkopfrechner.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private int amountOfLäufer = 0;
        private bool isSchneiderGame = false;
        private bool isSchwarzGame = false;

        private bool isProgrammaticChange = false;

        public PlayEndGame()
        {
            InitializeComponent();
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

            if (!int.TryParse(e.NewTextValue, out amountOfLäufer) && e.NewTextValue != string.Empty && amountOfLäufer > 4)
            {
                isProgrammaticChange = true;
                await DisplayAlert("Falscher Wert", "Bitte nur Zahlenwerte eingeben", "OK");
                ((Entry)sender).Text = e.OldTextValue;
                isProgrammaticChange = false;
                return;
            }

            for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
            {
                PlayerManager.Instance.Players[i].AmountOfLaeufer = amountOfLäufer;
            }
        }

        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
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

            int legerDoubler = 2 ^ PlayerManager.Instance.Players.Where(p => p.DidLegen == true).Count();
            int jungFrauDoubler = 2 ^ PlayerManager.Instance.Players.Where(p => p.DidJungfrau == true).Count();

            int kontraDoubler = 2 ^ PlayerManager.Instance.Players.Where(p => p.DidKontra == true).Count();
            kontraDoubler = isRamsch ? 1 : kontraDoubler;

            int totalGamePrice = tempPrice * kontraDoubler * legerDoubler;

            foreach (var player in PlayerManager.Instance.Players)
            {
                if (player.DidWin == true)
                {
                    int soloFactor = isSolo ? 3 : 1;
                    
                    int ramschFactor = isRamsch && player.DidKontra ? 2 : 1;
                    ramschFactor = isRamsch ? -3 * ramschFactor : 1;

                    player.BankBalanceInCent += (totalGamePrice * soloFactor * ramschFactor);
                }
                else
                {
                    int ramschFactor = isRamsch && player.DidKontra ? 2 : 1;
                    ramschFactor = isRamsch ? 3 : 1;

                    player.BankBalanceInCent -= totalGamePrice * ramschFactor;
                }
            }
        }

        private async void NextRoundButton_Clicked(object sender, EventArgs e)
        {
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
}