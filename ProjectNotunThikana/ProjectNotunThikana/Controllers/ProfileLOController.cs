using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectNotunThikana.Controllers
{
    public class ProfileLOController : Controller
    {
        // GET: ProfileLO
       public static ArrayList lat = new ArrayList();
       public static ArrayList lng = new ArrayList();
        public static ArrayList Post = new ArrayList();
        public static ArrayList PostFlat = new ArrayList();

        class Details : Models.Database
        {

            

            public String insertValues(String FlatName, String Address, String city, String Advanceinfio, String MonthlyRateinfo, String ServiceChargeInfo, String electricbillinfo, String gasbill,String Email)
            {
                String Details = "";
                String Msg="Welcome";

                Models.LatLng lt = new Models.LatLng();
                try
                {
                    if (Empty(FlatName) != 0 && Empty(Address) != 0 && Empty(city) != 0)
                    {
                        if((Empty(MonthlyRateinfo)==0 && Empty(electricbillinfo)==0 && Empty(gasbill) == 0))
                        {
                            Details = "For Details Contact Land Owner";
                        }
                        else
                        {
                            if (Empty(ServiceChargeInfo) == 0)
                            {
                                ServiceChargeInfo = "0.0 Taka";
                            }
                            Details = "A Flat Will Rented by" + FlatName + ", Address: " + Address + " City: " + city + " With Monthly Rate of " + MonthlyRateinfo + " Pre Advance of " + Advanceinfio + ", Electric Bill : " + electricbillinfo + " And Gas Bill: " + gasbill +" With Service Charge: "+ServiceChargeInfo;
                        }
                        GoogleMap gm = new GoogleMap();
                        String[] x = gm.latlng(Address);

                        
                        
                            if (x[0].Length != 0 && x[1].Length != 0)
                            {

                            Boolean svr = true;

                            DatabaseCon("NotunThikana");

                            getData("Select Address,FlatName from GoogleMap where Email='" +Email+"'");

                            while (reading.Read())
                            {
                                if (lt.StrCheck(reading[0].ToString()).Equals(lt.StrCheck(Address)) && lt.StrCheck(reading[1].ToString()).Equals(lt.StrCheck(FlatName)))
                                {
                                    svr = false;
                                    Msg = "Seems That This Flat has Already on Advertisment.";
                                  ///  DatabaseCon("NotunThikana").Close();
                                    break;
                                    //break;
                                }
                            }
                            DatabaseCon("NotunThikana").Close();

                            if (svr == true)
                            {

                                DatabaseCon("NotunThikana");
                                setData("Insert into GoogleMap(Email,FlatName,Address,city,Details,Lat,Lng) values(" + "'" + Email + "'," + "'" + FlatName + "'," + "'" + Address + "'," + "'" + city + "'," + "'" + Details + "'," + "'" + x[0] + "'," + "'" + x[1] + "'" + ")");
                                Msg = "Thank You For the Advatisement";
                                Details = "";
                                return Msg;
                            }
                            else if(svr==false)
                            {
                              //  Msg = "Seems That This Flat has Already on Advertisment.\n Change The Flat Name/NO";
                                return Msg;
                            }
                            }
                        
                        else
                        {
                            Msg = "Server Error Please Try Again";
                            return Msg;
                        }
                    }

                }
                catch(Exception e)
                {
                    // Error = e.ToString();
                    Msg = e.ToString();
                    return Msg;
                }
                
               
                return Msg;
            }
            public void getGoogleMapPointer(String Email)
            {
                
                DatabaseCon("NotunThikana");
                getData("Select Lat,Lng from GoogleMap where Email='"+Email+"'");
                while (reading.Read())
                {

                    lat.Add( (string)reading[0]);
                    lng.Add ((string)reading[1]);
                   
                }
                DatabaseCon("NotunThikana").Close();

                
            }
            public void getPost(String Email)
            {
                
                
                DatabaseCon("NotunThikana");
                getData("Select Address,FlatName from GoogleMap where Email='" + Email + "'");
                while (reading.Read())
                {

                    Post.Add((String)reading[0]);
                    PostFlat.Add((String)reading[1]);

                }

                DatabaseCon("NotunThikana").Close();

               
            }

            public String getDetails(String Address , String FlatName, String Mail)
            {

                String x="";

                DatabaseCon("NotunThikana");

                getData("Select Details from GoogleMap where Address= '" + Address + "' and FlatName = '" + FlatName + "'  and Email='"+Mail +"'");

                while (reading.Read())
                {
                    x = reading[0].ToString();
                    break;
                }

                DatabaseCon("NotunThikana").Close();
                return x;

            }

           
           
            
           
        }
        class GoogleMap : Models.LatLng
        {
            public String[] latlng(String Address)
            {
               return GetFirstLastName(Address);
            }
              
        }

       
        public ActionResult ProfileLandOwner(String FlatName, String Address, String city, String Advanceinfio, String MonthlyRateinfo, String ServiceChargeInfo,String electricbillinfo, String gasbill)
        {
            String jsPoints="";
            Models.LatLng gm = new Models.LatLng();
            Details d = new Details();
            Models.ProfilePictures pic = new Models.ProfilePictures();

            
           
            if (Session["Name"] == null && Session["Email"]==null)
            {
                return Redirect("~/HomePage/getHomeData");
            }
            if (Session["User"].ToString().Contains("Rental"))
            {
                Response.Redirect("~/ProfileRe/ProfileRe");
            }

            gm.getImg=pic.getUserProfilePic(Session["Email"].ToString(),"UserProfilePic");
            gm.getCover = pic.getUserProfilePic(Session["Email"].ToString(), "UserProfileCover");

            gm.Name = FlatName;
                gm.Address = Address;
                gm.city = city;
               
                gm.Email = Session["Email"].ToString();
              
                
                
                ViewBag.Message = d.insertValues(gm.Name,gm.Address,gm.city,Advanceinfio,MonthlyRateinfo,ServiceChargeInfo,electricbillinfo,gasbill,gm.Email);
                d.getGoogleMapPointer(Session["Email"].ToString());
                d.getPost(Session["Email"].ToString());
                 for(int i = 0; i < Post.Count; i++)
            {
                gm.PostsofFlat.Add((String)Post[i]);
                gm.FlatName.Add((String)PostFlat[i]);
                gm.getDetailsList.Add(d.getDetails((String)Post[i],(String)PostFlat[i], Session["Email"].ToString()));
                
            }
            Post.Clear();
            PostFlat.Clear();

            for (int i = 0; i < lat.Count; i++)
            {
                jsPoints += " var marker = new google.maps.Marker({\n" +
    "            position: { lat: " + lat[i] + ", lng: " + lng[i] + " },\n" +
    "            map : map\n" +
    "        });";
            }


            gm.Operation = jsPoints;
            lat.Clear();
            lng.Clear();


            return View(gm);
        }
        public ActionResult LogOut()
        {

            Session["Name"] = null;
            Session["Email"] = null;
            Session["User"] = null;
            Session["Phone"] = null;
            Session["tempFlatName"] = null;
            Session["tempAddress"] = null;
            return Redirect("~/HomePage/getHomeData");
          
        }
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
           
            try
            {
                if (file.ContentLength > 0)
                {

                   
                    Models.LatLng lt = new Models.LatLng();
                    Models.ProfilePictures pic = new Models.ProfilePictures();



                    string path = Path.Combine(Server.MapPath("~/Content/UserProfileNotunThikana"), lt.UploadEx(Session["Email"].ToString())+""+Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    pic.setProfilePic(Session["Email"].ToString(), lt.UploadEx(Session["Email"].ToString()) + "" + Path.GetFileName(file.FileName),"UserProfilePic");
                    

                }
                else
                {

                }


            }
            catch (Exception e)
            {

            }



            return Redirect("~/ProfileLO/ProfileLandOwner");
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



            return Redirect("~/ProfileLO/ProfileLandOwner");
        }


    }

}
