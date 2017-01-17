using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Facebook;


namespace IdealMall.Controllers
{
    public class FbLoginController : Controller
    {
        public ActionResult LoginWithFacebook()
        {
            string req = "https://www.facebook.com/dialog/oauth?client_id=" +
                         System.Web.Configuration.WebConfigurationManager.AppSettings["facebookappid"] +
                         "&redirect_uri=http://" +
                         System.Web.Configuration.WebConfigurationManager.AppSettings["DomainName"] +
                         "/FbLogin/Returnfromfb&scope=user_about_me,user_hometown,user_location,email";
            return Redirect(req);
        }

        public ActionResult Returnfromfb()
        {
            string code = Request.QueryString["code"];
            if (code == null)
            {
                return RedirectToAction("PublicLogin", "Public");
            }

            string accessToken = "";
            try
            {

                string str = "https://graph.facebook.com/oauth/access_token?client_id=" +
                             System.Web.Configuration.WebConfigurationManager.AppSettings["facebookappid"] +
                             "&redirect_uri=http://" + Request.Url.Authority + "/FbLogin/Returnfromfb&client_secret=" +
                             System.Web.Configuration.WebConfigurationManager.AppSettings["facebookappsecret"] +
                             "&code=" + code;
                var req = (HttpWebRequest)WebRequest.Create(str);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] Param = Request.BinaryRead(System.Web.HttpContext.Current.Request.ContentLength);
                string strRequest = System.Text.Encoding.ASCII.GetString(Param);

                req.ContentLength = strRequest.Length;

                var streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
                streamOut.Write(strRequest);
                streamOut.Close();
                var streamIn = new StreamReader(req.GetResponse().GetResponseStream());
                string strResponse = streamIn.ReadToEnd();
                if (strResponse.Contains("&expires"))
                    strResponse = strResponse.Substring(0, strResponse.IndexOf("&expires"));
                accessToken = strResponse.Replace("access_token=", "");
                streamIn.Close();

                var fbClient = new FacebookClient(accessToken);
                dynamic fbUserDetails = fbClient.Get("me", new { fields = "name,email" });

                try
                {
                    if (fbUserDetails != null)
                    {
                        if (string.IsNullOrWhiteSpace(fbUserDetails.email))
                        {
                            return RedirectToAction("Index", "Home",
                                new
                                {
                                    msg =
                                        "You should give access to retrieve your primary email address in facebook settings in order to login into DealzLocal through your facebook account."
                                });
                        }
                        Session.Add("PublicUserName", fbUserDetails.email);
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Home",
                        new
                        {
                            msg = "An exception occured while authenticating through Facebook Login."
                        });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home",
                        new
                        {
                            msg = "An exception occured while authenticating through Facebook Login."
                        });
            }
            return RedirectToAction("PublicIndex", "Public");
        }
    }
}
