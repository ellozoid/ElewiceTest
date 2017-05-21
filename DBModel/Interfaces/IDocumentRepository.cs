using DBModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.Interfaces
{
    interface IDocumentRepository : IBaseRepository<Document>
    {
        IEnumerable<Document> GetAllByCriteria(string searchQuery, string searchCriteria = "Name");
        void Save(Document doc);
    }
}
