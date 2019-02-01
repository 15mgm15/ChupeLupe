﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ChupeLupe.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ChupeLupe
{
    public partial class App : Application
    {
        public static bool IsInit { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PromotionsList());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
