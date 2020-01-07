using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ProjectNotunThikana.Models
{

   
    public class GotoUser
    {
        public String Address { set; get; }

        public String mail { set; get; }

        public String Name { set; get; }

        public String Phone { set; get; }

        public String User { set; get; }

        public String Operation { set; get; }

        public String getImg { set; get; }

        public String getCover { set; get; }


        public string Decrypt(string strData)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(strData)));
            // reference https://msdn.microsoft.com/en-us/library/system.convert.frombase64string(v=vs.110).aspx

        }


        public static byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes =
            new PasswordDeriveBytes(Global.strPermutation,
            new byte[] { Global.bytePermutation1,
                         Global.bytePermutation2,
                         Global.bytePermutation3,
                         Global.bytePermutation4
            });

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }
        // reference
        // https://msdn.microsoft.com/en-us/library/system.security.cryptography(v=vs.110).aspx
        // https://msdn.microsoft.com/en-us/library/system.security.cryptography.cryptostream%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
        // https://msdn.microsoft.com/en-us/library/system.security.cryptography.rfc2898derivebytes(v=vs.110).aspx
        // https://msdn.microsoft.com/en-us/library/system.security.cryptography.aesmanaged%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
    }




}
