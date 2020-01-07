using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectNotunThikana.Controllers
{
    public class GotoUserController : Controller
    {

        private class getInfo : Models.Database{


            public String Name;

            public String User;

            public String Phone;

            public void getInformation(String mail)
            {
                DatabaseCon("NotunThikana");

                getData("Select Name,User1,PhoneNum from RegistrationTable where Email='"+mail+"'");

                while (reading.Read())
                {
                    this.Name = reading[0].ToString(); 
                    this.User = reading[1].ToString();
                    this.Phone = reading[2].ToString();
                }
                DatabaseCon("NotunThikana").Close();
            }

            public String[] getLatLng(String Address)
            {

                String[] x = new String[3];

                DatabaseCon("NotunThikana");

                getData("Select Lat,Lng from GoogleMap where Address='" + Address + "'");

                while (reading.Read())
                {
                    x[0] = reading[0].ToString();
                    x[1] = reading[1].ToString();
                    break;
                }

                DatabaseCon("NotunThikana").Close();

                return x;

            }
            public String getProfilePic(String Email)
            {
                String img="/Empty/";

                DatabaseCon("NotunThikana");

                getData("Select UserProPic from UserProfilePic where Email='" + Email + "'");

                while (reading.Read())
                {
                    img = reading[0].ToString();

                    break;
                }
                DatabaseCon("NotunThikana").Close();

                return img;
            }

            public String getProfileCover(String mail)
            {
                String img = "/Empty/";

                DatabaseCon("NotunThikana");

                getData("Select UserProPic from UserProfileCover where Email='" + mail + "'");

                while (reading.Read())
                {
                    img = reading[0].ToString();

                    break;
                }
                DatabaseCon("NotunThikana").Close();
                return img;
            }
        }
        // GET: GotoUser
        public ActionResult GotoUser()
        {

            if(Session["Name"] == null && Session["Email"] == null && Session["User"]==null && Session["Phone"]==null)
            {

                Response.Redirect("~/HomePage/getHomeData");

            }

            Models.GotoUser gt = new Models.GotoUser();

            getInfo gi = new getInfo();

            String Address = Request.Params["Address"].ToString();

            String mail = Request.Params["tailo"].ToString();

            String[] GoogleMapPoint = new string[3];

            gt.Address = Address;

            gt.mail=gt.Decrypt(mail);

            gt.getImg=gi.getProfilePic(gt.mail);

            gt.getCover=gi.getProfileCover(gt.mail);

            gi.getInformation(gt.mail);

            gt.Name = gi.Name;

            gt.User = gi.User;

            gt.Phone = gi.Phone;

            GoogleMapPoint = gi.getLatLng(gt.Address);

            gt.Operation = " var marker = new google.maps.Marker({\n" +
   "            position: { lat: " + GoogleMapPoint[0] + ", lng: " + GoogleMapPoint[1] + " },\n" +
   "            map : map\n" +
   "        });";


            return View(gt);
        }
    }
}