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
    public class ProjectContext : ContextHelpers
    {
        public string EntityName { get { return "salesorder"; } }

        public string PkField { get { return "salesorderid"; } }

        public string FieldToSort = "name"; 

        public string[] TargetFields
        {
            get
            {
                return new string[]
                {
                   "salesorderid",
                   "name",
                   "gsc_projectmanagerid"
                };
            }
        }
        public IEnumerable<Project> FetchAll()
        {
            List<Project> projects = new List<Project>();
            EntityCollection collection = FetchEntity(EntityName, TargetFields, FieldToSort);

            foreach (Entity item in collection.Entities)
            {
                Project project = new Project();
                project.ProjectID = item["salesorderid"].ToString();
                project.ProjectName = item["name"].ToString();              
                projects.Add(project);
            }
            return projects;
        } 
   
        public IEnumerable<Project> FindProjectForUser(string userID)
        {
            List<Project> projects = new List<Project>();
            EntityCollection collection = Find(EntityName,
                                                TargetFields,
                                                "gsc_projectmanagerid",
                                                userID);
            foreach (Entity item in collection.Entities)
            {
                Project project = new Project();
                project.ProjectID = item["salesorderid"].ToString();
                project.ProjectName = item["name"].ToString();
                projects.Add(project);
            }
            return projects;
        }
    }
}