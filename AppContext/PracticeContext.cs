using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using TE_System.Models;
using Microsoft.Xrm.Sdk.Query;

namespace TE_System.AppContext
{
    public class PracticeContext : ContextHelpers
    {
        public string EntityName { get { return "gsc_practice"; } }
        private string FieldToSort = "gsc_name"; 
        public string[] TargetFields
        {
            get
            {
                return new string[]
                {
                   "gsc_practiceid",
                   "gsc_name"
                };
            }
        }
        public IEnumerable<Practice> FetchAll()
        {
            List<Practice> practices = new List<Practice>();
            EntityCollection collection = FetchEntity(EntityName, TargetFields, FieldToSort);

            foreach (Entity item in collection.Entities)
            {
                Practice practice = new Practice();
                practice.PracticeID = item["gsc_practiceid"].ToString();
                practice.PracticeName = item["gsc_name"].ToString();
                practices.Add(practice);
            }

            return practices;
        }    
    }
}