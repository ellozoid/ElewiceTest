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
            string fileNameRequest = Request.Params["Name"];
            string fileAuthorRequest = CurrentUser.UserName;
            string uploadedFileName;
            string[] nameSeparate = new string[100];
            if (model.uploadedFile != null && fileNameRequest != string.Empty)
            {
                uploadedFileName = System.IO.Path.GetFileName(model.uploadedFile.FileName);
                nameSeparate = uploadedFileName.Split('.');
                model.uploadedFile.SaveAs(Server.MapPath("~/App_Data/Documents/" + fileNameRequest + '.' + nameSeparate.Last()));
                IQuery query = session.CreateSQLQuery("exec NewDocument @Name=:name, @Author=:author, @Date=:date");
                query.SetString("name", fileNameRequest + '.' + nameSeparate.Last());
                query.SetString("author", fileAuthorRequest);
                query.SetDateTime("date", DateTime.Now);
                query.ExecuteUpdate();
                return RedirectToAction("Index");
            }
            else
            {
                if (model.uploadedFile == null && fileNameRequest == string.Empty)
                    ModelState.AddModelError("", "Введите имя документа и выберете файл для загрузки");
                else if (fileNameRequest == string.Empty)
                    ModelState.AddModelError("", "Введите имя документа");
                else if (model.uploadedFile == null)
                    ModelState.AddModelError("", "Выберете файл для загрузки");
            }
            return View(model);
        }
        public FilePathResult DownloadFile(string fileForDownload)
        {
            string FDir_AppData = "~/App_Data/Documents/";
            var documentPath = Server.MapPath(FDir_AppData + fileForDownload);
            return File(documentPath, "application/unknown", fileForDownload);
        }
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
        [HttpPost]
        public ActionResult Find()
        {
            ViewBag.Username = CurrentUser.UserName;
            var session = NHibernateHelper.MakeSession();
            string findQuery = Request.Params["findQuery"];
            var documents = session.Query<Document>().Where<Document>(x => x.Name.Contains(findQuery)).ToList(); //.Query<Document>().Where(x => x.Name.Like(findQuery)).ToList();
            ViewBag.SearchResult = string.Format("Results for : <b>{0}</b>", findQuery);
            return View("Index", documents); //("Index", documents);
        }
    }
}