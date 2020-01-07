using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectNotunThikana.Controllers
{
    public class RegistrationController : Controller
    {
       

        class Registration : Models.Database
        {
           
            public String insertValues(String Name, String mail, String phnNum, String Date, String Address, String city, String User, String password)
            {


                try
                {
                    if (Empty(Name) != 0 && Empty(mail) != 0 && Empty(phnNum) != 0 && Empty(Address) != 0 && Empty(password) != 0 && Empty(Date) != 0 && Empty(city) != 0 && Empty(User) != 0)
                    {
                        try
                        {
                            DatabaseCon("NotunThikana");
                            setData("Insert into RegistrationTable(Name,Email,PhoneNum,DOB,Address,City,User1,Password) Values(" + "'" + Name + "'," + "'" + mail + "'," + "'" + phnNum + "'," + "'" + Date + "'," + "'" + Address + "'," + "'" + city + "'," + "'" + User + "'," + "'" + password + "'" + ")");
                            return "Registration Complete";
                        }
                        catch(Exception e)
                        {
                            return e.ToString();
                        }
                    }
                    else
                    {
                        return "Fill up all the Section";
                    }

                }
                catch(Exception e)
                {
                    return null;
                }

                return null;
              
            }

            public String[] getUpdateInfo(String Email)
            {
                DatabaseCon("NotunThikana");
                getData("Select * From RegistrationTable where Email='"+Email+"'");

                String[] info = new String[8];

                while (reading.Read())
                {
                    info[0] = reading[0].ToString();
                    info[1] = reading[1].ToString();
                    info[2] = reading[2].ToString();
                    info[3] = reading[3].ToString();
                    info[4] = reading[4].ToString();
                    info[5] = reading[5].ToString();
                    info[6] = reading[6].ToString();
                    info[7] = reading[7].ToString();

                    break;
                }
                DatabaseCon("NotunThikana").Close();

                return info;
            }
            public String setUpdateInfo(String Name,String PhoneNum,String Address,String City,String Email)
            {
                try
                {
                    if (Empty(Name) != 0 && Empty(PhoneNum) != 0 && Empty(Address) != 0 && Empty(City) != 0)
                    {
                        DatabaseCon("NotunThikana");

                        setData("Update RegistrationTable set Name='" + Name + "',PhoneNum='" + PhoneNum + "',Address='" + Address + "',City='" + City + "' where Email='" + Email + "'");

                        return "Update Sucessfull";
                    }
                }
                catch(Exception e)
                {
                    return null;
                }
                return null;
            }

           
        }

        
        // GET: Registration

     
        
        public ActionResult RegistrationForm(String Name,String mail,String phnNum,String Date,String Address,String city,String User,String password)
        {

            Models.RegistrationClass rf = new Models.RegistrationClass();
            Registration reg = new Registration();
           

            if (Session["Name"] == null && Session["Email"] == null && Session["User"] == null && Session["Phone"] == null)
            {

                rf.Name = Name;
                rf.Email = mail;
                rf.PhnNum = phnNum;
                rf.Address = Address;
                rf.City = city;
                rf.User = User;
                rf.Dob = Date;
                rf.Password = password;

                ViewBag.Message = reg.insertValues(rf.Name, rf.Email, rf.PhnNum, rf.Dob, rf.Address, rf.City, rf.User, rf.Password);
            }
            else
            {
               

                ViewBag.Message = reg.setUpdateInfo(Name,phnNum,Address,city,Session["Email"].ToString());


                String[] info = new String[8];

                info = reg.getUpdateInfo(Session["Email"].ToString());

                rf.Name = info[0];

                rf.Email = info[1];

                rf.PhnNum = info[2];

                rf.Dob = info[3];

                rf.Address = info[4];

                rf.City = info[5];

                rf.User = info[6];

                rf.Password = info[7];


                //  reg.setUpdateInfo(info)
            }
            
            return View(rf);
        }
    }
}