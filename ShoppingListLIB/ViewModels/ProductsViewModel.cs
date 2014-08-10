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
            set { _myProducts = value; RaisePropertyChanged(); RaisePropertyChanged("TotalPrice"); }
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


        public Product NewProduct { get; set; }


        public RelayCommand Load { get; set; }
        public RelayCommand Add { get; set; }
        public RelayCommand<Product> Save { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand<Product> Increase { get; set; }
        public RelayCommand<Product> Decrease { get; set; }

        #endregion //Properties

        #region Constructor

        public ProductsViewModel(IDataService dataService)
        {
            _dataService = dataService;

            NewProduct = new Product(Main.Shop.ShopID);

            Load = new RelayCommand(ExecuteLoad);
            Add = new RelayCommand(ExecuteAdd);
            Save = new RelayCommand<Product>(ExecuteSave);
            Cancel = new RelayCommand(ExecuteCancel);
            Increase = new RelayCommand<Product>(ExecuteIncrease);
            Decrease = new RelayCommand<Product>(ExecuteDecrease);
        }

        #endregion //Constructor

        #region Methods

        private async void ExecuteLoad()
        {
            Products = Converter.ListToObservableCollection<Product>(await _dataService.GetProducts(Main.Shop.ShopID));
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

        private void ExecuteSave(Product product)
        {
            if (!IsCreatingProduct)
            {
                MyProducts.Add(product);
                Products.Remove(product);

                RaisePropertyChanged("TotalPrice");
            }
            else
            {
                Products.Add(NewProduct);
                IsCreatingProduct = false;
            }
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
            RaisePropertyChanged("TotalPrice");
        }

        private void ExecuteDecrease(Product product)
        {
            product.Quantity--;
            RaisePropertyChanged("TotalPrice");
        }

        #endregion //Methods
                
    }
}
