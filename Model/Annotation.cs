using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TE_System.Models
{
    public class Annotation
    {     
        public Guid ID { get; set; }
        [Display(Name = "Title")]
        [Required(ErrorMessage = "This field is required.")]
        public string Subject { get; set; }
        [Display(Name = "Remarks")]
        [Required(ErrorMessage = "This field is required.")]
        public string NoteText { get; set; }
        public Guid ManagerID { get; set; }
        public string ManagerName { get; set; }
        public DateTime CreatedOn { get; set; }

        public Guid ObjectID { get; set; }
    }
}
