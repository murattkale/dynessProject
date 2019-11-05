using Core;
using Core.Entities.Dto;
using Core.General;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace WebUI.Helpers
{
    public class FileHelper
    {
        private static readonly string[] Extensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

        private static bool ImageFileUploadCheckExtension(HttpPostedFileBase httpPostedFile)
        {
            var extension = Path.GetExtension(httpPostedFile.FileName);
            extension = extension.ToLower();
            return Extensions.Any(extension.Contains);
        }

        public static MessageInfo FileSave(HttpPostedFileBase postedFile, string directory, string filePath, string field)
        {
            var messageInfo = new MessageInfo
            {
                Field = field
            };

            filePath = HttpContext.Current.Server.MapPath(filePath);

            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));

                postedFile.SaveAs(filePath);

                messageInfo.MessageInfoType = MessageInfoType.Success;
                messageInfo.Message = "Başarıyla kaydedildi.";

                return messageInfo;
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                messageInfo.MessageInfoType = MessageInfoType.Error;
                messageInfo.Message = ex.Message;

                return messageInfo;
            }
        }

        public static MessageInfo ImageSave(HttpPostedFileBase postedFile, string directory, string filePath, string field, Size? size)
        {
            var messageInfo = new MessageInfo
            {
                Field = field
            };

            var tempPathImageName = string.Empty;

            try
            {
                if (!ImageFileUploadCheckExtension(postedFile))
                {
                    messageInfo.MessageInfoType = MessageInfoType.Error;
                    messageInfo.Message = "Eklemek istediğiniz görselin, türü uygun değil.";

                    return messageInfo;
                }

                filePath = HttpContext.Current.Server.MapPath(filePath);

                if (!Directory.Exists(HttpContext.Current.Server.MapPath(directory)))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directory));

                if (size == null)
                {
                    postedFile.SaveAs(filePath);
                }
                else
                {
                    var img = Image.FromStream(postedFile.InputStream, true, true);

                    if (size.Value.Height > img.Height || size.Value.Width > img.Width)
                    {
                        messageInfo.MessageInfoType = MessageInfoType.Error;
                        messageInfo.Message = $"Eklemek istediğiniz görselin, boyutları uygun değil. Minimum {size.Value.Height} yükseklik, {size.Value.Width} genişlik ölçülerinde görsel ekleyebilirsiniz.";

                        return messageInfo;
                    }
                    else if (size.Value.Height == img.Height && size.Value.Width == img.Width)
                    {
                        postedFile.SaveAs(filePath);

                        messageInfo.MessageInfoType = MessageInfoType.Success;
                        messageInfo.Message = "Başarıyla kaydedildi.";

                        return messageInfo;
                    }

                    var date = DateTime.Now.ToShortDateString().Split('.');
                    var time = DateTime.Now.ToString("HH:mm:ss:fff").Split(':');

                    var tempPath = $"{AyarlarService.Get().GeciciYol}";

                    var tempImageName = $"{date[0]}{date[1]}{date[2]}{time[0]}{time[1]}{time[2]}{time[3]}";

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(tempPath)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(tempPath));
                    }

                    tempPathImageName = HttpContext.Current.Server.MapPath(tempPath + tempImageName + Path.GetExtension(postedFile.FileName));
                    postedFile.SaveAs(tempPathImageName);

                    using (var imageForResize = new Bitmap(tempPathImageName))
                    {
                        var castedSize = (Size)size;
                        var newSize = new Size();

                        var rate = (double)imageForResize.Height / castedSize.Height;

                        var sizeCheckWidth = new Size(Convert.ToInt32(imageForResize.Width / rate), castedSize.Height);

                        rate = (double)imageForResize.Width / castedSize.Width;

                        var sizeCheckHeight = new Size(castedSize.Width, Convert.ToInt32(imageForResize.Height / rate));

                        if (sizeCheckWidth.Width <= castedSize.Width &&
                            sizeCheckWidth.Height <= castedSize.Height)
                        {
                            newSize = sizeCheckWidth;
                        }
                        else if (sizeCheckHeight.Width <= castedSize.Width &&
                                 sizeCheckHeight.Height <= castedSize.Height)
                        {
                            newSize = sizeCheckHeight;
                        }

                        using (var b = new Bitmap(newSize.Width, newSize.Height))
                        {
                            using (var g = Graphics.FromImage(b))
                            {
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                                g.DrawImage(imageForResize, 0, 0, newSize.Width, newSize.Height);

                                b.Save(filePath);
                            }
                        }
                    }

                    if (File.Exists(tempPathImageName))
                        File.Delete(tempPathImageName);
                }

                messageInfo.MessageInfoType = MessageInfoType.Success;
                messageInfo.Message = "Başarıyla kaydedildi.";

                return messageInfo;
            }
            catch (Exception ex)
            {
                if (File.Exists(tempPathImageName))
                    File.Delete(tempPathImageName);

                messageInfo.MessageInfoType = MessageInfoType.Error;
                messageInfo.Message = ex.Message;

                return messageInfo;
            }
        }

        public static void IfFileExistsDeleteFile(string filePath)
        {
            if (File.Exists(HttpContext.Current.Server.MapPath(filePath)))
                File.Delete(HttpContext.Current.Server.MapPath(filePath));
        }

        public static string KurumLogoImageName(HttpPostedFileBase postedFile, string kurumAd)
        {
            return $"logo-{kurumAd.ClearChars()}{Path.GetExtension(postedFile.FileName)} ";
        }

        public static string KurumArkaPlanImageName(HttpPostedFileBase postedFile, string kurumAd)
        {
            return $"arka-plan-{kurumAd.ClearChars()}{Path.GetExtension(postedFile.FileName)} ";
        }

        public static string PersonelGorselImageName(HttpPostedFileBase postedFile, string personelAdSoyad)
        {
            return $"personel-{personelAdSoyad.ClearChars()}{Path.GetExtension(postedFile.FileName)} ";
        }

        public static string OgrenciGorselImageName(HttpPostedFileBase postedFile, string ogrenciAdSoyad)
        {
            return $"ogrenci-{ogrenciAdSoyad.ClearChars()}{DateTime.Now.Second}{DateTime.Now.Millisecond}{Path.GetExtension(postedFile.FileName)} ";
        }
    }
}