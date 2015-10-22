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
    public class StaffTypeContext : ContextHelpers
    {
        public string EntityName { get { return "gsc_stafftype"; } }

        public string FieldToSort = "gsc_name"; 

        public string[] TargetFields
        {
            get
            {
                return new string[]
                {
                   "gsc_stafftypeid",
                   "gsc_name"
                };
            }
        }

        public IEnumerable<StaffType> FetchAll()
        {
            List<StaffType> staffs = new List<StaffType>();
            EntityCollection collection = FetchEntity(EntityName, TargetFields, FieldToSort);

            foreach (Entity item in collection.Entities)
            {
                StaffType staff = new StaffType();
                staff.StaffTypeID = item["gsc_stafftypeid"].ToString();
                staff.StaffTypeName = item["gsc_name"].ToString();
                staffs.Add(staff);
            }
            return staffs;
        }    
    
        
    }
}