using System;
using System.Web.Mvc;
using System.Collections.Generic;
using DBModel.Models;
using DBModel.Managers;
using System.IO;

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
            ViewData["SearchBy"] = listItems;
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
                var fileNameExtension = Path.GetExtension(model.uploadedFile.FileName);
                model.Date = DateTime.Now;
                model.Name = $"{model.Name}[{model.Date.ToString().Replace(':', '_')}]{fileNameExtension}";
                model.Author = CurrentUser.UserName;
                
                model.uploadedFile.SaveAs(Server.MapPath($"{FDir_AppData}{model.Name}"));
                
                DocumentManager.Save(model);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Enter the name of the document and select the file to upload.");
            }
            return View(model);
        }
        public FilePathResult DownloadFile(string fileForDownload)
        {
            var documentPath = Server.MapPath($"{FDir_AppData}/{fileForDownload}");
            return File(documentPath, "application/unknown", fileForDownload.Split('[')[0] + Path.GetExtension(fileForDownload));
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<Document> Model)
        {
            ViewBag.Username = CurrentUser.UserName;

            var criteria = Request.Params["SearchBy"];
            var searchQuery = Request.Params["findQuery"];
            ChangeSelect(criteria);
            if (searchQuery != string.Empty)
                ViewBag.SearchResult = $"Results for : <b>{searchQuery}</b> (Search by {criteria})";
            else
                ViewBag.SearchResult = string.Empty;
            return View("Index", DocumentManager.GetAllByCriteria(searchQuery, criteria));
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