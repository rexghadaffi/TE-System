using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TE_System.Models;

namespace TE_System.AppContext
{
    public class TimekeepingContext : ContextHelpers
    {
        private SystemUserContext dbUser = new SystemUserContext();
        public int WeekNumber { get; set; }
        public IEnumerable<SystemUser> FetchFlaggedUsers
        {
            get
            {
                Connect();
                QueryExpression qe = new QueryExpression("gsc_timeentry");
                qe.ColumnSet.AddColumn("gsc_userid");
                qe.Criteria = new FilterExpression();
                qe.Criteria.AddCondition("gsc_isvalidbillability", ConditionOperator.Equal, false);
  
                EntityCollection ec = service.RetrieveMultiple(qe);

                List<SystemUser> users = new List<SystemUser>();

                foreach (Entity rd in ec.Entities)
                {
                    SystemUser user = new SystemUser();
                    //-- Get User Reference                   
                    user.ID = ((EntityReference)rd["gsc_userid"]).Id.ToString();
                    user.Fullname = dbUser.GetFullNameForUser(new Guid(user.ID)).Fullname;

                    users.Add(user);
                }              
              
                return users.GroupBy(user => user.Fullname).Select(group => group.First());
            }
        }
    }
}