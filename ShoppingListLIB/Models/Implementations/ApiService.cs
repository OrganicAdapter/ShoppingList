using ShoppingListLIB.Models.Data;
using ShoppingListLIB.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalExtensions;

namespace ShoppingListLIB.Models.Implementations
{
    public class ApiService : IApiService
    {
        #region Fields

        #endregion //Fields

        #region Properties

        private HttpClientService Service { get; set; }
        public Random Rnd { get; set; }

        #endregion //Properties

        #region Constructor

        public ApiService()
        {
            Service = new HttpClientService("http://collectiveshopping.softit.hu/WCF/ShoppingListService.svc/");
            //Service = new HttpClientService("http://localhost:19768/WCF/ShoppingListService.svc/");
            Rnd = new Random();
        }

        #endregion //Constructor

        #region Methods

        public async Task<List<Shop>> SyncShops(List<Shop> shops)
        {
            var newShops = await Service.GetRequest("SyncShops?userShops=" + await JsonSerializer.Serialize<Shop>(shops) + "&culture=" + CultureInfo.CurrentCulture.ToString() + "&rnd=" + Rnd.Next(1000));

            if (newShops == null)
                return null;
            else
                return await JsonSerializer.Deserialize<List<Shop>>(CutJson(newShops));
        }

        public async Task<List<Product>> SyncProducts(List<Product> products, int shopId)
        {
            var newProducts = await Service.GetRequest("SyncProducts?userProducts=" + await JsonSerializer.Serialize<Product>(products) + "&shopId=" + shopId + "&rnd=" + Rnd.Next(1000));

            if (newProducts == null)
                return null;
            else
                return await JsonSerializer.Deserialize<List<Product>>(CutJson(newProducts));
        }

        public async Task<List<Category>> SyncCategories(List<Category> categories, int shopId)
        {
            var newCategories = await Service.GetRequest("SyncCategories?userCategories=" + await JsonSerializer.Serialize<Category>(categories) + "&shopId=" + shopId + "&rnd=" + Rnd.Next(1000));

            if (newCategories == null)
                return null;
            else
                return await JsonSerializer.Deserialize<List<Category>>(CutJson(newCategories));
        }

        public async Task<int> SyncShop(Shop shop)
        { 
            var shopId = await Service.GetRequest("SyncShop?userShop=" + await JsonSerializer.Serialize(shop) + "&rnd=" + Rnd.Next(1000));

            if (shopId == null)
                return -1;
            else
                return int.Parse(CutJson(shopId));
        }

        private string CutJson(string json)
        {
            var idx0 = json.IndexOf('<');
            var idx1 = json.IndexOf('>');

            json = json.Remove(idx0, idx1 - idx0 + 1);

            idx0 = json.IndexOf('<');
            idx1 = json.IndexOf('>');

            json = json.Remove(idx0, idx1 - idx0 + 1);

            return json;
        }

        #endregion //Methods

    }
}
