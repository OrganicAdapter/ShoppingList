using ShoppingListLIB.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListLIB.Models.Interfaces
{
    public interface IChatService
    {
        void SendSms(List<Product> products);
        void SendEmail(List<Product> products);
    }
}
