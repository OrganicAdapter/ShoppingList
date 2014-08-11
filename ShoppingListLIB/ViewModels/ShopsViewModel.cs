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
        private readonly IStorageService _storageService;

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


        private Shop _newShop;
        public Shop NewShop
        {
            get { return _newShop; }
            set { _newShop = value; RaisePropertyChanged(); }
        }


        public RelayCommand Load { get; set; }
        public RelayCommand Add { get; set; }
        public RelayCommand Save { get; set; }
        public RelayCommand<Shop> Delete { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand<Shop> Open { get; set; }

        #endregion //Properties

        #region Constructor

        public ShopsViewModel(IDataService dataService, INavigationService navigationService, IStorageService storageService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _storageService = storageService;

            NewShop = new Shop();

            Load = new RelayCommand(ExecuteLoad);
            Add = new RelayCommand(ExecuteAdd);
            Save = new RelayCommand(ExecuteSave);
            Delete = new RelayCommand<Shop>(ExecuteDelete);
            Cancel = new RelayCommand(ExecuteCancel);
            Open = new RelayCommand<Shop>(ExecuteOpen);
        }

        #endregion //Constructor

        #region Methods

        private async void ExecuteLoad()
        {
            Shops = Converter.ListToObservableCollection<Shop>(await _storageService.LoadShops());
            IsAdding = false;
        }

        private void ExecuteAdd()
        {
            IsAdding = true;
        }

        private void ExecuteSave()
        {
            IsAdding = false;
            NewShop.ShopID = Shops.Count;
            Shops.Add(NewShop);
            NewShop = new Shop();

            _storageService.StoreShops(Shops.ToList());
        }

        private void ExecuteCancel()
        {
            IsAdding = false;
        }

        private void ExecuteDelete(Shop shop)
        {
            Shops.Remove(shop);

            _storageService.RemoveShopItems(shop.ShopID);
            _storageService.StoreShops(Shops.ToList());
        }

        private void ExecuteOpen(Shop shop)
        {
            Main.Shop = shop;
            _navigationService.Navigate("ShoppingList", "Products");
        }

        #endregion //Methods
    }
}
