using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;

namespace TE_System.AppContext
{
    public class CrmOAuth : ContextHelpers
    {
       public string EntityName { get { return "systemuser"; } }
       private string FieldToSort = "fullname"; 
        public string[] TargetFields
        {
            get
            {
                return new string[]
                {
                   "systemuserid",
                   "fullname"
                };
            }
        }
        public IEnumerable<OCrmUser> FetchAll()
        {
            List<OCrmUser> users = new List<OCrmUser>();
            EntityCollection collection = FetchEntity(EntityName, TargetFields, FieldToSort);

            foreach (Entity item in collection.Entities)
            {
                OCrmUser practice = new OCrmUser();
                practice.ID = (Guid)item["systemuserid"];
                practice.Fullname = item["fullname"].ToString();
                users.Add(practice);
            }

            return users;
        }    
    }
    public class OCrmUser
    {
        public Guid ID { get; set; }
        public string Fullname { get; set; }
    }
}
