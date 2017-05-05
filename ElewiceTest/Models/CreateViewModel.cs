using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ElewiceTest.Models
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }
         
        [DataType(DataType.Upload)]
        public HttpPostedFileBase uploadedFile { get; set; }
    }
}