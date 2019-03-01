using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Polly;
using QRCoder;

namespace SchoolWebApi.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class RaffQRCode : IQRCode
    {
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="logoPath"></param>
        /// <param name="content"></param>
        /// <param name="pixel">像素大小</param>
        /// <returns></returns>
        public string GetQRCode(string logoPath, string content, int pixel)
        {
            using (var generator = new QRCodeGenerator())
            {
                using (var codeData = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.M, true))
                {
                    using (var qrcode = new QRCode(codeData))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            var qrImage = qrcode.GetGraphic(pixel, Color.Black, Color.White, true);
                            qrImage.Save(ms, ImageFormat.Png);
                            byte[] bytes = new byte[ms.Length];
                            ms.Position = 0;
                            ms.Read(bytes, 0, bytes.Length);
                            ms.Close();
                            Convert.ToBase64String(bytes);
                            return Convert.ToBase64String(bytes);

                            //using (MemoryStream ms1 = new MemoryStream())
                            //{
                            //    //把logo图片放到二维码图片正中心
                            //    var newImage = CombinImage(qrImage, logoPath);
                            //    newImage.Save(ms1, ImageFormat.Png);
                            //    return StreamToBytes(ms1);
                            //}
                        }
                    }
                }
            }
        }

        /// <summary>   
        /// 调用此函数后使此两种图片合并，类似相册，有个   
        /// 背景图，中间贴自己的目标图片   
        /// </summary>   
        /// <param name="imgBack">粘贴的源图片</param>   
        /// <param name="destImg">粘贴的目标图片</param>   
        public static Image CombinImage(Image imgBack, string destImg)
        {
            Image img = Image.FromFile(destImg);        //照片图片     
            if (img.Height != 65 || img.Width != 65)
            {
                img = KiResizeImage(img, 65, 65, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);    

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框   
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);   

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }

        /// <summary>   
        /// Resize图片   
        /// </summary>   
        /// <param name="bmp">原始Bitmap</param>   
        /// <param name="newW">新的宽度</param>   
        /// <param name="newH">新的高度</param>   
        /// <param name="Mode">保留着，暂时未用</param>   
        /// <returns>处理以后的图片</returns>   
        public static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                using (Graphics g = Graphics.FromImage(b))
                {
                    // 插值算法的质量   
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                }
                return b;
            }
            catch
            {
                return null;
            }
        }
    }
}
