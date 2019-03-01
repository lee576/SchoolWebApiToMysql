using System;
using System.Security.Cryptography;
using System.Text;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// SHA256加密算法
    /// </summary>
    public class Sha256Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HMACSHA256(string srcString, string key = "abc123")
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(secrectKey))
            {
                hmac.Initialize();
                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);
                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");
                return str_hamc_out;
            }
        }
    }
}
