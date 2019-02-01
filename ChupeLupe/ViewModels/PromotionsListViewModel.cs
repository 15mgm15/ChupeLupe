using System;
using ChupeLupe.ViewModels.Helpers;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using ChupeLupe.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using ChupeLupe.Helpers;

namespace ChupeLupe.ViewModels
{
    public class PromotionsListViewModel : BaseViewModel
    {
        #region Properties & Commands

        public ObservableCollection<Promotion> PromotionsList { get; set; }

        public Command GetPromotionsCommand { get; set; }

        bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                SetValue(ref _isBusy, value);
                Device.BeginInvokeOnMainThread(GetPromotionsCommand.ChangeCanExecute);
            }
        }

        #endregion

        public PromotionsListViewModel(INavigation navigation, IDependencyService dependencyService) : base(navigation, dependencyService)
        {
            GetPromotionsCommand = new Command(async () => await ExecuteGetPromotionsCommand(), () => !IsBusy);
        }

        #region Methods

        async Task ExecuteGetPromotionsCommand()
        {
            try
            {
                if(IsBusy)
                {
                    return;
                }

                IsBusy = true;

                var promotions = await WebApi.GetPromotions();

                if(promotions?.Any() ?? false)
                {
                    List<Promotion> tempList = new List<Promotion>();
                    foreach (var item in promotions)
                    {
                        if(item == null)
                        {
                            continue;
                        }

                        if (item.Id != 0)
                        {
                            tempList.Add(item);
                        }
                    }
                    PromotionsList = new ObservableCollection<Promotion>(tempList);
                    OnPropertyChanged(nameof(PromotionsList));
                }

                IsBusy = false;
                App.IsInit = true;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Console.WriteLine(ex.Message);
                //TODO: AppCenter Crashes...
            }
        }

        #endregion
    }
}
