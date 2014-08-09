using ShoppingListLIB.Models.Data;
using ShoppingListLIB.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.Models.Implementations
{
    public class SampleDataService : IDataService
    {
        #region Properties

        public List<Shop> Shops { get; set; }
        public List<Product> Products { get; set; }

        #endregion //Properties

        #region Constructor

        public SampleDataService()
        {
            Shops = new List<Shop>() 
            { 
                new Shop(0, "Spar", "hun"),
                new Shop(1, "Tesco", "hun")
            };

            Products = new List<Product>()
            {
                new Product(0, 0, "Tejtermék", "Sajt", 325, 50, "dkg", DateTime.Parse("2014.05.08.")),
                new Product(1, 0, "Hús", "Sonka", 200, 20, "dkg", DateTime.Parse("2014.07.23.")),
                new Product(2, 1, "Tejtermék", "Tej", 179, 1, "L", DateTime.Parse("2014.06.09.")),
                new Product(3, 1, "Édesség", "Mars", 299, 1, "db", DateTime.Parse("2014.08.01."))
            };
        }

        #endregion //Constructor

        #region Methods

        public async Task<List<Shop>> GetShops()
        {
            return Shops;
        }

        public async Task<List<Product>> GetProducts(int shopID)
        {
            return Products.Where((product) => product.ShopID == shopID).ToList();
        }

        #endregion //Methods
    }
}
