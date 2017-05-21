using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DBModel.Models
{
    public class Document
    {
        public virtual int ID { get; protected set; }
        public virtual string Name { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Author { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public virtual HttpPostedFileBase uploadedFile { get; set; }
    }
    public class DocumentMap : ClassMap<Document>
    {
        public DocumentMap()
        {
            Id(x => x.ID);
            Map(x => x.Name);
            Map(x => x.Date);
            Map(x => x.Author);
        }
    }
}