using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using TE_System.Models;
using System.Threading.Tasks;

namespace TE_System.AppContext
{
    public class SystemUserContext : ContextHelpers
    {
        public string Domain
        {
            get
            {
                return @"PH-WWOS\";
            }
        }
        public string EntityName { get { return "systemuser"; } }
        public string[] TargetFields
        {
            get
            {
                return new string[]
                {
                   "systemuserid",
                   "fullname",
                   "domainname",
                   "gsc_stafftypeid"
                };
            }
        }
        public async Task<SystemUser> GetUserDetails(string username)
        {           
            Connect();
            SystemUser userdetail = new SystemUser();
            EntityCollection collection = Find(EntityName,
                                               TargetFields,
                                               "domainname",
                                               username);
            foreach (Entity user in collection.Entities)
            {
                userdetail.ID = user["systemuserid"].ToString();
                userdetail.Fullname = user["fullname"].ToString();
            }
            return userdetail;
        }

        public SystemUser GetFullNameForUser(Guid userid)
        {
            

            SystemUser userdetail = new SystemUser();
            EntityCollection collection = FindUsingGuid(EntityName,
                                               TargetFields,
                                               "systemuserid",
                                               userid);
            foreach (Entity user in collection.Entities)
            {               
                userdetail.Fullname = user["fullname"].ToString();
            }
            return userdetail;
        }
        

        public string GetUserRole(string username)
        {
            string staffTypeID = "";

            Connect();
            QueryExpression qe = new QueryExpression();
            qe.EntityName = "systemuser";
            qe.LinkEntities.Add(new LinkEntity("systemuser", "gsc_stafftype", "gsc_stafftypeid", "gsc_stafftypeid", JoinOperator.Inner));
            qe.LinkEntities[0].Columns.AddColumns("gsc_name", "gsc_stafftypeid");
            qe.LinkEntities[0].EntityAlias = "SystemUserRole";
            qe.Criteria = new FilterExpression();
            qe.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, username);
            EntityCollection ec = service.RetrieveMultiple(qe);

            foreach (Entity user in ec.Entities)
            {
                staffTypeID = ((AliasedValue)user["SystemUserRole.gsc_name"]).Value.ToString();
            }

            return staffTypeID;
        }
       
    }
}