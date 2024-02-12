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
            InitializeComponent();

            RoundStartViewModel viewModel = new RoundStartViewModel();
            viewModel.Players = PlayerManager.Instance.Players;
            viewModel.IsRamschAllowed = GameOptions.Instance.RamschIsAllowed;
            this.BindingContext = viewModel;
        }

        private void PlayerButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            
            if (button != null)
            {
                string playerName = button.CommandParameter as string;

                Player playingPlayer = PlayerManager.Instance.Players.First(p => p.Name == playerName);

                int indexOfPlayingPlayer = PlayerManager.Instance.Players.IndexOf(playingPlayer);

                PlayerManager.Instance.Players[indexOfPlayingPlayer].IsPlayer = true;

                this.NavigateToChooseGamePage();
            }
        }

        private async void NavigateToChooseGamePage()
        {
            await Navigation.PushAsync(new ChooseGamePage());
        }

        private async void RamschButton_Clicked(object sender, EventArgs e)
        {
            // Navigiere zur nächsten Seite
            await Navigation.PushAsync(new WhilePlayingPage()); // NextPage ist ein Platzhalter für die tatsächliche Seite, zu der navigiert werden soll
        }
    }

    public class RoundStartViewModel : INotifyPropertyChanged
    {
        private bool _isLegenCheckBoxVisible;
        private bool _isRamschAllowed;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Player> Players { get; set; }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}