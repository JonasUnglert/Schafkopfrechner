﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Schafkopfrechner
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new FrontPage();
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
