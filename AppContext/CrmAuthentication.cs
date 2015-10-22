using System;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System.ServiceModel.Description;
using System.Web.UI;
using System.Web.Configuration;
using TE_System.Models;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk.Query;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Discovery;
using System.Net;

namespace TE_System.AppContext
{
    public class CrmAuthentication : ContextHelpers
    {
        public async Task<bool> IsAuthenticated(PortalUser user)
        {
            string Url = ConfigurationManager.AppSettings["URL"].ToString();
            string CrmConnectionString = string.Format("Url={0}; Username={1}; Password={2}",
                                                              Url, user.Username, user.Password);
          
            ClientCredentials credential = new ClientCredentials();          
            
            credential.UserName.UserName = user.Username;
            credential.UserName.Password = user.Password;


            CrmConnection crmConnection = CrmConnection.Parse(CrmConnectionString);
            crmConnection.ClientCredentials = credential;
            OrganizationService service = new OrganizationService(crmConnection);

            QueryExpression qe = new QueryExpression("systemuser");
            qe.ColumnSet = new ColumnSet();
            qe.ColumnSet.AddColumn("systemuserid");
            qe.ColumnSet.AddColumn("fullname");
            qe.Criteria = new FilterExpression();
            qe.Criteria.AddCondition("domainname", ConditionOperator.Equal, user.Username);

            EntityCollection collection = service.RetrieveMultiple(qe);

            if (collection.Entities.Count == 0)
            {
                return false;   
            }
            return true;
        }
    }
}