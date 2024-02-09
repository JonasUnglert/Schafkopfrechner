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
            viewModel.IsLegenCheckBoxVisible = GameOptionManager.Instance.GameOptions.LegenIsAllowed;
            this.BindingContext = viewModel;
        }

        private void PlayerButton_Clicked(object sender, EventArgs e)
        {
            int i = 0;
        }

        private async void NavigateButton_Clicked(object sender, EventArgs e)
        {
            // Navigiere zur nächsten Seite
            await Navigation.PushAsync(new GameOptionsPage()); // NextPage ist ein Platzhalter für die tatsächliche Seite, zu der navigiert werden soll
        }
    }

    public class RoundStartViewModel
    {
        public ObservableCollection<Player> Players { get; set; }

        public bool IsLegenCheckBoxVisible { get; set; }

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