using ShoppingListLIB.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.Models.Interfaces
{
    public interface IApiService
    {
        Task<List<Shop>> SyncShops(List<Shop> shops);
        Task<List<Product>> SyncProducts(List<Product> products, int shopId);
        Task<List<Category>> SyncCategories(List<Category> categories, int shopId);
        Task<int> SyncShop(Shop shop);
    }
}
