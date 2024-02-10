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
    public partial class GameOptionsPage : ContentPage
    {
        private bool isProgrammaticChange = false; 
        public GameOptionsPage()
        {
            InitializeComponent();
        }

        private async void PriceEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (isProgrammaticChange)
            {
                return;
            }

            if (!int.TryParse(e.NewTextValue, out int priceInCent) && e.NewTextValue != string.Empty)
            {
                isProgrammaticChange=true;
                await DisplayAlert("Falscher Wert", "Bitte nur Zahlenwerte eingeben", "OK");
                ((Entry)sender).Text = e.OldTextValue;
                isProgrammaticChange = false;
                return;
            }

            GameOptions.Instance.PriceInCent = priceInCent;
        }

        private void SoloCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptions.Instance.SoloIsAllowed = isChecked;
        }

        private void WenzCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptions.Instance.WenzIsAllowed = isChecked;
        }
        private void SauspielCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptions.Instance.SauspielIsAllowed = isChecked;
        }

        private void RamschCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptions.Instance.RamschIsAllowed = isChecked;
        }

        private void LegenCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptions.Instance.LegenIsAllowed = isChecked;
        }

        private void KontraCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptions.Instance.KontraIsAllowed = isChecked;
        }

        private void KontraSauspielCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value; // Der neue Zustand der Checkbox: true, wenn ausgewählt; false, wenn nicht ausgewählt.
            GameOptions.Instance.KontraSauspielIsAllowed = isChecked;
        }
        private async void NavigateButton_Clicked(object sender, EventArgs e)
        {
            // Navigiere zur nächsten Seite
            await Navigation.PushAsync(new RoundStartPage()); // NextPage ist ein Platzhalter für die tatsächliche Seite, zu der navigiert werden soll
        }

    }
}