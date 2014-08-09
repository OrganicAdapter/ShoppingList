using ShoppingListLIB.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.Models.Interfaces
{
    public interface IDataService
    {
        Task<List<Shop>> GetShops();
        Task<List<Product>> GetProducts(int shopID);
    }
}
