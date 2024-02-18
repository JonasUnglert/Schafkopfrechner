using Schafkopfrechner.DataStructures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schafkopfrechner.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPlayerPage : ContentPage
    {
        public AddPlayerPage()
        {
            InitializeComponent();
            PlayerManager.Instance.Players.CollectionChanged += Players_CollectionChanged;

#if DEBUG
            List<string> exampleNames = new List<string>() { "Hans", "Peter", "Dieter", "Olaf" };

            for (int i = 0; i < 4; i++)
            {
                var player = new Player { Name = exampleNames[i] };
                PlayerManager.Instance.Players.Add(player);
            }
#endif

            BindingContext = PlayerManager.Instance.Players;
        }

        private void Players_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Button sichtbar machen, wenn vier oder mehr Spieler hinzugefügt wurden
            NextButton.IsVisible = PlayerManager.Instance.Players.Count >= 4; 
        }

        private async void OnAddPlayer_Clicked(object sender, EventArgs e)
        {
            if (PlayerManager.Instance.Players.Count >= 5)
            {
                await DisplayAlert("Maximale Spieleranzahl erreicht", "Du hast die maximale Anzahl an Spieler erreicht", "OK");
                return;
            }

            string playerName = await DisplayPromptAsync("Neuer Spieler", "Name des neuen Spielers eingeben");

            if (PlayerManager.Instance.Players.Where(p => p.Name == playerName).ToList().Count != 0)
            {
                await DisplayAlert("Name doppelt", "Du hast zwei mal den gleichen Namen eingegeben", "OK");
                return;
            }

            if (!string.IsNullOrEmpty(playerName))
            {
                PlayerManager.Instance.Players.Add(new Player { Name = playerName });
            }
        }

        private void OnRemovePlayer_Clicked(object sender, EventArgs e)
        {
            if (PlayerManager.Instance.Players.Count > 0)
            {
                PlayerManager.Instance.Players.RemoveAt(PlayerManager.Instance.Players.Count - 1);
            }
        }

        private async void NavigateButton_Clicked(object sender, EventArgs e)
        {
            // immer den ersten in der liste als geber setzen
            foreach (var player in PlayerManager.Instance.Players)
            {
                player.IsGeber = false;
            }
            PlayerManager.Instance.Players.First().IsGeber = true;

            await Navigation.PushAsync(new GameOptionsPage()); // NextPage ist ein Platzhalter für die tatsächliche Seite, zu der navigiert werden soll
        }
    }
}