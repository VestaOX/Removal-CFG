using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vesta_CFG
{
    class Magic
    {
        public void decrypt(string Bytes)
        {
            byte[] data = Convert.FromBase64String(Bytes);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Properties.Settings.Default.hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    Resulthash = UTF8Encoding.UTF8.GetString(results);
                }
            }
        }
        public string Resulthash { get; set; }
    }
}
