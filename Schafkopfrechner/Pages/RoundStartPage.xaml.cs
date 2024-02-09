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
    public partial class RoundStartPage : ContentPage
    {
        public RoundStartPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = PlayerManager.Instance.Players;
        }

        private void Player1CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptionManager.Instance.GameOptions.SoloIsAllowed = isChecked;
        }

        private void Player2CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptionManager.Instance.GameOptions.SoloIsAllowed = isChecked;
        }

        private void Player3CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptionManager.Instance.GameOptions.SoloIsAllowed = isChecked;
        }

        private void Player4CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptionManager.Instance.GameOptions.SoloIsAllowed = isChecked;
        }

        private async void NavigateButton_Clicked(object sender, EventArgs e)
        {
            // Navigiere zur nächsten Seite
            await Navigation.PushAsync(new GameOptionsPage()); // NextPage ist ein Platzhalter für die tatsächliche Seite, zu der navigiert werden soll
        }
    }
}