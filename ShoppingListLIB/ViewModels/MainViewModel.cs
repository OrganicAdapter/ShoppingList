using ShoppingListLIB.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.ViewModels
{
    public class MainViewModel
    {
        private static MainViewModel _instance;
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainViewModel();

                return _instance;
            }
        }

        public Shop Shop { get; set; }
    }
}
