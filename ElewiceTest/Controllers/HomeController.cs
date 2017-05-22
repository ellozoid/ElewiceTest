using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using DBModel.Models;
using DBModel.Helpers;
using DBModel.Managers;
using DBModel.Models.Identity;
using DBModel.Interfaces;
using System.IO;
using System.Collections;
using ElewiceTest.Models;

namespace ElewiceTest.Controllers
{
    public class HomeController : Controller
    {
        #region PrivateMembers
        private User currentUser = null;
        private DocumentManager DocumentManager { get; set; }
        private static IEnumerable<Document> DocumentList { get; set; }
        private UserRepositoryManager UserManager { get; set; }
        private const string FDir_AppData = "~/App_Data/Documents/";
        public List<SelectListItem> listItems;
        #endregion
        public User CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    string userName = User.Identity.Name;
                    if (userName != null)
                    {
                        currentUser = UserManager.GetCurrentUser(userName);
                    }
                }
                return currentUser;
            }
        }
        public HomeController()
        {
            DocumentManager = new DocumentManager();
            UserManager = new UserRepositoryManager();
            if(DocumentList == null)
                DocumentList = DocumentManager.GetAll();
            listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Name",
                Value = "Name",
                Selected = true
            });
            listItems.Add(new SelectListItem
            {
                Text = "Author",
                Value = "Author",
                Selected = false
            });
            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "Date",
                Selected = false
            });
            ViewData["SearchBy"] = ViewData["SortBy"] = listItems;
        }
        //GET: Home
        [Authorize]
        public ActionResult Index()
        {
            DocumentList = DocumentManager.GetAll();
            if (CurrentUser != null)
            {
                ViewBag.Username = CurrentUser.UserName;
            }
            return View(DocumentList);
        }
        public ActionResult Create()
        {
            ViewBag.Username = CurrentUser.UserName;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Document model)
        {
            ViewBag.Username = CurrentUser.UserName;
            if (model.uploadedFile != null && model.Name != string.Empty)
            {
                var uploadedFileName = Path.GetFileName(model.uploadedFile.FileName);
                model.Name = $"{model.Name}.{uploadedFileName.Split('.').Last()}";
                model.Author = CurrentUser.UserName;
                if (!Directory.Exists($"{FDir_AppData}{model.Author}"))
                {
                    var path = Server.MapPath($"{FDir_AppData}{model.Author}");
                    Directory.CreateDirectory(path);
                    model.uploadedFile.SaveAs(Server.MapPath($"{FDir_AppData}{model.Author}/{model.Name}"));
                }
                else
                {
                    model.uploadedFile.SaveAs(Server.MapPath($"{FDir_AppData}{model.Author}/{model.Name}"));
                }
                //Сохраняем здесь
                DocumentManager.Save(model);
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
        public FilePathResult DownloadFile(string fileForDownload, string author)
        {
            var documentPath = Server.MapPath($"{FDir_AppData}{author}/{fileForDownload}");
            return File(documentPath, "application/unknown", fileForDownload);
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<Document> Model)
        {
            ViewBag.Username = CurrentUser.UserName;
            IEnumerable<Document> model = new List<Document>();
            if(Request.Params["sortButton"] == "Sort by")
            {
                var criteria = Request.Params["SortBy"];
                model = DocumentManager.GetAllSortBy(DocumentList, criteria);
            }
            else
            {
                var criteria = Request.Params["SearchBy"];
                var searchQuery = Request.Params["findQuery"];
                ChangeSelect(criteria);
                if (searchQuery != string.Empty)
                    ViewBag.SearchResult = $"Results for : <b>{searchQuery}</b> (Search by {criteria})";
                else
                    ViewBag.SearchResult = string.Empty;
                model = DocumentList = DocumentManager.GetAllByCriteria(searchQuery, criteria);
            }
            return View("Index", model);
        }
        private void ChangeSelect(string item)
        {
            if (item == "Name")
                listItems[0].Selected = true;
            else if (item == "Author")
                listItems[1].Selected = true;
            else if (item == "Date")
                listItems[2].Selected = true;
        }
    }
}