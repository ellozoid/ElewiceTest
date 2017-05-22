using DBModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBModel.Models;
using DBModel.Helpers;
using NHibernate.Linq;
using NHibernate;

namespace DBModel.Managers
{
    public class DocumentManager : IDocumentRepository
    {
        private ISession session;
        private IEnumerable<Document> document;

        public DocumentManager()
        {
            session = NHibernateHelper.MakeSession();
            document = session.Query<Document>().ToList();
        }
        public IEnumerable<Document> GetAll()
        {
            //document = session.Query<Document>().ToList();
            return document;
        }

        public IEnumerable<Document> GetAllByCriteria(string searchQuery, string searchCriteria = "Name")
        {
            List<Document> documents = new List<Document>();
            if(searchCriteria == "Name")
                documents = document.Where<Document>(x => x.Name.Contains(searchQuery)).ToList();
            else if(searchCriteria == "Author")
                documents = document.Where<Document>(x => x.Author.Contains(searchQuery)).ToList();
            else if(searchCriteria == "Date")
                documents = document.Where<Document>(x => x.Date.ToString().Contains(searchQuery)).ToList();
            document = documents;
            return documents;
        }

        public IEnumerable<Document> GetAllSortBy(IEnumerable<Document> model, string sortCriteria = "Name")
        {
            List<Document> documents = new List<Document>();
            if (sortCriteria == "Name")
                documents = model.OrderBy(x => x.Name).ToList();
            else if (sortCriteria == "Author")
                documents = model.OrderBy(x => x.Author).ToList();
            else if (sortCriteria == "Date")
                documents = model.OrderBy(x => x.Date).ToList();
            return documents;
        }

        public void Save(Document model)
        {
            IQuery query = session.CreateSQLQuery("exec NewDocument @Name=:name, @Author=:author, @Date=:date");
            query.SetString("name", model.Name);
            query.SetString("author", model.Author);
            query.SetDateTime("date", DateTime.Now);
            query.ExecuteUpdate();
        }
    }
}

