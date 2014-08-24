using Newtonsoft.Json;
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

        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { _category = value; RaisePropertyChanged(); }
        }

        public int CategoryId { get { return Category.CategoryId; } }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value.ToLower().Trim(); }
        }

        public double Price { get; set; }
        
        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set { _unit = value.ToLower().Trim(); }
        }

        private double _quantity;
        [JsonIgnore]
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                if (value <= 1)
                    _quantity = 1;
                else if (value > 999)
                    _quantity = 999;
                else
                    _quantity = value; 

                RaisePropertyChanged();
            }
        }

        private double _unitQuantity;
        public double UnitQuantity
        {
            get { return _unitQuantity; }
            set
            {
                if (value <= 0)
                    _unitQuantity = 1;
                else if (value > 999)
                    _unitQuantity = 999;
                else
                    _unitQuantity = value;

                RaisePropertyChanged();
            }
        }

        public bool IsCompleted { get; set; }
        public bool IsEnabled { get; set; }
        public string Date { get; set; }

        [JsonIgnore]
        public string Key
        {
            get { return Category.Name; }
        }


        public Product()
        {

        }

        public Product(int shopID)
        {
            ProductID = -1;
            ShopID = shopID;
            Category = new Category(shopID);
            Name = "";
            Price = 0;
            Unit = "";
            Quantity = 1;
            UnitQuantity = 1;
            Date = DateTime.Now.ToString();
            IsEnabled = false;
            IsCompleted = false;
        }

        public Product(int productID, int shopID, Category category, string name, double price, int quantity, string unit, string date)
        {
            ProductID = productID;
            ShopID = shopID;
            Category = category;
            Name = name;
            Price = price;
            Unit = unit;
            Quantity = quantity;
            UnitQuantity = 1;
            Date = date;
            IsEnabled = false;
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
