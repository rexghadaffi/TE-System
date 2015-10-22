using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;

namespace TE_System.AppContext
{
    public abstract class ContextHelpers
    {
        private static string Url = ConfigurationManager.AppSettings["Url"].ToString();
        private static string Username = ConfigurationManager.AppSettings["Username"].ToString();
        private static string Password = ConfigurationManager.AppSettings["Password"].ToString();
        private static string CrmConnectionString = string.Format("Url={0}; Username={1}; Password={2}",
                                                                  Url, Username, Password);
        private static CrmConnection crmConnection = null;
        protected static OrganizationService service = null;
        public virtual void Connect()
        {
            string CrmConnectionString = string.Format("Url={0}; Username={1}; Password={2};",
                                                       Url, Username, Password);
            crmConnection = CrmConnection.Parse(CrmConnectionString);
            service = new OrganizationService(crmConnection);
        }

        public EntityCollection FetchEntity(string name, string[] fields, string fieldToSort)
        {
            Connect();
            QueryExpression qe = new QueryExpression(name);
            qe.ColumnSet = new ColumnSet();
            qe.ColumnSet.AddColumns(fields);
            OrderExpression order = new OrderExpression();
            order.AttributeName = fieldToSort;
            order.OrderType = OrderType.Ascending;
            qe.Orders.Add(order);
            return service.RetrieveMultiple(qe);
        }

        public EntityCollection Find(string name, string[] fields, string key, string value)
        {
            Connect();
            QueryExpression qe = new QueryExpression(name);
            qe.ColumnSet = new ColumnSet();
            qe.ColumnSet.AddColumns(fields);
            qe.Criteria = new FilterExpression();
            qe.Criteria.AddCondition(key, ConditionOperator.Equal, value);
            return service.RetrieveMultiple(qe);
        }

        public EntityCollection FindUsingGuid(string name, string[] fields, string key, Guid value)
        {
            Connect();
            QueryExpression qe = new QueryExpression(name);
            qe.ColumnSet = new ColumnSet();
            qe.ColumnSet.AddColumns(fields);
            qe.Criteria = new FilterExpression();
            qe.Criteria.AddCondition(key, ConditionOperator.Equal, value);
            return service.RetrieveMultiple(qe);
        }

        protected EntityCollection GetAttachments(Guid entityID)
        {
            QueryExpression qe = new QueryExpression
            {
                EntityName = "annotation",
                ColumnSet = new ColumnSet("subject",
                                          "notetext",
                                          "createdby",
                                          "createdon")
            };

            qe.Criteria = new FilterExpression();
            qe.Criteria.AddCondition("objectid", ConditionOperator.Equal, new EntityReference("timeentry", entityID).Id);

            return service.RetrieveMultiple(qe);
        }

        public QueryExpression ConditionHelper(QueryExpression qe, string key, List<string> values)
        {
            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.Or;

            foreach (string item in values)
            {
                filter.Conditions.Add(new ConditionExpression(key, ConditionOperator.Equal, item));
            }

            qe.Criteria.Filters.Add(filter);
            return qe;
        }
        public FilterExpression ConditionHelper(LogicalOperator op, Dictionary<string, object> keyValues)
        {
            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = op;

            foreach (KeyValuePair<string, object> item in keyValues)
            {
                filter.Conditions.Add(new ConditionExpression(item.Key, ConditionOperator.Equal, item.Value));
            }
            return filter;
        }
    }
}