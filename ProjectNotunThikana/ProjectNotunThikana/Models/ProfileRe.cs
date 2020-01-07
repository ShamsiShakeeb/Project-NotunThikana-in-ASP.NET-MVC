using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectNotunThikana.Models
{
    public class ProfileRe
    {
        public HashSet<String> PostsofFlat = new HashSet<String>();

        public String FlatNames { set; get; }
          
        public  String Area ="";
        public String des;

        public Object[] Post;

        public String getImg { set; get; }

        public String getCover { set; get; }

        public String Error { set; get; }



        

        public double getNearbyLocations(String SearchLat,String SearchLng,String UserLat,String UserLng)
        {
            double lat1 = Convert.ToDouble(SearchLat);

            double lat2 = Convert.ToDouble(UserLat);

            double lng1 = Convert.ToDouble(SearchLng);

            double lng2 = Convert.ToDouble(UserLng);

            double earthRedius = 6371 * 1000;

            double f1 = toRadians(lat1);

            double f2 = toRadians(lat2);

            double delf3 = toRadians(lat2 - lat1);

            double delf4 = toRadians(lng2 - lng1);

            double a = Math.Sin(delf3 / 2) * Math.Sin(delf3 / 2) + Math.Cos(f1) * Math.Cos(f2) * Math.Sin(delf4 / 2) * Math.Sin(delf4 / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = earthRedius * c;

            return d / 1000;
            //System.out.println(d / 1000 + "km");

        }
        private static double toRadians(double angdeg)
        {
            return angdeg / 180.0 * Math.PI;
        }
        
    }
}