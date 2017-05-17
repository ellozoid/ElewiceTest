using ElewiceTest.Models;
using ElewiceTest.Models.NHibernate;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElewiceTest.Models.Identity;

namespace ElewiceTest.Controllers
{
    public class HomeController : Controller
    {

        private User currentUser = null;
        public User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    string userName = User.Identity.Name;
                    if (userName != null)
                    {
                        currentUser = new NHibernateHelper().Users.FindByNameAsync(userName).Result;
                    }
                }
                return currentUser;
            }
        }
        //GET: Home
        [Authorize]
        public ActionResult Index()
        {
            if(CurrentUser != null)
            {
                ViewBag.Username = CurrentUser.UserName;
            }
            var session = NHibernateHelper.MakeSession();
            var documents = session.Query<Document>().ToList();

            return View(documents);
        }
        public ActionResult Create()
        {
            ViewBag.Username = CurrentUser.UserName;
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateViewModel model)
        {
            var session = NHibernateHelper.MakeSession();
            ViewBag.Username = CurrentUser.UserName;
            string createRequest = Request.Params["createBtn"];
            string fileAuthorRequest = CurrentUser.UserName;
            string uploadedFileName;
            string[] nameSeparate = new string[100];
            if (model.uploadedFile != null && model.Name != string.Empty)
            {
                uploadedFileName = System.IO.Path.GetFileName(model.uploadedFile.FileName);
                nameSeparate = uploadedFileName.Split('.');
                model.uploadedFile.SaveAs(Server.MapPath("~/App_Data/Documents/" + model.Name + '.' + nameSeparate.Last()));
                IQuery query = session.CreateSQLQuery("exec NewDocument @Name=:name, @Author=:author, @Date=:date");
                query.SetString("name", model.Name + '.' + nameSeparate.Last());
                query.SetString("author", fileAuthorRequest);
                query.SetDateTime("date", DateTime.Now);
                query.ExecuteUpdate();
                return RedirectToAction("Index");
            }
            else
            {
                if (model.uploadedFile == null && model.Name == string.Empty)
                    ModelState.AddModelError("", "Enter the name of the document and select the file to upload.");
                else if (model.Name == string.Empty)
                    ModelState.AddModelError("", "Enter the name of the document");
                else if (model.uploadedFile == null)
                    ModelState.AddModelError("", "Select the file to upload");
            }
            return View(model);
        }
        public FilePathResult DownloadFile(string fileForDownload)
        {
            string FDir_AppData = "~/App_Data/Documents/";
            var documentPath = Server.MapPath(FDir_AppData + fileForDownload);
            return File(documentPath, "application/unknown", fileForDownload);
        }

        [HttpPost]
        public ActionResult Index(string str)
        {
            ViewBag.Username = CurrentUser.UserName;
            var session = NHibernateHelper.MakeSession();
            string findQuery = Request.Params["findQuery"];
            var documents = session.Query<Document>().Where<Document>(x => x.Name.Contains(findQuery)).ToList(); //.Query<Document>().Where(x => x.Name.Like(findQuery)).ToList();
            if (findQuery != string.Empty)
                ViewBag.SearchResult = string.Format("Results for : <b>{0}</b>", findQuery);
            else
                ViewBag.SearchResult = string.Empty;
            return View("Index", documents); //("Index", documents);
        }
    }
}