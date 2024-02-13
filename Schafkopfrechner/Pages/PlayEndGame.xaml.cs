using Schafkopfrechner.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var winningPlayers = PlayerManager.Instance.Players.Where(p => p.DidWin == true);

            foreach (var player in winningPlayers)
            {
                int indexPlayer = PlayerManager.Instance.Players.IndexOf(player);
                PlayerManager.Instance.Players[indexPlayer].AmountOfLaeufer = amountOfLäufer;
            }
        }

        private void CalculateButton_Clicked(object sender, EventArgs e)
        {
            int gamePrice = 0;
            bool isSolo = false;
            if (GameInfoManager.Instance.GameInfo.Last().GameType == GameInfo.GameTypeEnum.Sauspiel)
            {
                gamePrice = GameOptions.Instance.PriceInCent;
            }
            else
            {
                isSolo = true;
                gamePrice = GameOptions.Instance.PriceInCent * 5;
            }

            int schneiderPrice = isSchneiderGame ? GameOptions.Instance.PriceInCent : 0;
            int schwarzPrice = isSchwarzGame ? GameOptions.Instance.PriceInCent : 0;

            int läuferPrice = GameOptions.Instance.PriceInCent * PlayerManager.Instance.Players.Max(p => p.AmountOfLaeufer);

            int tempPrice = gamePrice + schneiderPrice + schwarzPrice + läuferPrice;

            int legerDoubler = PlayerManager.Instance.Players.Where(p => p.DidLegen == true).Any() ? 2 : 1;
            int kontraDoubler = PlayerManager.Instance.Players.Where(p => p.DidKontra == true).Any() ? 2 : 1;

            int totalGamePrice = tempPrice * kontraDoubler * legerDoubler;

            foreach (var player in PlayerManager.Instance.Players)
            {
                if (player.DidWin == true)
                {
                    int soloFactor = isSolo ? 3 : 1;
                    player.BankBalanceInCent += (totalGamePrice * soloFactor);
                }
                else
                {
                    player.BankBalanceInCent -= totalGamePrice;
                }
            }
        }

        private async void NextRoundButton_Clicked(object sender, EventArgs e)
        {
            foreach (var player in PlayerManager.Instance.Players)
            {
                PlayerHistory.Instance.Players.Add(player);
                player.DidJungfrau = false;
                player.ResetPlayValues();
            }

            int indexGeber = PlayerManager.Instance.Players.ToList().FindIndex(p => p.IsGeber);

            if (indexGeber == -1)
            {
                throw new Exception("Kein Geber definiert!");
            }

            int newGeberIndex = indexGeber + 1;

            if(PlayerManager.Instance.Players.Count == newGeberIndex)
            {
                newGeberIndex = 0;
            }

            PlayerManager.Instance.Players[indexGeber].IsGeber = false;
            PlayerManager.Instance.Players[newGeberIndex].IsGeber = true;

            await Navigation.PushAsync(new RoundStartPage());
        }
    }
}