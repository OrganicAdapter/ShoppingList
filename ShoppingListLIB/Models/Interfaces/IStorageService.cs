using ShoppingListLIB.Models.Data;
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
    }
}
