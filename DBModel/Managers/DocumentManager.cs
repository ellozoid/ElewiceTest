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

        public DocumentManager()
        {
            session = NHibernateHelper.MakeSession();
        }
        public IEnumerable<Document> GetAll()
        {
            return session.Query<Document>().ToList();
        }

        public IEnumerable<Document> GetAllByCriteria(string searchQuery, string searchCriteria = "Name")
        {
            var documents = session.Query<Document>().Where<Document>(x => x.Name.Contains(searchQuery)).ToList();
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

