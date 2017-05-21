using DBModel.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace UserFiles
{
    public class UserFile
    {
        private const string FDir_AppData = "~/App_Data/Documents/";
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }

        public void Save(Document model)
        {
            if (!Directory.Exists($"{FDir_AppData}{model.Author}"))
            {
                Directory.CreateDirectory($"{FDir_AppData}{model.Author}");
                model.uploadedFile.SaveAs(HttpContext.Current.Server.MapPath($"{FDir_AppData}{model.Author}/{model.Name}"));
            }
            else
            {
                model.uploadedFile.SaveAs(HttpContext.Current.Server.MapPath($"{FDir_AppData}{model.Author}/{model.Name}"));
            }
        }
        //public static FilePathResult Download(string fileForDownload)
        //{
        //    var documentPath = HttpContext.Current.Server.MapPath(FDir_AppData + fileForDownload);
        //    return Controller.File(documentPath, "application/unknown", fileForDownload);
        //}
    }
}
