using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.Models.Data
{
    public class Shop
    {
        public int ShopID { get; set; }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value.ToUpper().Trim(); }
        }


        private string _culture;
        public string Culture
        {
            get { return _culture; }
            set { _culture = value.ToLower().Trim(); }
        }

        public bool IsEnabled { get; set; }


        public Shop()
        {
            ShopID = -1;
            Name = "";
            Culture = CultureInfo.CurrentCulture.ToString();
            IsEnabled = false;
        }

        public Shop(int shopID, string name, string culture)
        {
            ShopID = shopID;
            Name = name;
            Culture = culture;
            IsEnabled = false;
        }

        public Shop(int shopId,string culture)
        {
            ShopID = shopId;
            Name = "";
            Culture = culture;
            IsEnabled = false;
        }
    }
}
