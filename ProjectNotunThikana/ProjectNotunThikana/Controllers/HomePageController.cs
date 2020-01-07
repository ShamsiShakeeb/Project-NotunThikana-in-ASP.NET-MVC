using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectNotunThikana.Controllers
{
    public class HomePageController : Controller
    {

        Models.HomePage h = new Models.HomePage();
        public static String Name="";
        public static String Email1 = "";
        public static String User = "";
        public static String Phone = "";
       

        class Login : Models.Database
        {
            Models.HomePage h = new Models.HomePage();
            HomePageController hm = new HomePageController();

            public String er = "";
            public int CheckCustomer(String Email,String Password)
            {
                int count = 0;
                try
                {
                    if(Empty(Email)!=0 && Empty(Password) != 0)
                    {

                       
                            DatabaseCon("NotunThikana");
                               getData("Select Name,Email,User1,PhoneNum from RegistrationTable Where Email= '"+Email  +"' and Password = '"+Password+"'");
                               while (reading.Read())
                               {

                                   Name = (string)reading[0];
                                   Email1 = (string)reading[1];
                                   User = (string)reading[2];
                                   Phone = (string)reading[3];
                                   count++;
                                   break;
                               }
                               
                            DatabaseCon("NotunThikana").Close();

                           
                       
                        return count;
                    }
                   
                }
                catch(Exception e)
                {
                    count = 100;
                    er = e.ToString();
                    return count;
                }

                return count;
               
            }
        }


        // GET: HomePage
        public ActionResult getHomeData(String a , String b)
        {

            Login l = new Login();

            h.Zero = "Vai Ami So Close Yet So Far";

            if (l.CheckCustomer(a, b) != 0 && l.CheckCustomer(a, b) != 100)
            {
                Session["Name"] = Name;
                Session["Email"] = Email1;
                Session["User"] = User;
                Session["Phone"] = Phone;
                if(Session["User"].ToString().Contains("Land Owner"))
                return Redirect("~/ProfileLO/ProfileLandOwner");
                else if (Session["User"].ToString().Contains("Rental"))
                {
                    
                    return Redirect("~/ProfileRe/ProfileRe");

                }
                
            }
            else
            {
                if (l.CheckCustomer(a, b) != 100)
                {
                   
                    return Redirect("~/HomePage/getHomeData#Login");
                }
                else
                {
                    h.Zero = l.er;
                    return View(h);
                }
            }
            return View(h);
        }
    }
}
 