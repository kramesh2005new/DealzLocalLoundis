using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using IdealMall.DataAccess;
using IdealMall.Entities;

namespace IdealMall.BusinessAccess
{
    public static class TradeShoppingBA
    {
        public static List<ShoppingList> GetTradeShoppingList(string UserId)
        {
            List<ShoppingList> lstShoppingLst = new List<ShoppingList>();
            lstShoppingLst = TradeShoppingDA.GetTradeShoppingList(UserId);

            if (lstShoppingLst != null)
            {
                foreach (ShoppingList objList in lstShoppingLst)
                {
                    if (!string.IsNullOrWhiteSpace(objList.Additional_Info))
                    {
                        objList.Additional_Info = "[" + objList.Additional_Info + "]";
                    }
                }
            }
            return lstShoppingLst;
        }

        public static bool sendMail(string sender, string recipient, string cc, string subject, string body)
        {
            MailMessage objMessage = new MailMessage();
            objMessage.From = new MailAddress(sender);
            objMessage.To.Add(new MailAddress(recipient));
            if (!string.IsNullOrWhiteSpace(cc))
                objMessage.CC.Add(new MailAddress(cc));
            objMessage.Subject = subject;
            objMessage.Body = body;
            objMessage.IsBodyHtml = true;
            Mailer objMailer = new Mailer();
            return objMailer.sendMail(objMessage);
        }

        public static List<CashandCarryShop> GetCashAndCarryShops()
        {
            return TradeShoppingDA.GetCashAndCarryShops();
        }
    }
}
