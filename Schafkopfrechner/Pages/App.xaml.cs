using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schafkopfrechner
{
    public partial class App : Application
    {
        public const string BackgroundFilename = "bavarianFlag.png";
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new AppStartPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
