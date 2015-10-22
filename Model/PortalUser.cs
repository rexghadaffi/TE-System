using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TE_System.Models
{
    public class PortalUser
    {
        public string Path
        {
            get
            {
                //return ConfigurationManager.ConnectionStrings["ADConnectionString"].ConnectionString;
                return "LDAP://ph-wwdc88/DC=PH-WWOS,DC=COM";
            }

        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}