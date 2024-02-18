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
            viewModel.LegenIsAllowed = GameOptions.Instance.LegenIsAllowed;
            viewModel.KontraIsAllowed = GameOptions.Instance.KontraIsAllowed;
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
        private bool _LegenIsAllowed;
        private bool _KontraIsAllowed;

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