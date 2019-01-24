using System;
using Xamarin.Forms;
using ChupeLupe.ViewModels;
using ChupeLupe.Helpers;

namespace ChupeLupe.Views
{
    public partial class PromotionsList : ContentPage
    {
        PromotionsListViewModel _vm;

        public PromotionsList()
        {
            InitializeComponent();
            _vm = new PromotionsListViewModel(Navigation, new DependencyServiceWrapper());
            BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.GetPromotionsCommand.Execute(null);
        }
    }
}
