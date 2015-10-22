using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TE_System.Models
{
    public class ServiceOrder
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Client { get; set; }
    }
}