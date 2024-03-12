using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.PlatformConfiguration;

namespace Schafkopfrechner.DataStructures
{
    public class GameInfo
    {
        private const int SoloPriceFactor = 5;
        private const int SoloWinFactor = 3;
        public enum GameTypeEnum
        {
            Sauspiel,
            Solo,
            Wenz,
            Wenztout,
            Solotout,
            Ramsch,
            Bettl,
            Farbwenz
        }

        public GameTypeEnum GameType { get; set; }

        public ObservableCollection<RoundPlayer> Players { get; set; }

        public ObservableCollection<RoundPlayer> BackupPlayers { get; set; } = new ObservableCollection<RoundPlayer>();

        public DateTime dateTime { get; set; }

        public int BaseRoundPrice { get; set; }

        public int TotalGamePrice { get; set; }

        public void CalcNewBankBalance()
        {
            this.SetBankBalanceToBackup();

            int gameFactor = CalcFactor();
            int basePrice = CalcBasePrice();

            TotalGamePrice = basePrice * gameFactor;

            for (int i = 0; i < Players.Count; i++) 
            {
                var playerCopy = Players[i].DeepCopy();

                playerCopy = this.ApplyNewBankBalance(playerCopy, gameFactor, basePrice);

                Players[i] = playerCopy;
            }
        }

        private int CalcBasePrice()
        {
            int basePrice = BaseRoundPrice;

            if (GameType == GameTypeEnum.Sauspiel || GameType == GameTypeEnum.Ramsch)
            {
                basePrice = BaseRoundPrice;
            }
            else if (GameType == GameTypeEnum.Solo || GameType == GameTypeEnum.Wenz)
            {
                basePrice = BaseRoundPrice * SoloPriceFactor;
            }

            bool isSchneiderGame = Players.Any(p => p.IsSchneider == true);
            bool isSchwarzGame = Players.Any(p => p.IsSchwarz == true);
            int amountLaeufer = Players.Max(p => p.AmountOfLaeufer);

            if (isSchneiderGame)
            {
                basePrice = basePrice + BaseRoundPrice;
            }

            if (isSchwarzGame)
            {
                basePrice = basePrice + BaseRoundPrice;
            }

            int laeuferPrice = BaseRoundPrice * amountLaeufer;
            basePrice += laeuferPrice;

            return basePrice;
        }

        private int CalcFactor()
        {
            int amountLeger = Players.Where(p => p.DidLegen == true).Count();
            int legerFactor = (int)Math.Pow(2, amountLeger);

            if (GameType == GameTypeEnum.Ramsch)
            {
                int amountJungfrauen = Players.Where(p => p.DidJungfrau == true).Count();
                int jungfrauFactor = (int)Math.Pow(2, amountJungfrauen);
                return legerFactor * jungfrauFactor;
            }
            else
            {
                int amountKontra = Players.Where(p => p.DidKontra == true).Count();
                int kontraFactor = (int)Math.Pow(2, amountKontra);

                return legerFactor * kontraFactor;
            }
        }

        private RoundPlayer ApplyNewBankBalance(RoundPlayer player, int basePrice, int gameFactor)
        {
            int winFactor = player.DidWin ? 1 : -1;

            if (GameType == GameTypeEnum.Ramsch)
            {
                winFactor = player.DidWin ? 1 : -3;
                gameFactor *= player.DidKontra ? 2 : 1;
                basePrice += this.GetAdditionalSchussPrice(player.DidWin);
            }

            if (player.IsPlayer && (GameType == GameTypeEnum.Solo|| GameType == GameTypeEnum.Wenz))
            {
                winFactor = winFactor * SoloWinFactor;
            }

            int totalPrice = basePrice * gameFactor;
            player.BankBalanceInCent += winFactor * totalPrice;

            return player;
        }

        private int GetAdditionalSchussPrice(bool didWin)
        {
            int additionalPrice = 0;

            if (didWin)
            {
                additionalPrice = +GeneralGameRules.Instance.PriceInCent;
            }
            else
            {
                int amountSchuss = Players.Where(p => p.DidSchuss).Count();
                additionalPrice = amountSchuss * GeneralGameRules.Instance.PriceInCent;
            }

            return additionalPrice;
        }

        private void SetBankBalanceToBackup()
        {
            if (BackupPlayers.Count == 0)
            {
                BackupPlayers = this.MakeCopyPlayerList();
            }

            for (int i = 0; i < BackupPlayers.Count; i++)
            {
                Players[i].BankBalanceInCent = BackupPlayers[i].BankBalanceInCent;
            }
        }

        public ObservableCollection<RoundPlayer> MakeCopyPlayerList()
        {
            var copiedRoundPlayerList = new ObservableCollection<RoundPlayer>();

            foreach (var player in this.Players)
            {
                copiedRoundPlayerList.Add(player.DeepCopy());
            }

            return copiedRoundPlayerList;
        }
    }
}
