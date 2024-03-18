using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

                playerCopy = this.ApplyNewBankBalance(playerCopy, basePrice, gameFactor);

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

                // wenn der verlierer geschossen hat, dann verdoppelt sich der preis
                int schussFactor;
                try
                {
                    schussFactor = Players.First(p => p.DidWin == false).DidSchuss ? 2 : 1;
                }
                catch (InvalidOperationException ex) 
                {
                    schussFactor = 1;
                }

                return legerFactor * jungfrauFactor * schussFactor;
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

            // fuer den ramsch verlierer muss der preis gesondert berechnet werden, da jeder spieler unterschiedlich viel bekommt
            if (GameType == GameTypeEnum.Ramsch && !player.DidWin)
            {
                int completeLoserPrice = 0;

                foreach (var roundPlayer in Players)
                {
                    if (roundPlayer.DidWin)
                    {
                        int schussFactor = roundPlayer.DidSchuss ? 2 : 1;

                        completeLoserPrice += basePrice * schussFactor * gameFactor;
                    }
                }

                player.BankBalanceInCent -= completeLoserPrice;

                return player;
            }

            if (GameType == GameTypeEnum.Ramsch && player.DidWin)
            {
                int schussFactor = player.DidSchuss ? 2 : 1;
                gameFactor = gameFactor * schussFactor;
            }

            if (player.IsPlayer && (GameType == GameTypeEnum.Solo || GameType == GameTypeEnum.Wenz))
            {
                winFactor = winFactor * SoloWinFactor;
            }

            int totalPrice = basePrice * gameFactor;
            player.BankBalanceInCent += winFactor * totalPrice;

            return player;
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
