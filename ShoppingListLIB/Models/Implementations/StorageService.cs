using ShoppingListLIB.Models.Data;
using ShoppingListLIB.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalExtensions;
using Windows.Storage;
using Windows.UI.Xaml;

namespace ShoppingListLIB.Models.Implementations
{
    public class StorageService : IStorageService
    {
        public async void StoreProducts(int shopID, List<Product> products, bool isAll)
        {
            if (products == null || products.Count == 0)
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(shopID + "," + isAll))
                    ApplicationData.Current.LocalSettings.Values.Remove(shopID + "," + isAll);
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[shopID + "," + isAll] = await JsonSerializer.Serialize<Product>(products);
            }
        }

        public async Task<List<Product>> LoadProducts(int shopID, bool isAll)
        {
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(shopID + "," + isAll)) return new List<Product>();
            var a = ApplicationData.Current.LocalSettings.Values[shopID + "," + isAll].ToString();

            return await JsonSerializer.Deserialize<List<Product>>(ApplicationData.Current.LocalSettings.Values[shopID + "," + isAll].ToString());
        }
    }
}
