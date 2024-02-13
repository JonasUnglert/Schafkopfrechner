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
    public partial class WhilePlayingPage : ContentPage
    {
        public WhilePlayingPage()
        {
            InitializeComponent();

            WhilePlayingViewModel viewModel = new WhilePlayingViewModel();
            viewModel.Players = PlayerManager.Instance.Players;
            viewModel.IsRamschPlayed = GameInfoManager.Instance.GameInfo.Last().GameType == GameInfo.GameTypeEnum.Ramsch ? true : false;
            this.BindingContext = viewModel;
        }

        private async void NavigateToChoosGamePage()
        {
            await Navigation.PushAsync(new ChooseGamePage());
        }

        private async void PlayEndsButton_Clicked(object sender, EventArgs e)
        {
            int amountOfWinners = PlayerManager.Instance.Players.Where(p => p.DidWin).Count();
            Console.WriteLine($"amountOfWinners: {amountOfWinners}, Typ: {amountOfWinners.GetType()}");

            if (GameInfoManager.Instance.GameInfo.Last().GameType == GameInfo.GameTypeEnum.Sauspiel)
            {
                if( amountOfWinners != 2)
                {
                    await DisplayAlert("Fehler", "Ein Sauspiel hat zwei Gewinner", "OK");
                    return;
                }
            }
            else
            {
                if (amountOfWinners != 1)
                {
                    await DisplayAlert("Fehler", "Ein Solo/Ramsch hat einen Gewinner", "OK");
                    return;
                }
            }

            await Navigation.PushAsync(new PlayEndGame());
        }
    }

    public class WhilePlayingViewModel : INotifyPropertyChanged
    {
        private bool _isRamschPlayed;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Player> Players { get; set; }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}