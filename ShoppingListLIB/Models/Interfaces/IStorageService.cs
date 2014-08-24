﻿using ShoppingListLIB.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.Models.Interfaces
{
    public interface IStorageService
    {
        void StoreProducts(int shopID, List<Product> products, bool isAll);
        Task<List<Product>> LoadProducts(int shopID, bool isAll);
        void StoreShops(List<Shop> shops);
        Task<List<Shop>> LoadShops();
        void RemoveShopItems(int shopID);
        void StoreCategories(List<Category> categories, int shopId);
        Task<List<Category>> LoadCategories(int shopId);
    }
}
