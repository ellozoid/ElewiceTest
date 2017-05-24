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
        private NHibernateHelper helper;

        public DocumentManager()
        {
            helper = new NHibernateHelper();
            session = helper.MakeSession();
            document = session.Query<Document>().ToList();
        }
        public IEnumerable<Document> GetAll()
        {
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

        public void Save(Document model)
        {
            IQuery query = session.CreateSQLQuery("exec NewDocument @Name=:name, @Author=:author, @Date=:date");
            query.SetString("name", model.Name);
            query.SetString("author", model.Author);
            query.SetDateTime("date", model.Date);
            query.ExecuteUpdate();
        }
    }
}

