using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectNotunThikana.Models
{
    public class ProfilePictures : Models.Database
    {

        public void setProfilePic(String Email, String imgpath,String DbTable)
        {

            Models.LatLng lt = new Models.LatLng();

            DatabaseCon("NotunThikana");

            getData("Select Email From "+DbTable);

            Boolean userFound = false;

            while (reading.Read())
            {
                if (lt.StrCheck(reading[0].ToString()).Equals(lt.StrCheck(Email)))
                {
                    userFound = true;
                    break;
                }
            }
            DatabaseCon("NotunThikana").Close();

            if (userFound == true)
            {
                DatabaseCon("NotunThikana");
                setData("Update "+DbTable+" set UserProPic='" + imgpath + "' where Email='" + Email + "'");
            }
            else if (userFound == false)
            {
                DatabaseCon("NotunThikana");
                setData("Insert into "+DbTable+"(Email,UserProPic) values( '" + Email + "'," + "'" + imgpath + "'" + ")");
            }

        }
        public String getUserProfilePic(String Email,String DbTable)
        {

            DatabaseCon("NotunThikana");

            String getImg = "/Empty/";

            getData("Select UserProPic from "+DbTable+" where Email='" + Email + "'");

            while (reading.Read())
            {
                getImg = reading[0].ToString();
                break;
            }
            return getImg;

        }
    }
}