using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ShoppingList.Implementations;
using ShoppingListLIB.Models.Implementations;
using ShoppingListLIB.Models.Interfaces;
using ShoppingListLIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using UniversalExtensions.Navigation;

namespace ShoppingList.Injection
{
    public class InjectionManager
    {
        public InjectionManager()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDataService, SampleDataService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<ITypeService, TypeService>();
            SimpleIoc.Default.Register<IStorageService, StorageService>();

            SimpleIoc.Default.Register<ShopsViewModel>();
            SimpleIoc.Default.Register<ProductsViewModel>();
        }

        public ShopsViewModel Shops
        {
            get { return SimpleIoc.Default.GetInstance<ShopsViewModel>(); }
        }

        public ProductsViewModel Products
        {
            get { return SimpleIoc.Default.GetInstance<ProductsViewModel>(); }
        }
    }
}
