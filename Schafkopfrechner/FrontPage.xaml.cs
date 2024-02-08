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
	public partial class FrontPage : ContentPage
	{
		public FrontPage ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
			DisplayAlert("Hallo", "Hallo, du Schafkopfer", "ok");
        }
    }
}