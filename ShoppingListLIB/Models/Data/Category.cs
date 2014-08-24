using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.Models.Data
{
    public class Category
    {
        public int CategoryId { get; set; }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value.ToUpper().Trim(); }
        }

        public int ShopId { get; set; }

        public bool IsEnabled { get; set; }


        public Category()
        {

        }

        public Category(int shopId)
        {
            ShopId = shopId;
        }
    }
}
