using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;

namespace VSW.Lib.Global
{
    public static class Image
    {
        public static void SaveFromUrl(string url, string target)
        {
            try
            {
                var client = new WebClient();
                client.DownloadFile(new Uri(url), target);
            }
            catch (Exception e)
            {
                Error.Write(e.Message);
            }
        }

        public static void ResizeImageFile(int type, string file, string target, string ext, int width, int height, params object[] parameter)
        {
            var oldBmp = new Bitmap(file);

            var oldWidth = oldBmp.Width;
            var oldHeight = oldBmp.Height;

            int newWidth = 0, newHeight = 0, left = 0, top = 0;
            decimal ratio;

            switch (type)
            {
                case 1:
                    newWidth = width == 0 ? oldWidth : width;
                    newHeight = height == 0 ? oldHeight : height;
                    break;

                case 2:
                    if (width == 0 && height == 0)
                    {
                        newWidth = oldWidth;
                        newHeight = oldHeight;
                    }
                    else
                    {
                        newWidth = width == 0 ? (int)(oldWidth * (height / (double)oldHeight)) : width;
                        newHeight = height == 0 ? (int)(oldHeight * (width / (double)oldWidth)) : height;
                    }
                    break;

                case 3:
                    if (width == 0 && height == 0)
                    {
                        newWidth = oldWidth;
                        newHeight = oldHeight;
                    }
                    else
                    {
                        newWidth = width == 0 ? oldWidth : (int)(oldWidth * ((double)width / 100));
                        newHeight = height == 0 ? oldHeight : (int)(oldHeight * ((double)height / 100));
                    }
                    break;

                case 4:
                    if (oldWidth > oldHeight)
                    {
                        newWidth = width == 0 ? oldHeight : width;
                        ratio = (decimal)newWidth / oldWidth;
                        newHeight = (int)(oldHeight * ratio);
                    }
                    else
                    {
                        newHeight = height == 0 ? oldWidth : height;
                        ratio = (decimal)height / oldHeight;
                        newWidth = (int)(oldWidth * ratio);
                    }
                    break;

                case 5:
                    if (oldWidth > oldHeight)
                    {
                        newWidth = width == 0 ? oldHeight : width;
                        ratio = (decimal)newWidth / height;
                        newWidth = (int)(Math.Round(oldHeight * ratio));
                        if (newWidth < oldWidth)
                        {
                            newHeight = oldHeight;
                            left = (oldWidth - newWidth) / 2;
                        }
                        else
                        {
                            newHeight = (int)Math.Round(oldHeight * ((double)oldWidth / newWidth));
                            newWidth = oldWidth;
                            top = (oldHeight - newHeight) / 2;
                        }
                    }
                    else
                    {
                        newHeight = height == 0 ? oldWidth : height;
                        ratio = (decimal)newHeight / width;
                        newHeight = (int)(Math.Round(oldWidth * ratio));
                        if (newHeight < oldHeight)
                        {
                            newWidth = oldWidth;
                            top = (oldHeight - newHeight) / 2;
                        }
                        else
                        {
                            newWidth = (int)Math.Round(oldWidth * ((double)oldHeight / newHeight));
                            newHeight = oldHeight;
                            left = (oldHeight - newWidth) / 2;
                        }
                    }
                    break;
            }

            string codec = ext == ".png" ? "image/png" : (ext == ".gif" ? "image/gif" : "image/jpeg");
            var format = ext == ".png" ? MagickFormat.Png : (ext == ".gif" ? MagickFormat.Gif : MagickFormat.Pjpeg);
            var brushes = ext == ".png" ? Brushes.Transparent : Brushes.White;

            var newBmp = new Bitmap(newWidth, newHeight);
            var graphic = Graphics.FromImage(newBmp);
            graphic.SmoothingMode = SmoothingMode.AntiAlias;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

            graphic.FillRectangle(brushes, 0, 0, newWidth, newHeight);
            graphic.DrawImage(oldBmp, left, top, newWidth, newHeight);

            //watermark
            AddWaterMark(graphic, newWidth, newHeight, parameter);

            //quality image
            newBmp.Save(target, GetEncoder(codec), new EncoderParameters(1) { Param = { [0] = new EncoderParameter(Encoder.Quality, 100L) } });

            if (ext == ".webp")
            {
                WebP.Save(newBmp, target, 80);
            }
            else
            {
                ProgessiveSave(newBmp, target, format, 80);
            }

            graphic.Dispose();
            oldBmp.Dispose();
            newBmp.Dispose();
        }

        public static void CroppedImageFile(string file, string target, string extension, int width, int height, params object[] parameter)
        {
            ResizeImageFile(5, file, target, extension, width, height, parameter);
        }

        #region private

        private static void AddWaterMark(Graphics graphic, int newWidth, int newHeight, params object[] parameter)
        {
            //watermark
            if (parameter == null) return;

            var watermark = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(HttpUtility.UrlDecode(parameter[0].ToString())));
            {
                var wtmWidth = newWidth - watermark.Width - parameter.Length > 1 ? Core.Global.Convert.ToInt(parameter[1], 0) : 0;
                var wtmHeight = newHeight - watermark.Height - parameter.Length > 2 ? Core.Global.Convert.ToInt(parameter[2], 0) : 0;

                graphic.DrawImage(watermark, wtmWidth, wtmHeight, watermark.Width, watermark.Height);
            }
        }

        private static void ProgessiveSave(Bitmap bmp, string path, MagickFormat format, int quality)
        {
            using (var image = new MagickImage(bmp))
            {
                //new ImageOptimizer().LosslessCompress(new FileInfo(image.FileName));

                image.Format = format;
                image.Quality = quality;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.Write(stream);
                }
            }
        }

        private static ImageCodecInfo GetEncoder(string mime)
        {
            return Encoders[mime.ToLower()];
        }

        private static Dictionary<string, ImageCodecInfo> _oEncoders;
        private static Dictionary<string, ImageCodecInfo> Encoders
        {
            get
            {
                if (_oEncoders == null)
                    _oEncoders = new Dictionary<string, ImageCodecInfo>();

                if (_oEncoders.Count != 0) return _oEncoders;

                foreach (var encoder in ImageCodecInfo.GetImageEncoders())
                    _oEncoders.Add(encoder.MimeType.ToLower(), encoder);

                return _oEncoders;
            }
        }

        #endregion private
    }
}