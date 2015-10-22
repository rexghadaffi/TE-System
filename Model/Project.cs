using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_System.Models
{
    public class Project
    {
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }

        public Guid ProjectManagerID { get; set; }
    }
}