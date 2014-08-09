using ShoppingListLIB.Models.Data;
using ShoppingListLIB.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalExtensions;
using UniversalExtensions.MVVM;
using UniversalExtensions.Navigation;

namespace ShoppingListLIB.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDataService _dataService;

        #endregion //Fields

        #region Properties

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Product> _myProducts;
        public ObservableCollection<Product> MyProducts
        {
            get { return _myProducts; }
            set { _myProducts = value; RaisePropertyChanged(); }
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


        public Product NewProduct { get; set; }


        public RelayCommand Load { get; set; }
        public RelayCommand Add { get; set; }
        public RelayCommand<Product> AddItem { get; set; }
        public RelayCommand Hide { get; set; }

        #endregion //Properties

        #region Constructor

        public ProductsViewModel(IDataService dataService)
        {
            _dataService = dataService;

            NewProduct = new Product(Main.ShopID);

            Load = new RelayCommand(ExecuteLoad);
            Add = new RelayCommand(ExecuteAdd);
            AddItem = new RelayCommand<Product>(ExecuteAddItem);
            Hide = new RelayCommand(ExecuteHide);
        }

        #endregion //Constructor

        #region Methods

        private async void ExecuteLoad()
        {
            Products = Converter.ListToObservableCollection<Product>(await _dataService.GetProducts(Main.ShopID));
            MyProducts = new ObservableCollection<Product>();
            IsAdding = false;
        }

        private void ExecuteAdd()
        {
            if (!IsAdding)
                IsAdding = true;
            else
                IsCreatingProduct = true;
        }

        private void ExecuteAddItem(Product product)
        {
            MyProducts.Add(product);
            Products.Remove(product);
        }

        private void ExecuteHide()
        {
            if (!IsCreatingProduct)
            {
                IsAdding = false;
            }
            else
            { 
                Products.Add(NewProduct);
                IsCreatingProduct = false;
            }            
        }

        #endregion //Methods
                
    }
}
