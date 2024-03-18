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
    public partial class ChooseGamePage : ContentPage
    {
        public ChooseGamePage()
        {
            this.BackgroundImageSource = App.BackgroundFilename;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            ChooseGameViewModel viewModel = new ChooseGameViewModel();
            viewModel.isSoloAllowed = GeneralGameRules.Instance.SoloIsAllowed;
            viewModel.isWenzAllowed = GeneralGameRules.Instance.WenzIsAllowed;
            viewModel.isSauspielAllowed = GeneralGameRules.Instance.SauspielIsAllowed;

            this.BindingContext = viewModel;
        }

        private async void SoloButton_Clicked(object sender, EventArgs e)
        {
            GameInfoManager.Instance.GameInfo.Last().GameType = GameInfo.GameTypeEnum.Solo;
            await Navigation.PushAsync(new WhilePlayingPage());
        }

        private async void WenzButton_Clicked(object sender, EventArgs e)
        {
            GameInfoManager.Instance.GameInfo.Last().GameType = GameInfo.GameTypeEnum.Wenz;
            await Navigation.PushAsync(new WhilePlayingPage());
        }

        private async void SauspielButton_Clicked(object sender, EventArgs e)
        {
            GameInfoManager.Instance.GameInfo.Last().GameType = GameInfo.GameTypeEnum.Sauspiel;
            await Navigation.PushAsync(new WhilePlayingPage());
        }

        public class ChooseGameViewModel
        {
            public bool isSoloAllowed { get; set; }
            public bool isWenzAllowed { get; set; }
            public bool isSauspielAllowed { get; set; }
        }
    }
}