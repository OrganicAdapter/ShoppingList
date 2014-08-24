using ShoppingListLIB.Models.Data;
using ShoppingListLIB.Models.Interfaces;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Chat;
using Windows.ApplicationModel.Email;

namespace ShoppingList.Implementation
{
    public class ChatService : IChatService
    {
        public async void SendSms(List<Product> products)
        {
            ChatMessage msg = new ChatMessage();
            msg.Body = CreateMessageBody(products);
            await ChatMessageManager.ShowComposeSmsMessageAsync(msg);
        }

        public async void SendEmail(List<Product> products)
        {
            EmailMessage mail = new EmailMessage();
            mail.Subject = "Collective Shopping";
            mail.Body = CreateMessageBody(products);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private string CreateMessageBody(List<Product> products)
        {
            var str = "";

            foreach (var item in products)
            {
                str += item.Name + "\t" + item.Quantity + item.Unit + "\r\n";
            }

            return str;
        }
    }
}
