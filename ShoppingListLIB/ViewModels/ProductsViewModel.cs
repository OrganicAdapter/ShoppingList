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
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ShoppingListLIB.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDataService _dataService;
        private readonly IStorageService _storageService;
        private readonly IApiService _apiService;
        private readonly IChatService _chatService;

        #endregion //Fields

        #region Properties

        private bool IsEditing { get; set; }
        private Product EditedProduct { get; set; }


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

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; RaisePropertyChanged(); }
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

        private bool _isCreatingProduct;
        public bool IsCreatingProduct
        {
            get { return _isCreatingProduct; }
            set { _isCreatingProduct = value; RaisePropertyChanged(); }
        }

        private bool _isCreatingCategory;
        public bool IsCreatingCategory
        {
            get { return _isCreatingCategory; }
            set { _isCreatingCategory = value; RaisePropertyChanged(); }
        }


        public double TotalPrice
        {
            get
            {
                if (MyProducts == null) return 0;

                double sum = 0;

                foreach (var item in MyProducts)
                {
                    sum += item.Price * item.Quantity;
                }

                RaisePropertyChanged("RemainingPrice");

                return sum;
            }
        }

        private Product _newProduct;
        public Product NewProduct
        {
            get { return _newProduct; }
            set { _newProduct = value; RaisePropertyChanged(); }
        }

        private Category _newCategory;
        public Category NewCategory
        {
            get { return _newCategory; }
            set { _newCategory = value; RaisePropertyChanged(); }
        }


        public RelayCommand Load { get; set; }
        public RelayCommand Sync { get; set; }
        public RelayCommand Add { get; set; }
        public RelayCommand AddCategory { get; set; }
        public RelayCommand<Product> Save { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand<Product> Edit { get; set; }
        public RelayCommand<Product> Delete { get; set; }
        public RelayCommand<Product> TotalDelete { get; set; }
        public RelayCommand<Product> Increase { get; set; }
        public RelayCommand<Product> Decrease { get; set; }
        public RelayCommand ClearList { get; set; }
        public RelayCommand SendSms { get; set; }
        public RelayCommand SendEmail { get; set; }
        public RelayCommand Pin { get; set; }

        #endregion //Properties

        #region Constructor

        public ProductsViewModel(IDataService dataService, IStorageService storageService, IApiService apiService, IChatService chatService)
        {
            _dataService = dataService;
            _storageService = storageService;
            _apiService = apiService;
            _chatService = chatService;

            Load = new RelayCommand(ExecuteLoad);
            Sync = new RelayCommand(ExecuteSync);
            Add = new RelayCommand(ExecuteAdd);
            AddCategory = new RelayCommand(ExecuteAddCategory);
            ClearList = new RelayCommand(ExecuteClearList);
            Save = new RelayCommand<Product>(ExecuteSave);
            Edit = new RelayCommand<Product>(ExecuteEdit);
            Delete = new RelayCommand<Product>(ExecuteDelete);
            TotalDelete = new RelayCommand<Product>(ExecuteTotalDelete);
            Cancel = new RelayCommand(ExecuteCancel);
            Increase = new RelayCommand<Product>(ExecuteIncrease);
            Decrease = new RelayCommand<Product>(ExecuteDecrease);
            SendSms = new RelayCommand(ExecuteSendSms);
            SendEmail = new RelayCommand(ExecuteSendEmail);
            Pin = new RelayCommand(ExecutePin);

            (Window.Current.Content as Frame).Navigating += Navigating;
        }

        #endregion //Constructor

        #region Methods

        private async void ExecuteLoad()
        {
            Products = Converter.ListToObservableCollection<Product>(await _storageService.LoadProducts(Main.Shop.ShopID, true));
            MyProducts = Converter.ListToObservableCollection<Product>(await _storageService.LoadProducts(Main.Shop.ShopID, false));
            Categories = Converter.ListToObservableCollection<Category>(await _storageService.LoadCategories(Main.Shop.ShopID));

            IsCreatingCategory = false;
            IsCreatingProduct = false;
        }

        private async void ExecuteSync()
        {
            await SyncShops();
            await SyncCategories();
            await SyncProducts();
        }

        private async Task SyncShops()
        {
            Main.Shop.ShopID = await _apiService.SyncShop(Main.Shop);

            foreach (var product in MyProducts.Union(Products))
                product.ShopID = Main.Shop.ShopID;

            foreach (var category in Categories)
                category.ShopId = Main.Shop.ShopID;

            var shops = await _storageService.LoadShops();

            foreach (var shop in shops)
            {
                if (shop.Name.Equals(Main.Shop.Name))
                {
                    shop.ShopID = Main.Shop.ShopID;
                    break;
                }
            }

            _storageService.StoreShops(shops);
            _storageService.StoreCategories(Categories.ToList(), Main.Shop.ShopID);
            _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);
            _storageService.StoreProducts(Main.Shop.ShopID, MyProducts.ToList(), false);
        }

        private async Task SyncProducts()
        {
            var localProducts = Products.Union(MyProducts).ToList();
            var products = Converter.ListToObservableCollection<Product>(await _apiService.SyncProducts(Products.Union(MyProducts).ToList(), Main.Shop.ShopID));

            foreach (var product in products)
            {
                var akt = localProducts.Where((x) => x.Name.Equals(product.Name)).FirstOrDefault();

                if (akt == null)
                    Products.Add(product);
                else
                {
                    akt = product;
                    akt.Date = DateTime.Now.ToString();
                }
            }

            _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);
            RaisePropertyChanged("GroupedProducts");
        }

        private async Task SyncCategories()
        {
            var localCategories = Converter.ListToObservableCollection<Category>(await _apiService.SyncCategories(Categories.ToList(), Main.Shop.ShopID));

            foreach (var category in localCategories)
            {
                var akt = Categories.Where((x) => x.Name.Equals(category.Name)).FirstOrDefault();

                if (akt != null)
                {
                    foreach (var product in Products.Union(MyProducts))
                    {
                        if (product.Category.CategoryId == akt.CategoryId)
                            product.Category = category;
                    }

                    akt = category;
                }
                else
                {
                    if (category.IsEnabled)
                        Categories.Add(category);   
                }
            }

            _storageService.StoreCategories(Categories.ToList(), Main.Shop.ShopID);
            _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);
            _storageService.StoreProducts(Main.Shop.ShopID, MyProducts.ToList(), false);
        }

        private void ExecuteAdd()
        {
            IsCreatingProduct = true;
            NewProduct = new Product(Main.Shop.ShopID);
        }

        private void ExecuteAddCategory()
        {
            IsCreatingCategory = true;
            NewCategory = new Category(Main.Shop.ShopID);
        }

        private void ExecuteClearList()
        {
            foreach (var product in MyProducts)
            {
                Products.Add(product);
            }

            MyProducts.Clear();

            RaisePropertyChanged("TotalPrice");
            RaisePropertyChanged("GroupedProducts");
            RaisePropertyChanged("MyGroupedProducts");

            _storageService.StoreProducts(Main.Shop.ShopID, MyProducts.ToList(), false);
            _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);
        }

        private void ExecuteSave(Product product)
        {
            if (IsCreatingCategory)
            {
                Categories.Add(NewCategory);
                NewProduct.Category = NewCategory;

                _storageService.StoreCategories(Categories.ToList(), Main.Shop.ShopID);

                IsCreatingCategory = false;
            }
            else if (!IsCreatingProduct)
            {
                MyProducts.Add(product);
                Products.Remove(product);

                _storageService.StoreProducts(Main.Shop.ShopID, MyProducts.ToList(), false);
                _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);

                RaisePropertyChanged("TotalPrice");
                RaisePropertyChanged("GroupedProducts");
                RaisePropertyChanged("MyGroupedProducts");
            }
            else
            {
                NewProduct.Price = Math.Ceiling(NewProduct.Price / NewProduct.Quantity);
                NewProduct.Date = DateTime.Now.ToString();
                Products.Add(NewProduct);
                NewProduct = new Product(Main.Shop.ShopID);

                _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);

                RaisePropertyChanged("GroupedProducts");

                IsCreatingProduct = false;
                IsEditing = false;
            }
        }

        private void ExecuteEdit(Product product)
        {
            Products.Remove(product);
            NewProduct = product;
            EditedProduct = product;

            IsCreatingProduct = true;
            IsEditing = true;

            RaisePropertyChanged("GroupedProducts");
        }

        private void ExecuteDelete(Product product)
        {
            MyProducts.Remove(product);
            Products.Add(product);

            RaisePropertyChanged("MyGroupedProducts");
            RaisePropertyChanged("GroupedProducts");
            RaisePropertyChanged("TotalPrice");

            _storageService.StoreProducts(Main.Shop.ShopID, MyProducts.ToList(), false);
            _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);
        }

        private void ExecuteTotalDelete(Product product)
        {
            Products.Remove(product);
            RaisePropertyChanged("GroupedProducts");

            _storageService.StoreProducts(Main.Shop.ShopID, Products.ToList(), true);
        }

        private void ExecuteCancel()
        {
            if (IsEditing)
            {
                Products.Add(EditedProduct);
                RaisePropertyChanged("GroupedProducts");
            }

            IsEditing = false;

            if (!IsCreatingCategory)
                IsCreatingProduct = false;

            IsCreatingCategory = false;
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
        }

        private void ExecuteSendSms()
        {
            _chatService.SendSms(MyProducts.ToList());
        }

        private void ExecuteSendEmail()
        {
            _chatService.SendEmail(MyProducts.ToList());
        }

        private async void ExecutePin()
        {
            //SecondaryTile tile = new SecondaryTile("CollectiveShopping_" + Main.Shop.Name + Main.Shop.Culture);
            SecondaryTile tile = new SecondaryTile("CollectiveShopping", Main.Shop.Name, Main.Shop.Name, new Uri("ms-appx:///Assets/Logo.scale-240.png"), TileSize.Square150x150);
            await tile.RequestCreateAsync();
        }

        #endregion //Methods
    }
}
