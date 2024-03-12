using Schafkopfrechner.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schafkopfrechner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppStartPage : ContentPage
	{
		public AppStartPage ()
		{
			this.BackgroundImageSource = App.BackgroundFilename;

            InitializeComponent ();
		}

        private async void OnNewGameClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPlayerPage());
        }
    }
}