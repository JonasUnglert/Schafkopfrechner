using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Schafkopfrechner.DataStructures
{
    class PlayerHistoryManager
    {
        private static PlayerHistoryManager instance;
        public static PlayerHistoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerHistoryManager();
                }
                return instance;
            }
        }

        public ObservableCollection<PlayerHistory> PlayerHistories { get; private set; }

        private PlayerHistoryManager()
        {
            PlayerHistories = new ObservableCollection<PlayerHistory>();
        }

        public void AddHistoryGame(Player player, GameInfo lastGameInfo)
        {
            int indexPlayer = PlayerHistories.ToList().FindIndex(ph => ph.Name == player.Name);

            HistoryGame lastGame = new HistoryGame();

            lastGame.BankBalanceInCent = player.BankBalanceInCent;
            lastGame.IsSchneider = player.IsSchneider;
            lastGame.IsSchwarz = player.IsSchwarz;
            lastGame.IsPlayer = player.IsPlayer;
            lastGame.AmountOfLaeufer = player.AmountOfLaeufer;
            lastGame.DidJungfrau = player.DidJungfrau;
            lastGame.DidWin = player.DidWin;
            lastGame.IsGeber =player.IsGeber;
            lastGame.DidLegen = player.DidLegen;
            lastGame.DidKontra = player.DidKontra;
            lastGame.GameInfo = lastGameInfo;

            if (indexPlayer == -1)
            {
                PlayerHistory newPlayer = new PlayerHistory();
                newPlayer.Name = player.Name;
                PlayerHistories.Add(newPlayer);
                indexPlayer = PlayerHistories.Count - 1;
            }

            PlayerHistories[indexPlayer].GameHistoryList.Add(lastGame);
        }
    }

    public class PlayerHistory
    {
        public string Name;
        public List<HistoryGame> GameHistoryList;
    }

    public class HistoryGame
    {
        public int BankBalanceInCent { get; set; } = 0;

        public bool DidWin { get; set; } = false;

        public bool IsGeber { get; set; } = false;

        public bool DidLegen { get; set; } = false;

        public bool DidKontra { get; set; } = false;

        public bool IsPlayer { get; set; } = false;

        public bool DidJungfrau { get; set; } = false;

        public bool IsSchneider { get; set; } = false;

        public bool IsSchwarz { get; set; } = false;

        public int AmountOfLaeufer { get; set; } = 0;

        public GameInfo GameInfo { get; set; }
    }
}
