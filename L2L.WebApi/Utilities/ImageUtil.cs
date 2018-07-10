using L2L.Entities;
using L2L.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Utilities
{
    public class ImageUtil
    {
        public static bool SaveProfilePix(User user, string fileName, string dataUrl, out string imageUri)
        {

            imageUri = "";
            try
            {
                var savedImageUri = "Content/Images/" + user.LocalAuthUserId + "/";
                var imageLocationFolder = "/" + savedImageUri;
                CreateFolder(HttpContext.Current.Server.MapPath(imageLocationFolder));

                var idx = dataUrl.IndexOf(",");
                var data = dataUrl.Substring(idx + 1);
                byte[] bytes = Convert.FromBase64String(data);
                Image image;

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);

                    var saveLocation = Path.Combine(HttpContext.Current.Server.MapPath(imageLocationFolder), fileName);
                    imageUri = Path.Combine(savedImageUri, fileName);

                    image.Save(saveLocation, System.Drawing.Imaging.ImageFormat.Jpeg);

                    DeleteProfilePixFile(user, imageLocationFolder);

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void DeleteProfilePixFile(User user, string folderLocation)
        {
            var lastIdx = user.Profile.ProfileImageUrl.LastIndexOf("/");
            var fileName = user.Profile.ProfileImageUrl.Substring(lastIdx + 1);
            var fullPath = Path.Combine(HttpContext.Current.Server.MapPath(folderLocation), fileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public static void CreateDefaultProfilePix(string localAuthId)
        {
            try
            {
                var savedImageUri = "Content/Images/" + localAuthId + "/";
                var imageLocationFolder = "/" + savedImageUri;
                CreateFolder(HttpContext.Current.Server.MapPath(imageLocationFolder));
                var destinationProfilePix = HttpContext.Current.Server.MapPath(imageLocationFolder) + "default-profile-1.0.jpg";

                var defaultProfilePix = HttpContext.Current.Server.MapPath("/Content/Images/Icons/default-profile-1.0.jpg");
                File.Copy(defaultProfilePix, destinationProfilePix);
            }
            catch (Exception)
            {
            }
        }

        private static void CreateFolder(string folderName)
        {
            if (Directory.Exists(folderName))
                return;

            Directory.CreateDirectory(folderName);
        }

        public static bool SaveImage(string folder, string fileName, string oldFile, string dataUrl, out string imageUri, bool dontErasePrev = false)
        {

            imageUri = "";
            try
            {
                var savedImageUri = "Content/Images/" + folder + "/";
                var imageLocationFolder = "/" + savedImageUri;
                CreateFolder(HttpContext.Current.Server.MapPath(imageLocationFolder));

                var idx = dataUrl.IndexOf(",");
                var data = dataUrl.Substring(idx + 1);
                byte[] bytes = Convert.FromBase64String(data);
                Image image;

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);

                    var saveLocation = Path.Combine(HttpContext.Current.Server.MapPath(imageLocationFolder), fileName);
                    imageUri = Path.Combine(savedImageUri, fileName);

                    image.Save(saveLocation, System.Drawing.Imaging.ImageFormat.Jpeg);

                    if (dontErasePrev)
                        DeleteFile("/" + oldFile);

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void DeleteFile(string serverPath)
        {
            var localPath = HttpContext.Current.Server.MapPath(serverPath);
            if (File.Exists(localPath))
                File.Delete(localPath);
        }
    }
}