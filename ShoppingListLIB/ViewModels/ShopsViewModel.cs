using ShoppingListLIB.Models.Data;
using ShoppingListLIB.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalExtensions;
using UniversalExtensions.MVVM;
using UniversalExtensions.Navigation;

namespace ShoppingListLIB.ViewModels
{
    public class ShopsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        #endregion //Fields

        #region Properties

        private ObservableCollection<Shop> _shops;
        public ObservableCollection<Shop> Shops
        {
            get { return _shops; }
            set { _shops = value; RaisePropertyChanged(); }
        }

        private bool _isAdding;
        public bool IsAdding
        {
            get { return _isAdding; }
            set { _isAdding = value; RaisePropertyChanged(); }
        }


        public Shop NewShop { get; set; }

        public RelayCommand Load { get; set; }
        public RelayCommand Add { get; set; }
        public RelayCommand Save { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand<Shop> Open { get; set; }

        #endregion //Properties

        #region Constructor

        public ShopsViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;

            NewShop = new Shop();

            Load = new RelayCommand(ExecuteLoad);
            Add = new RelayCommand(ExecuteAdd);
            Save = new RelayCommand(ExecuteSave);
            Cancel = new RelayCommand(ExecuteCancel);
            Open = new RelayCommand<Shop>(ExecuteOpen);
        }

        #endregion //Constructor

        #region Methods

        private async void ExecuteLoad()
        {
            Shops = Converter.ListToObservableCollection<Shop>(await _dataService.GetShops());
            IsAdding = false;
        }

        private void ExecuteAdd()
        {
            IsAdding = true;
        }

        private void ExecuteSave()
        {
            IsAdding = false;
            Shops.Add(NewShop);
            NewShop = new Shop();
        }

        private void ExecuteCancel()
        {
            IsAdding = false;
        }

        private void ExecuteOpen(Shop shop)
        {
            Main.Shop = shop;
            _navigationService.Navigate("ShoppingList", "Products");
        }

        #endregion //Methods
    }
}
