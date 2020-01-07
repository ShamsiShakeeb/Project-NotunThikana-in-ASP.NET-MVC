using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectNotunThikana.Controllers
{
    public class ProfileReController : Controller
    {

        class NearbyLocations : Models.Database
        {
            public ArrayList Lat = new ArrayList();
            public ArrayList Lng = new ArrayList();
            public ArrayList Address = new ArrayList();
            public ArrayList FlatName = new ArrayList();
            
           Models.ProfileRe pre = new Models.ProfileRe();
           public String Des = "";
            public void getArea(String Area)
            {
                //String[] x = new String[3];
                try
                {
                    if (Empty(Area) != 0)
                    {
                        DatabaseCon("NotunThikana");
                        getData("Select Lat,Lng,Address,FlatName from GoogleMap");
                        SearchedArea sch = new SearchedArea();
                        String[] x = sch.AreaOfLatLng(Area);
                        while (reading.Read())
                        {

                            
                           Double destination = pre.getNearbyLocations(x[0], x[1], reading[0].ToString(), reading[1].ToString());
                            Des += x[0] + "\n" +" , " +x[1]+" "+Area;

                            if (destination <= 2)
                            {
                               Lat.Add(reading[0].ToString());
                               Lng.Add(reading[1].ToString());
                               Address.Add(reading[2].ToString());
                               FlatName.Add(reading[3].ToString());
                               
                            }

                        }
                        DatabaseCon("NotunThikana").Close();
                    }
                }
                catch(Exception e)
                {

                }
            }
        }
        class SearchedArea : Models.LatLng
        {
            public String[] AreaOfLatLng(String Area)
            {
                return GetFirstLastName(Area);
            }
        }


        // GET: ProfileRe



      
        
        public ActionResult ProfileRe(String SP)
        {
            Models.ProfileRe pre = new Models.ProfileRe();
            Models.ProfilePictures pic = new Models.ProfilePictures();
            NearbyLocations NL = new NearbyLocations();
            Models.Database lt = new Models.Database();
            String jsPoints = "";
             

            if (Session["Name"]==null && Session["Email"] == null)
            {
                Response.Redirect("~/HomePage/getHomeData");
            }
            if(Session["User"].ToString().Contains("Land Owner"))
            {
                Response.Redirect("~/ProfileLO/ProfileLandOwner");
            }

            pre.getImg = pic.getUserProfilePic(Session["Email"].ToString(), "UserProfilePic");
            pre.getCover = pic.getUserProfilePic(Session["Email"].ToString(), "UserProfileCover");

            NL.getArea(SP);

            for(int i = 0; i < NL.Lat.Count; i++)
            {
                jsPoints += " var marker = new google.maps.Marker({\n" +
 "            position: { lat: " + NL.Lat[i] + ", lng: " + NL.Lng[i] + " },\n" +
 "            map : map\n" +
 "        });";
            }
            pre.Area = jsPoints;

            for(int i = 0; i < NL.Address.Count; i++)
            {
                pre.PostsofFlat.Add(NL.Address[i].ToString());
            }

            pre.Post = pre.PostsofFlat.ToArray();

            for(int i = 0; i < NL.FlatName.Count; i++)
            {
                if(i!=NL.FlatName.Count-1)
                pre.FlatNames += NL.FlatName[i]+"***";
                else
                {
                    pre.FlatNames += NL.FlatName[i];
                }
            }

            pre.des = NL.Des;
            try
            {
                if (lt.Empty(SP) != 0)
                {
                    if (lt.Empty(pre.des) == 0)
                    {
                        pre.Error = "Please Try Again or Cheack Internet Connection";
                    }
                    else
                    {
                        pre.Error = "";
                    }
                }
            }
            catch(Exception e)
            {

            }

            
            NL.Lat.Clear();
            NL.Lng.Clear();
            NL.Address.Clear();
            NL.FlatName.Clear();
            
            return View(pre);
        }

        public ActionResult LogOut()
        {
            Session["Name"] = null;
            Session["Email"] = null;
            Session["Phone"] = null;
            Session["User"] = null;

            Response.Redirect("~/HomePage/getHomeData");

            return View();
        }
        public ActionResult UploadImageCover(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {


                    Models.LatLng lt = new Models.LatLng();
                    Models.ProfilePictures pic = new Models.ProfilePictures();



                    string path = Path.Combine(Server.MapPath("~/Content/UserCoverProfileNotunThikana"), lt.UploadEx(Session["Email"].ToString()) + "" + Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    pic.setProfilePic(Session["Email"].ToString(), lt.UploadEx(Session["Email"].ToString()) + "" + Path.GetFileName(file.FileName), "UserProfileCover");


                }
                else
                {

                }


            }
            catch (Exception e)
            {

            }



            return Redirect("~/ProfileRe/ProfileRe");
        }
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {


                    Models.LatLng lt = new Models.LatLng();
                    Models.ProfilePictures pic = new Models.ProfilePictures();



                    string path = Path.Combine(Server.MapPath("~/Content/UserProfileNotunThikana"), lt.UploadEx(Session["Email"].ToString()) + "" + Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    pic.setProfilePic(Session["Email"].ToString(), lt.UploadEx(Session["Email"].ToString()) + "" + Path.GetFileName(file.FileName), "UserProfilePic");


                }
                else
                {

                }


            }
            catch (Exception e)
            {

            }



            return Redirect("~/ProfileRe/ProfileRe");
        }
    }
}