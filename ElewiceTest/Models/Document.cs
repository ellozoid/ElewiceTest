using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElewiceTest.Models
{
    public class Document
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Author { get; set; }
        public virtual string FileLink { get; set; }
    }
    public class DocumentMap : ClassMap<Document>
    {
        public DocumentMap()
        {
            Id(x => x.ID);
            Map(x => x.Name);
            Map(x => x.Date);
            Map(x => x.Author);
            Map(x => x.FileLink);
        }
    }
}