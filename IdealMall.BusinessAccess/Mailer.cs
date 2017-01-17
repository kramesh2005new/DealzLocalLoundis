using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;

namespace IdealMall.BusinessAccess
{
    public class Mailer
    {
        public bool sendMail(MailMessage objMessage)
        {
            
            try
            {
                bool DefaultCredentials = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings.Get("DefaultCredentials"));
                bool SSLEnable = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings.Get("SSLEnable"));
                SmtpClient client = new SmtpClient();
                client.Host = System.Configuration.ConfigurationSettings.AppSettings.Get("ExchangeServer");
                client.UseDefaultCredentials = DefaultCredentials;
                client.EnableSsl = SSLEnable;

                if (DefaultCredentials == false)
                {
                    string username = ConfigurationSettings .AppSettings.Get("SystemUserName");
                    string pw = ConfigurationSettings.AppSettings.Get("SystemPassword");
                    client.Credentials = new System.Net.NetworkCredential(username, pw);
                }

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(objMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

    }
}
