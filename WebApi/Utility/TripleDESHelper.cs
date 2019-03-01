using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SchoolWebApi.Utility
{

    /// <summary>
    /// 3DES加密算法帮助类
    /// </summary>
    public class TripleDESHelper
    {
        #region 3des加密
        /// <summary>
        /// 3des ecb模式加密
        /// </summary>
        /// <param name="aStrString">待加密的字符串</param>
        /// <param name="aStrKey">密钥</param>
        /// <param name="iv">加密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string aStrString, string aStrKey = "123456789147258369123456", CipherMode mode = CipherMode.ECB, string iv = "12345678")
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(aStrKey),
                    Mode = mode
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = Encoding.UTF8.GetBytes(iv);
                }
                var desEncrypt = des.CreateEncryptor();
                byte[] buffer;
                //安迅通智能锁 的参数带中文的用GB2312处理
                if (Regex.IsMatch(aStrString, @"[\u4e00-\u9fa5]"))
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var encoding = Encoding.GetEncoding("GB2312");
                    buffer = encoding.GetBytes(aStrString);
                }
                else
                {
                    buffer = Encoding.UTF8.GetBytes(aStrString);
                }
                return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region 3des解密

        /// <summary>
        /// des 解密
        /// </summary>
        /// <param name="aStrString">加密的字符串</param>
        /// <param name="aStrKey">密钥</param>
        /// <param name="iv">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>解密的字符串</returns>
        public static string Decrypt(string aStrString, string aStrKey = "123456789147258369123456", CipherMode mode = CipherMode.ECB, string iv = "12345678")
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(aStrKey),
                    Mode = mode,
                    Padding = PaddingMode.PKCS7
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = Encoding.UTF8.GetBytes(iv);
                }
                var desDecrypt = des.CreateDecryptor();
                var result = "";
                byte[] buffer = Convert.FromBase64String(aStrString);
                result = Encoding.UTF8.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
