using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Models;
using Service;
using DbModel;
using IService;
using Models.ViewModels;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class CashierConfig
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static Aop.Api.IAopClient LoginDAL(ZhifubaoLogin login)
        {
            Aop.Api.IAopClient client = new Aop.Api.DefaultAopClient(login.serverURL, login.appId, login.privateKeyPem, login.format, login.version, login.signType, login.alipayPulicKey, login.charset, login.keyFromFile);
            //AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
            return client;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public static Aop.Api.IAopClient getDefaultAopClient(string schoolcode)
        {
            ZhifubaoLogin zhifubao = dataZFBlogin(schoolcode);
            Aop.Api.IAopClient dac = LoginDAL(zhifubao);
           
            return dac;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schoolcode"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static Aop.Api.IAopClient getDefaultAopClient(string schoolcode, string pid)
        {
            ZhifubaoLogin zhifubao = dataZFBlogin(schoolcode, pid);
            Aop.Api.IAopClient dac = LoginDAL(zhifubao);
           
            return dac;
        }
       /// <summary>
       /// 获取授权令牌
       /// </summary>
       /// <param name="auth_code"></param>
       /// <param name="schoolcode"></param>
       /// <returns></returns>
        public static Aop.Api.Response.AlipaySystemOauthTokenResponse getUserToken(string auth_code, string schoolcode)
        {
            Aop.Api.IAopClient client = getDefaultAopClient(schoolcode);

            Aop.Api.Request.AlipaySystemOauthTokenRequest request = new Aop.Api.Request.AlipaySystemOauthTokenRequest();
            request.Code = auth_code;
            request.GrantType = "authorization_code";

            Aop.Api.Response.AlipaySystemOauthTokenResponse response = client.Execute(request);

            return response;
        }

        /// <summary>
        /// 获取用户授权信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="schoolcode"></param>
        /// <returns></returns>
        public static Aop.Api.Response.AlipayUserInfoShareResponse getUserInfo(string accessToken, string schoolcode)
        {
            Aop.Api.IAopClient client = getDefaultAopClient(schoolcode);

            Aop.Api.Request.AlipayUserInfoShareRequest request = new Aop.Api.Request.AlipayUserInfoShareRequest();

            Aop.Api.Response.AlipayUserInfoShareResponse response = client.Execute(request, accessToken);

            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <returns></returns>
        public static ZhifubaoLogin dataZFBlogin(string schoolCode)
        {    
            Itb_school_InfoService shool = new tb_school_InfoService();       
            tb_school_info schoolmodel=shool.FindByClause(p => p.School_Code == schoolCode);
            ZhifubaoLogin datalogin = new ZhifubaoLogin();
            if (schoolmodel!=null)
            {
                datalogin.serverURL = "https://openapi.alipay.com/gateway.do";
                datalogin.format = "json";
                datalogin.version = "1.0";
                datalogin.signType = "RSA2";
                datalogin.charset = "utf-8";
            

                datalogin.appId = schoolmodel.app_id;
                datalogin.publickey = schoolmodel.publicKey;
                datalogin.privateKeyPem = schoolmodel.private_key;
                datalogin.alipayPulicKey = schoolmodel.alipay_public_key;
            }
            return datalogin;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schoolCode"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static ZhifubaoLogin dataZFBlogin(string schoolCode, string pid)
        {
          
            Itb_payment_accountsService payment = new tb_payment_accountsService();
            tb_payment_accounts paymentmodel = payment.FindByClause(p => p.schoolcode == schoolCode&&p.pid==pid);
            

          
            ZhifubaoLogin datalogin = new ZhifubaoLogin();
            if (paymentmodel!= null)
            {
                datalogin.serverURL = "https://openapi.alipay.com/gateway.do";
                datalogin.format = "json";
                datalogin.version = "1.0";
                datalogin.signType = "RSA2";
                datalogin.charset = "utf-8";
                datalogin.keyFromFile = false;

                datalogin.appId = paymentmodel.appid;
                datalogin.publickey = paymentmodel.publickey;
                datalogin.privateKeyPem =paymentmodel.private_key;
                datalogin.alipayPulicKey = paymentmodel.alipay_public_key;
            }
            return datalogin;
        }
        /// <summary>
        /// 
        /// </summary>
        public static void SetCode()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();
            SchoolCard = config.GetSection("SchoolCard");
            PID = SchoolCard.GetSection("pid").Value;
            devicepid= SchoolCard.GetSection("devicepid").Value;
            alipaystoreid= SchoolCard.GetSection("alipaystoreid").Value;
            ZFDTURL = SchoolCard.GetSection("ZFDTURL").Value;
            XYKURL = SchoolCard.GetSection("XYKURL").Value;
            XYZY= SchoolCard.GetSection("XYZY").Value;
        }
        /// <summary>
        /// 
        /// </summary>
        public static  IConfiguration SchoolCard = null;
        /// <summary>支付宝PID</summary>               
        public static string PID;//"2088811668583035";
        /// <summary>
        /// 
        /// </summary>
        public static string devicepid;//"2088811668583035";
        /// <summary>
        /// 
        /// </summary>
        public static string alipaystoreid ;//"2088811668583035";
        //// <summary>支付宝APPID</summary>
        //public static readonly string APPID = ConfigurationManager.AppSettings["appId"];//"2016112103077572";
        ///// <summary>支付宝私钥</summary>
        //public static readonly string PRIVATE_KEY = ConfigurationManager.AppSettings["privateKey"];//"MIIEowIBAAKCAQEA07xvGSBjKa7afJFIozOVGP05Q2eg/U1sQ4dPxI44qHoKcb1UJlhyLR5x51nZry5SbQDcdLGCQIxkJOO03/rtGSVIkMk5rWk7nF33UaUIlpM7Tmst6lhD3iu7vesaOheWrQcVCwP97uq6IJGSilvsZdIroBNVy+yXnYk2nHCJAmiMvNmz2/1VqLLHTpB/NsEK8JgHADGWVz8Pj+Z0mN48oQeWHEy/rq81nb61O5lPBiarpKA4iPHvnuHOLzuwOUDRzQlzDqkkIR7/yWRbsg/VAhMnKslC1w+xSGruVPDHaf6oUhgJHRAtk0NOF7VLrhqg9pZsoMuHCQ+SI3CLMQJspQIDAQABAoIBAEFaM+BJgtHmykEQhp742NgxsG8BmUsVjoj5RsGgNBSsNnUMvuBnBDaw4u9JaEsNvRsv3NuXvmA19OL99WH8jRnpzBAES+0eQevthNyKV7BCGVgG9NEUFT6JuRTRb/LmPPeC7BfOT55ijLhothZGcRysVhK+ZQjgKZvSMizu/xriogGTx7IZLqvH9FbyQWEiOku6VNosQelidy+jSQ/J1TYTALMMVyxEKICYNSfxKHd13Jb/urK23C0mDFZglOqbfeBx9IaxKN4SW7V1AMHKnaShqmzpnEGXcoeeixF5S10M8NxRv1vn1tLBgxvFsaCflvURS2fLldzHF2kJXda4MQECgYEA/AbTfoLys1o7aZZHLvU53v4cQg1tgCZwhwRJuqzHlHTALS0f37JxZs6FVXqJHtQz/WLKTQsoA71142aSOBw14ETlfHBTbyYPcH2IRwXWsmrkXjR2pmLTlDCs44yvUUeqr6Nje/AShM7UkNx5LZNDA+q03HLzmSjyE0zlaEPqPdECgYEA1xL+9AU/4Sr/PIZwy5TBzNpHHTpvFvrWhkqUxQU2Ibg/oSZ+MqUTx/dCAiaIaZf5abPvg3si+ysmpWzSBu5QCaEMkDM++5wAMs+FVfDgMqXFWXU73d5yBobYlcriLbazKz0gByfwT5ZpgF3M1KwdU5WSe/d8vIN2luHfif5H0pUCgYEAyYy/+I3KgGpp1yAKX2BX3qCDgsNwTarwFNn2CKcCmRPhWH+c3O28yQXiFaEAJbp4tWwa8xA3+P28WJZ/2wWchHU0vZaq8tmSQVjy8jGWKGtZpIj6Vkf0gq+GpBevYSYaN4pIFibA+Jrb3dmjwzHgxzIdX6tCarsXFR5K3F5r5vECgYA9YnWUDh+CU7RPgQuWf1mk7zPW2sO0KlmqMIUvimI904mNpB/msojnzOFxHbBXewG7spiMzUtZpqI8GsgDJGeBqA6e5ZF3XLoNxn8G3V4P9pJSCwzQMVoYFMqiTiqp43hVwfdvM236OTLZaWw50vn3zjvl1+gpIdhqDgOwLfwv1QKBgD2tm6bCL/kcnfEr42GagVqKuRXiC3TquI4nu9U7zeAzeWc8s+KXg+c7vK7j/GfP4EQ845BB+B9EDJqiWjwjTOcIRMTCja6JVCa4lSarW8dahHOmwntxnfHY4eAjCVciXgiFW0FB0A7ugXW32CrTVJnRQej/SfDrKRJ9gFn9xXz+";
        ///// <summary>支付宝公钥</summary>
        //public static readonly string ALIPAY_PUBLIC_KEY = ConfigurationManager.AppSettings["alipayPublicKey"];//"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0EBjMem5D6HqEKDS45gurdkBe1Qd0Z/SprAnHBY2oAtu8QeEeQDGgOqqJjEexDaFgD97Dtlu6uRUAWT3EC059kjAZJ6ETw1vwHJntkmIdLH7CcUX/RRaHrF3IvlL0APNofyFP8rw7ynN95uqNGN9uXW+wR1KP9ezs7bujQQb59TBuky6lrm3ayeG4Mvl81BsQG0OhHAybcoRErioOBCdBEV7kaeF2mzXK76154D/bwqQEPlyHbdT/omQexikfCzXp78hh5n2ACCYaRC0ICYGQbWriDUOyMoJPltPF9bb7VWIWBxxKrNvTzAFKMjcC51WKingIIBMZTEKYkeKzHyMFQIDAQAB";
        ////2088811668583035	2015122201022484	MIIEpAIBAAKCAQEAyN6XKC9ytVdxj8s9Gw1vB1lYzp5g5g81fKTflKKLih0Z4hKQekqF970kThoVJspA/QGM2kPZRpyOL75M6BbK5c8QxgbbhOxB6SZ33vCSEO9hBLMEgeLL2CV030xzWwHFh4DnMppiexTZIMC1q1drcedCEvCP4QA71vJss39fWG0air8fMb8MU2XP9htzWx2cz8LGYHY0shKdwUNxsDyxbfFRPeHMephPpDCRN4lvry/6YOs9feQMTVx6SKjC5lPV5QDvL0MbYKQ6ebHwy3Ub7/ie5WNvDwVNDxMoUvhWBsmteoLG0PWzS93vOy3yJBIb4A/zEnJEouuDVmRWJXg2WwIDAQABAoIBAQCZZ72jOBFvwvd8rOfe+DR6NVcofTZdnPHpXnVOK5FMCouQ50Kl0rJbkHzglPTgagiV8RAkRTrzvW6tsbVEbtvIBIq34dbWviRcLj/P6IR1IIxErX9cvtuVGI4YV2el8kVsBhsLv3JEs5hbdjGISLxLAiWpF0WavbX1o7E2qKkletoPBTRWI6F88pOBJ+z4G6LxW6uz4TIqFcsr6EBf8c6oy9fyvwijXkdpk5BUwyZDsVXk0UB1b1Pt1XYBQc4ciE7VEWGQLQRxonrmDZmiXgC3iYvCa5JbBt4ibgqQTEiOYHJtXI2Zxk5zl0aNRD2mpa+1OD3OfZyrPT1KEf9r8tnRAoGBAPXsPWmPS5NPs008fTCHbVkXYNcGHQHqHbat1rqUiWYvYHV1qIK6pMvk0lrprb/DhPvDhV/TMPViwIv0yPOt8L5WVrIBBFXkzroUrDZIO5jVOvPm6lWIQUsxfUpaTh2+I71W3EtAm/VLe2A6UrUXFOHym0vI4gcthYJ05l/lt6SzAoGBANEZvF0wICehUwkp1EhhotFIOPMtDBz9nGZ2CRfbwnDZrebNLMu6z7EbLbGawvZSPenqHnEPpBIWVqg8cHVwUfiR7AmVwVVUG5y87sZnlhLsnUuC7cts/boOv3CoDoWA3rAEJgJ+yr8+syV6PIMIIi3D3M4/ftLLsL+IZBv514u5AoGAS3UiXtpuGRRScveFfjd/sN+AglnI2saIOX5brcJX6nfNBB2HCB3W5Q0gEm8zNez7R/j1WrLFifW0GP2SD1smzHBXh6TSPLzJRcWEFd/SEZIT1bTb4ES/rB/STtcosu2dr8IQNDLt57UydRNQP2qGqNG1HurTl9o65g2ShohI8gsCgYBXOU8T6GFhZrBGoEZHM0NB3ciz19S3uyskqpQ8eZVwkb0zC99l1LWSgW3cKlytd70P+HNeYlHkoaDgaOXYd1QaRnSZwvh06bLi/QT1inxVxJIQz7r2Iq7sj/5XtiLomctKzVA1tkJI6JS+S3E2j4wCXzOabW4v7Hv8SZ7I9L1vMQKBgQDQGsQ/RnxT/3QaJIFHbR1OHJfFCvI5YXmCUYvy+oTo9CWp6BH5aAQHBUyM3Jrta1rAC7FWhAym0rlIAxITk1GwyeGX7twtuJtKabI/rwgJ01lKxfWsDDSswztLamYhq6HegkI81ojLSa1Sks9eY8BboZE/JjN0mNPIv5i/W91v0A==	MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0EBjMem5D6HqEKDS45gurdkBe1Qd0Z/SprAnHBY2oAtu8QeEeQDGgOqqJjEexDaFgD97Dtlu6uRUAWT3EC059kjAZJ6ETw1vwHJntkmIdLH7CcUX/RRaHrF3IvlL0APNofyFP8rw7ynN95uqNGN9uXW+wR1KP9ezs7bujQQb59TBuky6lrm3ayeG4Mvl81BsQG0OhHAybcoRErioOBCdBEV7kaeF2mzXK76154D/bwqQEPlyHbdT/omQexikfCzXp78hh5n2ACCYaRC0ICYGQbWriDUOyMoJPltPF9bb7VWIWBxxKrNvTzAFKMjcC51WKingIIBMZTEKYkeKzHyMFQIDAQAB

        //public static readonly string APPID = dataZFBlogin().appId;
        //public static readonly string PUBLIC_KEY = dataZFBlogin().publickey;
        //public static readonly string PRIVATE_KEY = dataZFBlogin().privateKeyPem;
        //public static readonly string ALIPAY_PUBLIC_KEY = dataZFBlogin().alipayPulicKey;
        /// <summary>
        /// 
        /// </summary>
        /// 
        //校园站点地址
        public static string XYZY;
        /// <summary>
        /// 
        /// </summary>
        public static string ZFDTURL ;
        /// <summary>
        /// 
        /// </summary>
        public static string XYKURL ;

        /// <summary>微信APPID</summary>               
        public const string WXAPPID = "wx2daafe21a93e8ef3";
        //public const string WXAPPID = "wx73604b61e7ae1d23";

        /// <summary>微信MCHID</summary>               
        public const string MCHID = "1482202032";
        //public const string MCHID = "1324234601";

        /// <summary>微信KEY</summary>               
        public const string WXKEY = "3E4CD38EEE3CA5BC1272F49838B63765";
        /// <summary>微信APPSECRET</summary>               
        public const string APPSECRET = "6d55ae9a5466870e6a0c450c16b45d8f";
        //public const string APPSECRET = "040955aa39a3bfbfa3d545c469d9f338";
        /// <summary>微信PayTemplateID</summary>               
        public const string PayTemplateID = "TwG4TDlsN9V8QZ9Cb9lHluT1DjykwB1xI6jMmOn3WrA";
        /// <summary>
        /// 
        /// </summary>
        public const string SSLCERT_PATH = "/lib/cert/apiclient_cert.p12";
        /// <summary>
        /// 
        /// </summary>
        public const string SSLCERT_PASSWORD = "1482202032";
        //public const string SSLCERT_PASSWORD = "1324234601";

        /// <summary>
        /// 
        /// </summary>
        public const string PuFaagentNo = "30000039";
        /// <summary>
        /// 
        /// </summary>
        public const string PuFaversion = "1.0";
        /// <summary>
        /// 
        /// </summary>
        public const string PuFaKEY = "wqhatzjrj36wn39rwmf9kdttz81e21ir";

        /// <summary>
        /// 通联参数
        /// APPID：00000051
        /// </summary>
        public const string TLAppid = "00010398";
        /// <summary>
        /// KEY：allinpay888
        /// </summary>
        public const string TLKey = "allinpay888";


        //SYB_CUSID = "990521082996000";
        //SYB_APPID = "00010398";
        //SYB_APPKEY = "allinpay888";
        //SYB_APIURL = "https://vsp.allinpay.com/apiweb/unitorder";//生产环境
        /// <summary>
        /// 
        /// </summary>
        public const string Two_level_domain_name = "cash";

    }
}
