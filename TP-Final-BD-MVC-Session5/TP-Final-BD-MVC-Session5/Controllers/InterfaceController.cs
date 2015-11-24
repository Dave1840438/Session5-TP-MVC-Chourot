using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace TP_Final_BD_MVC_Session5.Controllers
{
    public class InterfaceController : Controller
    {
        public string saveImageToServer(WebImage image)
        {
            string folderPath = Server.MapPath(@"~/Images");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            String newFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
            String fullPath = @"Images/" + newFileName;

            image.Save(@"~/" + fullPath);
            return newFileName;
        }
    }
}