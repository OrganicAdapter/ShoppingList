using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UniversalExtensions.GroupedItems;

namespace ShoppingListLIB.Models.Data
{
    public class Product : INotifyPropertyChanged, IGroupItem
    {
        public int ProductID { get; set; }
        public int ShopID { get; set; }
        
        private string _category;
        public string Category
        {
            get { return _category; }
            set { _category = value.ToUpper(); }
        }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value.ToLower(); }
        }

        public double Price { get; set; }
        
        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set { _unit = value.ToLower(); }
        }

        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 1)
                    _quantity = 1;
                else if (value > 999)
                    _quantity = 999;
                else
                    _quantity = value; 

                RaisePropertyChanged();
            }
        }

        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }

        public string Key
        {
            get { return Category; }
        }


        public Product()
        {

        }

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
            Category = category;
            Name = name;
            Price = price;
            Unit = unit;
            Quantity = quantity;
            Date = date;

            IsCompleted = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string e = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(e));
        }
    }
}
