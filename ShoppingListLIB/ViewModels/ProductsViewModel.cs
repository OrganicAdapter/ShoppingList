using ShoppingListLIB.Models.Data;
using ShoppingListLIB.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalExtensions;
using UniversalExtensions.GroupedItems;
using UniversalExtensions.MVVM;
using UniversalExtensions.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ShoppingListLIB.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDataService _dataService;
        private readonly IStorageService _storageService;

        #endregion //Fields

        #region Properties

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; RaisePropertyChanged(); RaisePropertyChanged("GroupedProducts"); }
        }

        private ObservableCollection<Product> _myProducts;
        public ObservableCollection<Product> MyProducts
        {
            get { return _myProducts; }
            set { _myProducts = value; RaisePropertyChanged(); RaisePropertyChanged("TotalPrice"); RaisePropertyChanged("MyGroupedProducts"); }
        }

        public List<GroupedListItem<Product>> GroupedProducts
        {
            get
            {
                if (Products == null) return null;
                return GroupedListItem<Product>.GroupItems(Products.ToList());
            }
        }

        public List<GroupedListItem<Product>> MyGroupedProducts
        {
            get
            {
                if (Products == null) return null;
                return GroupedListItem<Product>.GroupItems(MyProducts.ToList());
            }
        }

        private bool _isAdding;
        public bool IsAdding
        {
            get { return _isAdding; }
            set { _isAdding = value; RaisePropertyChanged(); }
        }

        private bool _isCreatingProduct;
        public bool IsCreatingProduct
        {
            get { return _isCreatingProduct; }
            set { _isCreatingProduct = value; RaisePropertyChanged(); }
        }

        public double TotalPrice { 
            get 
            {
                if (MyProducts == null) return 0;

                double sum = 0;

                foreach (var item in MyProducts)
                {
                    sum += item.Quantity * item.Price;
                }

                return sum;
            } 
        }

        private Product _newProduct;
        public Product NewProduct
        {
            get { return _newProduct; }
            set { _newProduct = value; RaisePropertyChanged(); }
        }


        public RelayCommand Load { get; set; }
        public RelayCommand Add { get; set; }
        public RelayCommand<Product> Save { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand<Product> Edit { get; set; }
        public RelayCommand<Product> Increase { get; set; }
        public RelayCommand<Product> Decrease { get; set; }

        #endregion //Properties

        #region Constructor

        public ProductsViewModel(IDataService dataService, IStorageService storageService)
        {
            _dataService = dataService;
            _storageService = storageService;

            NewProduct = new Product(Main.Shop.ShopID);

            Load = new RelayCommand(ExecuteLoad);
            Add = new RelayCommand(ExecuteAdd);
            Save = new RelayCommand<Product>(ExecuteSave);
            Edit = new RelayCommand<Product>(ExecuteEdit);
            Cancel = new RelayCommand(ExecuteCancel);
            Increase = new RelayCommand<Product>(ExecuteIncrease);
            Decrease = new RelayCommand<Product>(ExecuteDecrease);

            (Window.Current.Content as Frame).Navigating += Navigating;
        }

        #endregion //Constructor

        #region Methods

        private async void ExecuteLoad()
        {
            Products = Converter.ListToObservableCollection<Product>(await _storageService.LoadProducts(Main.Shop.ShopID, true));
            MyProducts = Converter.ListToObservableCollection<Product>(await _storageService.LoadProducts(Main.Shop.ShopID, false));

            IsAdding = false;
        }

        private void ExecuteAdd()
        {
            if (!IsAdding)
                IsAdding = true;
            else
                IsCreatingProduct = true;
        }

        private void ExecuteSave(Product product)
        {
            if (!IsCreatingProduct)
            {
                MyProducts.Add(product);
                Products.Remove(product);

                _storageService.StoreProducts(Main.Shop.ShopID, MyProducts.ToList(), false);
                _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);

                RaisePropertyChanged("TotalPrice");
                RaisePropertyChanged("GroupedProducts");
                RaisePropertyChanged("MyGroupedProducts");

                if (Products.Count == 0)
                    IsAdding = false;
            }
            else
            {
                Products.Add(NewProduct);

                _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);

                RaisePropertyChanged("GroupedProducts");

                IsCreatingProduct = false;
            }
        }

        private void ExecuteEdit(Product product)
        {
            Products.Remove(product);
            NewProduct = product;

            IsCreatingProduct = true;

            RaisePropertyChanged("GroupedProducts");
        }

        private void ExecuteCancel()
        {
            if (!IsCreatingProduct)
                IsAdding = false;
            else
                IsCreatingProduct = false;
        }

        private void ExecuteIncrease(Product product)
        {
            product.Quantity++;

            _storageService.StoreProducts(Main.Shop.ShopID, MyProducts.ToList(), false);

            RaisePropertyChanged("TotalPrice");
        }

        private void ExecuteDecrease(Product product)
        {
            product.Quantity--;

            _storageService.StoreProducts(Main.Shop.ShopID, MyProducts.ToList(), false);

            RaisePropertyChanged("TotalPrice");
        }

        void Navigating(object sender, Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            IsCreatingProduct = false;
            IsAdding = false;
        }

        #endregion //Methods               
    }
}
