using Schafkopfrechner.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schafkopfrechner.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoundStartPage : ContentPage
    {
        public RoundStartPage()
        {
            this.BackgroundImageSource = App.BackgroundFilename;
            InitializeComponent();

            RoundStartViewModel viewModel = new RoundStartViewModel();
            viewModel.Players = RoundPlayerManager.Instance.Players;
            viewModel.IsRamschAllowed = GeneralGameRules.Instance.RamschIsAllowed;
            viewModel.IsLegenAllowed = GeneralGameRules.Instance.LegenIsAllowed;
            this.BindingContext = viewModel;

            GameInfo gameInfo = new GameInfo();
            gameInfo.dateTime = DateTime.Now;
            gameInfo.Players = RoundPlayerManager.Instance.Players;
            gameInfo.BaseRoundPrice = GeneralGameRules.Instance.PriceInCent;
            GameInfoManager.Instance.GameInfo.Add(gameInfo);
        }

        private void PlayerButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            
            if (button != null)
            {
                string playerName = button.CommandParameter as string;

                RoundPlayer playingPlayer = RoundPlayerManager.Instance.Players.First(p => p.Name == playerName);

                int indexOfPlayingPlayer = RoundPlayerManager.Instance.Players.IndexOf(playingPlayer);

                RoundPlayerManager.Instance.Players[indexOfPlayingPlayer].IsPlayer = true;

                this.NavigateToChooseGamePage();
            }
        }

        private async void NavigateToChooseGamePage()
        {
            await Navigation.PushAsync(new ChooseGamePage());
        }

        private async void RamschButton_Clicked(object sender, EventArgs e)
        {
            GameInfoManager.Instance.GameInfo.Last().GameType = GameInfo.GameTypeEnum.Ramsch;
            await Navigation.PushAsync(new WhilePlayingPage()); 
        }
    }

    public class RoundStartViewModel : INotifyPropertyChanged
    {
        private bool _isRamschAllowed;

        private bool _isLegenAllowed;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<RoundPlayer> Players { get; set; }

        public bool IsRamschAllowed
        {
            get => _isRamschAllowed;
            set
            {
                if (_isRamschAllowed != value)
                {
                    _isRamschAllowed = value;
                    OnPropertyChanged(nameof(IsRamschAllowed));
                }
            }
        }

        public bool IsLegenAllowed
        {
            get => _isLegenAllowed;
            set
            {
                if (_isLegenAllowed != value)
                {
                    _isLegenAllowed = value;
                    OnPropertyChanged(nameof(_isLegenAllowed));
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}