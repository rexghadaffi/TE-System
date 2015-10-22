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
    public class ActivityTypeContext : ContextHelpers
    {
        public string EntityName { get { return "gsc_activity"; } }
        private string FieldToSort = "gsc_name"; 
        public string[] TargetFields
        {
            get
            {
                return new string[]
                {
                   "gsc_activityid",
                   "gsc_name"
                };
            }
        }
        public IEnumerable<ActivityType> FetchAll()
        {
            List<ActivityType> activities = new List<ActivityType>();
            EntityCollection collection = FetchEntity(EntityName, TargetFields, FieldToSort);

            foreach (Entity item in collection.Entities)
            {
                ActivityType activity = new ActivityType();            
                activity.ActivityTypeID = item["gsc_activityid"].ToString();
                activity.ActivityTypeName = item["gsc_name"].ToString();
                activities.Add(activity);
            }
            return activities;
        }
    }
}