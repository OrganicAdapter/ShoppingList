using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.Models.Data
{
    public class Product
    {
        public int ProductID { get; set; }
        public int ShopID { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }


        public Product(int shopID)
        {
            ProductID = -1;
            ShopID = shopID;
            Category = "";
            Name = "";
            Price = 0;
            Unit = "";
            Quantity = 1;
            Date = DateTime.Now;

            IsCompleted = false;
        }

        public Product(int productID, int shopID, string category, string name, double price, int quantity, string unit, DateTime date)
        {
            ProductID = productID;
            ShopID = shopID;
            Category = category.ToUpper();
            Name = name.ToLower();
            Price = price;
            Unit = unit.ToLower();
            Quantity = quantity;
            Date = date;

            IsCompleted = false;
        }
    }
}
