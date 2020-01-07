using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ProjectNotunThikana.Models
{
    public class LatLng
    {

        public String Name { set; get; }
        public String Email { set; get; }
        public String Address { set; get; }
        public String city { set; get; }
        public String Details { set; get; }
        public String Operation { set; get; }

        public String getImg { set; get; }

        public String getCover { set; get; }

        public ArrayList PostsofFlat = new ArrayList();

        public ArrayList FlatName = new ArrayList();

        public ArrayList getDetailsList = new ArrayList();


        public String getGeoCode(String Address)
        {
            String Data = "";
            string urlAddress = "https://api.opencagedata.com/geocode/v1/json?q=" + Address + "&key=f2f924210645411d8260e62d3dbd5537";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                Data = readStream.ReadToEnd();



                response.Close();
                readStream.Close();


            }
            return Data;
        }


        public  String[] GetFirstLastName(String address)
        {

            String x = getGeoCode(address);
            char[] a = x.ToArray();
            String res = ""; bool svr = false;
            for (int i = 0; i < a.Length; i++)
            {

                if (a[i] == '#' && a[i + 1] == 'm' && a[i + 2] == 'a' && a[i + 3] == 'p' && a[i + 4] == '=')
                {
                    i++;
                    i++;
                    i++;
                    i++;
                    i++;
                    for (int j = i; j < a.Length; j++)
                    {
                        if (a[j] == '/' || a[j] == '.')
                        {
                            res += a[j];
                        }
                        else
                        {
                            try
                            {
                                res += Convert.ToInt32(a[j].ToString());
                            }
                            catch (Exception e)
                            {
                                svr = true;
                                break;
                            }
                        }
                    }
                    if (svr == true)
                    {
                        break;
                    }
                }
            }

            String[] LatLng = res.Split('/');
            String[] LATLNG = new String[2];
            LATLNG[0] = LatLng[1];
            LATLNG[1] = LatLng[2];
            // Console.WriteLine("Lat: " + LatLng[1] + " Lng: " + LatLng[2]);
            return LATLNG;

        }

        public  String StrCheck(String Value)
        {

            String x = Regex.Replace(Value, @"\s+", "");

            return x;

        }

        public String UploadEx(String Value)
        {
            String x = Regex.Replace(Value, @"\s+", "");

            String refactor = "";

            char[] re = x.ToCharArray();

            for (int i = 0; i < re.Length; i++)
            {
                
                if (re[i] == '/' || re[i] == '|' || re[i] == ':' || re[i] == '*' || re[i] == '?' || re[i] == '<' || re[i] == '>')
                {
                    re[i] = ' ';
                    refactor += re[i];
                }
                else
                refactor += re[i];
            }
            return refactor;
        }
        //static void Main(string[] args)
        //{

        //    GetFirstLastName("Anam Medical College,Savar,Dhaka,Bangladesh");
        //    Console.WriteLine(x[0] + " " + x[1]);
        //    Console.ReadKey();
        //}
    }
}