using ProjectNotunThikana.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProjectNotunThikana.Controllers
{
    public class ViewLOprofileController : Controller
    {


        public String FlatName="";

        public String Address="";

        

        class GetDetails : Models.Database
        {

            public ArrayList mail = new ArrayList();

            public ArrayList info = new ArrayList();

            public ArrayList LoInfo = new ArrayList();

            public ArrayList FlatName = new ArrayList();

            public ArrayList ImgList = new ArrayList();

            public ArrayList[] RentalImage;

            public  void TheCapacity(int Size)
            {

                RentalImage = new ArrayList[Size + 1];

                for (int i = 0; i < Size ; i++)
                {

                    RentalImage[i] = new ArrayList();

                }

            }
            public String getDetails(String Email , String Address,String FlatName)
            {
                String info="";

                DatabaseCon("NotunThikana");

                getData("Select Details from GoogleMap where Email = '" + Email + "' and Address='" + Address + "' and FlatName= '"+FlatName+"'");

                while (reading.Read())
                {
                    info += reading[0];
                }
                DatabaseCon("NotunThikana").Close();

                return info;
            }

            public void getMails(String Address)
            {

                DatabaseCon("NotunThikana");

                getData("Select Email,Details,FlatName from GoogleMap where Address='" + Address + "'");

                while (reading.Read())
                {
                    mail.Add(reading[0].ToString());

                    info.Add(reading[1].ToString());

                    FlatName.Add(reading[2].ToString());

                }
                DatabaseCon("NotunThikana").Close();

              

            }
            public Boolean CheackTheParams(String Email,String Address,String FlatName)
            {

                LatLng lt = new LatLng();

                int count = 0;

                DatabaseCon("NotunThikana");

                getData("Select Address,FlatName from GoogleMap where Email ='" + Email + "'");

                while (reading.Read())
                {
                    if (lt.StrCheck(reading[0].ToString()).Equals(lt.StrCheck(Address)) && lt.StrCheck(reading[1].ToString()).Equals(lt.StrCheck(FlatName)))
                    {
                        count++;
                        break;
                    }

                }
                if (count == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            public void SaveImageToDb(String Email,String Address,String FlatName,String imgpath)
            {
                DatabaseCon("NotunThikana");
                setData("Insert into ImageDb(Email,Address,FlatName,imgpath) values(" +"'"+Email +"'," + "'" + Address + "'," + "'" + FlatName + "'," + "'" + imgpath + "'"+")");
            }

            public void getImgList(String Email,String Address,String FlatName)
            {

                DatabaseCon("NotunThikana");

                getData("Select imgpath from ImageDb where Email='" + Email + "' and Address='" + Address + "' and FlatName='" + FlatName + "'");

                while (reading.Read())
                {
                    ImgList.Add(reading[0].ToString());
                }
                DatabaseCon("NotunThikana").Close();
                 
            }
            public void getImageList(String Email,String Address,String FlatName,int index)
            {
                DatabaseCon("NotunThikana");

                getData("Select imgpath from ImageDb where Email='" + Email + "' and Address='" + Address + "' and FlatName='" + FlatName + "'");

                while (reading.Read())
                {
                    RentalImage[index].Add(reading[0].ToString());
                }
                DatabaseCon("NotunThikana").Close();
            }
        }
        // GET: ViewLOprofile
        public ActionResult viewLo()
        {

            //

            if (Session["Name"] == null && Session["Email"] == null && Session["Address"] == null)
            {
                Response.Redirect("~/HomePage/getHomeData");
            }

            if (Session["User"].ToString().Contains("Land Owner"))
            {
                String FlatName = Request.Params["FlatName"].ToString();

                Models.ViewProfile vp = new Models.ViewProfile();

                vp.Address = Request.Params["Address"].ToString();

                vp.FlatName = FlatName;




                GetDetails gd = new GetDetails();


                if (gd.CheackTheParams(Session["Email"].ToString(), vp.Address, vp.FlatName) == true)
                {

                    Session["tempFlatName"] = vp.FlatName;

                    Session["tempAddress"] = vp.Address;

                    vp.theInfo = gd.getDetails(Session["Email"].ToString(), vp.Address, vp.FlatName);

                    gd.getImgList(Session["Email"].ToString(), Session["tempAddress"].ToString(), Session["tempFlatName"].ToString());

                    for(int i = 0; i < gd.ImgList.Count; i++)
                    {
                        vp.ImgList.Add(gd.ImgList[i].ToString());
                    }

                    gd.ImgList.Clear();

                    gd.LoInfo.Clear();

                    return View(vp);
                }
                else
                {
                    return Redirect("~/ProfileLO/ProfileLandOwner");
                }
            }
            else if (Session["User"].ToString().Contains("Rental"))
            {
                Models.ViewProfile vp = new Models.ViewProfile();

                vp.Address = Request.Params["Address"].ToString();

                GetDetails gd = new GetDetails();

                gd.getMails(vp.Address);


                Session["tempFlatName"] = null;

                Session["tempAddress"] = null;

                gd.TheCapacity(gd.mail.Count);

                for (int i = 0; i < gd.mail.Count; i++)
                {
                    vp.Mails.Add(vp.Encrypt(gd.mail[i].ToString()));

                    vp.InfoAll.Add(gd.info[i].ToString());

                    vp.FlatNames.Add(gd.FlatName[i]);

                    gd.getImageList(gd.mail[i].ToString(), Request.Params["Address"].ToString(), gd.FlatName[i].ToString(),i);
                }
               

                vp.RentalimgList = new ArrayList[gd.mail.Count + 1];

                for(int i = 0; i < gd.mail.Count; i++)
                {
                    vp.RentalimgList[i] = new ArrayList();
                }

                for (int i = 0; i < gd.mail.Count; i++)
                {
                    vp.Test += gd.RentalImage[i].Count;
                    for (int j = 0; j < gd.RentalImage[i].Count; j++)
                    {
                        vp.RentalimgList[i].Add(gd.RentalImage[i][j].ToString());
                    }
                }


                vp.mail = vp.Mails.ToArray();

                vp.theInfoAll = vp.InfoAll.ToArray();
               

                gd.mail.Clear();

                gd.info.Clear();

                gd.FlatName.Clear();

                for(int i = 0; i < gd.mail.Count; i++)
                {
                    gd.RentalImage[i].Clear();
                }

                return View(vp);
            }
            
                                                                                                                                                                                                       



            


            return View();
        }
        
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            try
            {
                    if (file.ContentLength > 0)
                    {

                    GetDetails gd = new GetDetails();
                    Models.LatLng lt = new Models.LatLng();

                        string path = Path.Combine(Server.MapPath("~/Content/NotunThikanaFile"), lt.UploadEx(Session["tempAddress"].ToString()+""+Session["tempFlatName"].ToString()+""+Path.GetFileName(file.FileName)));
                        file.SaveAs(path);
                        gd.SaveImageToDb(Session["Email"].ToString(), Session["tempAddress"].ToString(), Session["tempFlatName"].ToString(), lt.UploadEx(Session["tempAddress"].ToString() + "" + Session["tempFlatName"].ToString() + "" + file.FileName));

                    }
                    else
                    {

                    }
                
                
            }
            catch (Exception e)
            {

            }



            return Redirect("~/ViewLOprofile/viewLo?Address="+Session["tempAddress"] +"&FlatName="+Session["tempFlatName"]);
        }


    }
}