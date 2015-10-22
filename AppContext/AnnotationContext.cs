using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TE_System.Models;

namespace TE_System.AppContext
{
    public class AnnotationContext : ContextHelpers
    {
        public IEnumerable<Annotation> GetNotesForEntry(string entityID)
        {
            SystemUserContext dbUser = new SystemUserContext();
            List<Annotation> remarks = new List<Annotation>();

            foreach (Entity entity in GetAttachments(new Guid(entityID)).Entities)
            {
                Annotation EntryRemarks = new Annotation();
                EntryRemarks.NoteText = entity["notetext"].ToString();
                EntryRemarks.Subject = entity["subject"].ToString();
              
                EntryRemarks.ManagerID = ((EntityReference)entity["createdby"]).Id;
                EntryRemarks.ManagerName = dbUser.GetFullNameForUser(EntryRemarks.ManagerID).Fullname;
                          
                EntryRemarks.CreatedOn = Convert.ToDateTime(entity["createdon"]);
                remarks.Add(EntryRemarks);
            }

            return remarks;
        }
        public void AddRemarksForEntry(Guid entryID,  Annotation data, string Username, string Password)
        {
            Entity note = new Entity("annotation");

            note["objectid"] = new EntityReference("gsc_timeentry", entryID);
            note["subject"] = data.Subject;
            note["notetext"] = data.NoteText;


            string Url = ConfigurationManager.AppSettings["Url"].ToString();
            string CrmConnectionString = string.Format("Url={0}; Username={1}; Password={2}",
                                                        Url, Username, Password);
            CrmConnection crmConnection = null;
            crmConnection = CrmConnection.Parse(CrmConnectionString);
            OrganizationService service = new OrganizationService(crmConnection);

            service.Create(note);
        }
    }
}



