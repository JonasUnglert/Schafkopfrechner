using Schafkopfrechner.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
            viewModel.IsLegenCheckBoxVisible = GameOptions.Instance.LegenIsAllowed;
            viewModel.IsRamschAllowed = GameOptions.Instance.RamschIsAllowed;
            this.BindingContext = viewModel;
        }

        private async void PlayerButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            
            if (button != null)
            {
                var buttonText = button.Text;

                Player playingPlayer = PlayerManager.Instance.Players.First(p => p.Name == buttonText);

                int indexOfPlayingPlayer = PlayerManager.Instance.Players.IndexOf(playingPlayer);

                PlayerManager.Instance.Players[indexOfPlayingPlayer].IsPlayer = true;

                await Navigation.PushAsync(new ChooseGamePage());
            }
        }

        private async void RamschButton_Clicked(object sender, EventArgs e)
        {
            // Navigiere zur nächsten Seite
            await Navigation.PushAsync(new WhilePlayingPage()); // NextPage ist ein Platzhalter für die tatsächliche Seite, zu der navigiert werden soll
        }
    }

    public class RoundStartViewModel
    {
        public ObservableCollection<Player> Players { get; set; }

        public bool IsLegenCheckBoxVisible { get; set; }

        public bool IsRamschAllowed { get; set; }

        public bool NotIsLegenCheckBoxVisible
        {
            get
            {
                return !this.IsLegenCheckBoxVisible;
            }
            set
            {
                this.IsLegenCheckBoxVisible = value;
            }
        }
    }
}