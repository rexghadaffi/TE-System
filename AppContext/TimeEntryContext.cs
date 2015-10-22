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
using Utilities;
using System.Web.Mvc;

namespace TE_System.AppContext
{
    public class TimeEntryContext : ContextHelpers
    {
        private SystemUserContext dbUser = new SystemUserContext();
        private AnnotationContext dbAnnotation = new AnnotationContext();
        public string EntityName { get { return "gsc_timeentry"; } }
        public List<string> TargetFields
        {
            get
            {
                return new List<string>
                {
                   "gsc_userid",
                   "gsc_timeentryid",
                   "gsc_billability",
                   "gsc_onsite",
                   "gsc_monday",
                   "gsc_tuesday",
                   "gsc_wednesday",
                   "gsc_thursday",
                   "gsc_friday",
                   "gsc_saturday",
                   "gsc_sunday",
                   "gsc_activitydescription",
                   "gsc_totalentryhours",
                   "gsc_isvalidinfo",
                   "gsc_isvalidbillability"
                };
            }
        }

        public void AddNewRecord(string userID, int weekNumber, TimeEntry newEntry)
        {
            Connect();

            Entity TEDetail = new Entity("gsc_timeentry");

            //Get Entity of Staff
            Entity staff = GetStaff(service, userID);

            TEDetail.Attributes["gsc_userid"] = new EntityReference("systemuser", new Guid(userID));
            TEDetail.Attributes["gsc_projectsoid"] = new EntityReference("salesorder", new Guid(newEntry.ProjectID));
            TEDetail.Attributes["gsc_activitytypeid"] = new EntityReference("gsc_activity", new Guid(newEntry.ActivityTypeID));
            TEDetail.Attributes["gsc_stafftypeid"] = new EntityReference("gsc_stafftype", new Guid(newEntry.StaffTypeID));
            TEDetail.Attributes["gsc_practiceid"] = new EntityReference("gsc_practice", new Guid(newEntry.PracticeID));
            TEDetail.Attributes["gsc_week"] = weekNumber;
            TEDetail.Attributes["gsc_businessunitid"] = staff.Attributes["businessunitid"];
            TEDetail.Attributes["gsc_billability"] = newEntry.Billability;
            TEDetail.Attributes["gsc_onsite"] = newEntry.OnSite;
            TEDetail.Attributes["gsc_activitydescription"] = newEntry.Description;
            TEDetail.Attributes["gsc_monday"] = newEntry.MondayHours;
            TEDetail.Attributes["gsc_tuesday"] = newEntry.TuesdayHours;
            TEDetail.Attributes["gsc_wednesday"] = newEntry.WednesdayHours;
            TEDetail.Attributes["gsc_thursday"] = newEntry.ThursdayHours;
            TEDetail.Attributes["gsc_friday"] = newEntry.FridayHours;
            TEDetail.Attributes["gsc_saturday"] = newEntry.SaturdayHours;
            TEDetail.Attributes["gsc_sunday"] = newEntry.SundayHours;
            TEDetail.Attributes["gsc_totalentryhours"] = TotalWeekHours(newEntry);

            service.Create(TEDetail);            
        }
        public TimeEntry ExtractFormCollection(FormCollection formData)
        {
            TimeEntry newEntry = new TimeEntry();
            newEntry.ProjectID = formData["Entry.ProjectID"];
            newEntry.ActivityTypeID = formData["Entry.ActivityTypeID"];
            newEntry.StaffTypeID = formData["Entry.StaffTypeID"];
            newEntry.PracticeID = formData["Entry.PracticeID"];
            newEntry.Billability = formData["Entry.Billability"].CbxToBoolean();
            newEntry.OnSite = formData["Entry.OnSite"].CbxToBoolean();
            newEntry.Description = formData["Entry.Description"];
            // ------------
            newEntry.MondayHours = formData["Entry.MondayHours"].ToDecimal();
            newEntry.TuesdayHours = formData["Entry.TuesdayHours"].ToDecimal();
            newEntry.WednesdayHours = formData["Entry.WednesdayHours"].ToDecimal();
            newEntry.ThursdayHours = formData["Entry.ThursdayHours"].ToDecimal();
            newEntry.FridayHours = formData["Entry.FridayHours"].ToDecimal();
            newEntry.SaturdayHours = formData["Entry.SaturdayHours"].ToDecimal();
            newEntry.SundayHours = formData["Entry.SundayHours"].ToDecimal();
            newEntry.TotalWeekHours = TotalWeekHours(newEntry);
            return newEntry;
        }
        public void FlagInfo(Guid entryID)
        {
            Connect();
            Entity TEDetail = getTimeEntryDetailEntityId(service, entryID);
            TEDetail.Attributes["gsc_isvalidinfo"] = false;
            TEDetail.Attributes["gsc_isentryvalid"] = false;
            service.Update(TEDetail);
        }
        public void FlagBillability(Guid entryID)
        {
            Connect();
            Entity TEDetail = getTimeEntryDetailEntityId(service, entryID);
            TEDetail.Attributes["gsc_isvalidbillability"] = false;
            TEDetail.Attributes["gsc_isentryvalid"] = false;
            service.Update(TEDetail);
        }
        // Dim Entry as Valid
        public void EntryValid(Guid entryID)
        {
            Connect();
            Entity TEDetail = getTimeEntryDetailEntityId(service, entryID);
            TEDetail.Attributes["gsc_isvalidinfo"] = true;
            TEDetail.Attributes["gsc_isvalidbillability"] = true;
            TEDetail.Attributes["gsc_isentryvalid"] = true;
            service.Update(TEDetail);
        }
        public void Update(TimeEntry entry, string detailID, bool isTimekeeper = false)
        {
            Connect();

            Entity TEDetail = getTimeEntryDetailEntityId(service, new Guid(detailID));
            TEDetail.Attributes["gsc_projectsoid"] = new EntityReference("salesorder", new Guid(entry.ProjectID));
            TEDetail.Attributes["gsc_activitytypeid"] = new EntityReference("gsc_activity", new Guid(entry.ActivityTypeID));
            TEDetail.Attributes["gsc_stafftypeid"] = new EntityReference("gsc_stafftype", new Guid(entry.StaffTypeID));
            TEDetail.Attributes["gsc_practiceid"] = new EntityReference("gsc_practice", new Guid(entry.PracticeID));
            TEDetail.Attributes["gsc_onsite"] = entry.OnSite;
            TEDetail.Attributes["gsc_activitydescription"] = entry.Description;
            TEDetail.Attributes["gsc_monday"] = entry.MondayHours.ToDecimal();
            TEDetail.Attributes["gsc_tuesday"] = entry.TuesdayHours.ToDecimal();
            TEDetail.Attributes["gsc_wednesday"] = entry.WednesdayHours.ToDecimal();
            TEDetail.Attributes["gsc_thursday"] = entry.ThursdayHours.ToDecimal();
            TEDetail.Attributes["gsc_friday"] = entry.FridayHours.ToDecimal();
            TEDetail.Attributes["gsc_saturday"] = entry.SaturdayHours.ToDecimal();
            TEDetail.Attributes["gsc_sunday"] = entry.SundayHours.ToDecimal();
            TEDetail.Attributes["gsc_totalentryhours"] = TotalWeekHours(entry).ToDecimal();

            TEDetail.Attributes["gsc_isvalidinfo"] = true;

            if (isTimekeeper == true)
            {
                TEDetail.Attributes["gsc_isvalidbillability"] = true;
                TEDetail.Attributes["gsc_billability"] = entry.Billability;
            }

            service.Update(TEDetail);
        }
        private decimal? TotalWeekHours(TimeEntry entry)
        {
            return entry.MondayHours +
             entry.TuesdayHours +
             entry.WednesdayHours +
             entry.ThursdayHours +
             entry.FridayHours +
             entry.SaturdayHours + entry.SundayHours;
        }
        private Entity GetStaff(IOrganizationService service, string userID)
        {
            return service.Retrieve("systemuser",
                                    Guid.Parse(userID),
                                    new ColumnSet("businessunitid"));
        }
        private Entity getTimeEntryEntityId(IOrganizationService service, Guid timeEntryGuid)
        {
            return service.Retrieve("gsc_timeentry",
                                    timeEntryGuid,
                                    new ColumnSet("gsc_timeentryid"));
        }
        private Entity getTimeEntryDetailEntityId(IOrganizationService service, Guid timeEntryDetailGuid)
        {
            return service.Retrieve("gsc_timeentry",
                                    timeEntryDetailGuid,
                                    new ColumnSet("gsc_timeentryid"));
        }
        public IEnumerable<TimeEntry> FetchAll
        {
            get
            {
                Connect();
                QueryExpression qe = LinkEntities();

                EntityCollection ec = service.RetrieveMultiple(qe);

                List<TimeEntry> entries = new List<TimeEntry>();

                foreach (Entity rd in ec.Entities)
                {
                    TimeEntry entry = new TimeEntry();
                    entries.Add(FetchSetter(entry, rd));
                }

                return entries;
            }
        }
        public IEnumerable<TimeEntry> FetchForUser(Guid userID, int weekNumber)
        {
            // - Add Condition
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("gsc_userid", userID);
            conditions.Add("gsc_week", weekNumber);

            Connect();
            QueryExpression qe = LinkEntities();
            qe.Criteria = ConditionHelper(LogicalOperator.And, conditions);

            EntityCollection ec = service.RetrieveMultiple(qe);

            List<TimeEntry> entries = new List<TimeEntry>();

            foreach (Entity rd in ec.Entities)
            {
                entries.Add(FetchSetter(new TimeEntry(), rd));
            }
            return entries;
        }
        public TimeEntry FetchRecordDetails(Guid recordID)
        {
            Connect();
            QueryExpression qe = LinkEntities();

            qe.Criteria = new FilterExpression();
            qe.Criteria.AddCondition("gsc_timeentryid", ConditionOperator.Equal, recordID);


            EntityCollection ec = service.RetrieveMultiple(qe);

            TimeEntry entry = new TimeEntry();

            foreach (Entity rd in ec.Entities)
            {
                entry.ID = rd["gsc_timeentryid"].ToString();
                entry.ProjectID = ((AliasedValue)rd["Project.salesorderid"]).Value.ToString();
                entry.ActivityTypeID = ((AliasedValue)rd["ActivityType.gsc_activityid"]).Value.ToString();
                entry.StaffTypeID = ((AliasedValue)rd["StaffType.gsc_stafftypeid"]).Value.ToString();
                entry.PracticeID = ((AliasedValue)rd["Practice.gsc_practiceid"]).Value.ToString();
                entry.Description = rd["gsc_activitydescription"].ToString();
                entry.Billability = Convert.ToBoolean(rd["gsc_billability"]);
                entry.OnSite = Convert.ToBoolean(rd["gsc_onsite"]);
                entry.MondayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_monday"]));
                entry.TuesdayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_tuesday"]));
                entry.WednesdayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_wednesday"]));
                entry.ThursdayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_thursday"]));
                entry.FridayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_friday"]));
                entry.SaturdayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_saturday"]));
                entry.SundayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_sunday"]));
                entry.TotalWeekHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_totalentryhours"]));
                entry.EntryRemarks = this.dbAnnotation.GetNotesForEntry(entry.ID);
                entry.isValidBillability = Convert.ToBoolean(rd["gsc_isvalidbillability"]);
                entry.isValidInfo = Convert.ToBoolean(rd["gsc_isvalidinfo"]);
                //-- Get User Reference
                Guid userID = ((EntityReference)rd["gsc_userid"]).Id;
                entry.StaffFullname = this.dbUser.GetFullNameForUser(userID).Fullname;
                return entry;
            }

            return entry;
        }
        public IQueryable<TimeEntry> FetchPartialDetails(List<string> projectIDs, int? weekNumber, string searchText = null)
        {
            Connect();
            QueryExpression qe = LinkEntities();
            qe = ConditionHelper(qe, "gsc_projectsoid", projectIDs);


            if (weekNumber != null)
            {
                qe.Criteria = new FilterExpression();
                qe.Criteria.AddCondition("gsc_week", ConditionOperator.Equal, weekNumber);
            }

            EntityCollection ec = service.RetrieveMultiple(qe);

            List<TimeEntry> entries = new List<TimeEntry>();

            foreach (Entity rd in ec.Entities)
            {
                TimeEntry entry = new TimeEntry();
                entries.Add(FetchSetter(entry, rd));
            }
            var Iqueryable = entries.AsQueryable();
            return Iqueryable;
        }

        private TimeEntry FetchSetter(TimeEntry entry, Entity rd)
        {
            entry.ID = rd["gsc_timeentryid"].ToString();
            entry.ProjectID = ((AliasedValue)rd["Project.name"]).Value.ToString();
            entry.ActivityTypeID = ((AliasedValue)rd["ActivityType.gsc_name"]).Value.ToString();
            entry.StaffTypeID = ((AliasedValue)rd["StaffType.gsc_name"]).Value.ToString();
            entry.PracticeID = ((AliasedValue)rd["Practice.gsc_name"]).Value.ToString();
            entry.Description = rd["gsc_activitydescription"].ToString();
            entry.Billability = Convert.ToBoolean(rd["gsc_billability"]);
            entry.OnSite = Convert.ToBoolean(rd["gsc_onsite"]);
            entry.MondayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_monday"]));
            entry.TuesdayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_tuesday"]));
            entry.WednesdayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_wednesday"]));
            entry.ThursdayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_thursday"]));
            entry.FridayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_friday"]));
            entry.SaturdayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_saturday"]));
            entry.SundayHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_sunday"]));
            entry.TotalWeekHours = Convert.ToDecimal(string.Format("{0:0.00}", rd["gsc_totalentryhours"]));
            entry.isValidBillability = Convert.ToBoolean(rd["gsc_isvalidbillability"]);
            entry.isValidInfo = Convert.ToBoolean(rd["gsc_isvalidinfo"]);
            //entry.EntryRemarks = GetNotesForEntry(entry.ID);
            //-- Get User Reference
            Guid userID = ((EntityReference)rd["gsc_userid"]).Id;
            entry.StaffFullname = this.dbUser.GetFullNameForUser(userID).Fullname;

            entry.EntryRemarks = this.dbAnnotation.GetNotesForEntry(entry.ID);

            return entry;
        }       

        private QueryExpression LinkEntities()
        {
            QueryExpression qe = new QueryExpression(EntityName);
            qe.ColumnSet.AddColumns(TargetFields.ToArray());


            // Link gsc_timeentrydetail to salesorder
            qe.LinkEntities.Add(new LinkEntity("gsc_timeentry", "salesorder", "gsc_projectsoid", "salesorderid", JoinOperator.Inner));
            qe.LinkEntities[0].Columns.AddColumns("name", "salesorderid");
            qe.LinkEntities[0].EntityAlias = "Project";

            // Link gsc_timeentrydetail to Activity Type
            qe.LinkEntities.Add(new LinkEntity("gsc_timeentry", "gsc_activity", "gsc_activitytypeid", "gsc_activityid", JoinOperator.Inner));
            qe.LinkEntities[1].Columns.AddColumns("gsc_name", "gsc_activityid");
            qe.LinkEntities[1].EntityAlias = "ActivityType";

            // Link gsc_timeentrydetail to StaffType Type
            qe.LinkEntities.Add(new LinkEntity("gsc_timeentry", "gsc_stafftype", "gsc_stafftypeid", "gsc_stafftypeid", JoinOperator.Inner));
            qe.LinkEntities[2].Columns.AddColumns("gsc_name", "gsc_stafftypeid");
            qe.LinkEntities[2].EntityAlias = "StaffType";

            // Link gsc_timeentrydetail to Practice
            qe.LinkEntities.Add(new LinkEntity("gsc_timeentry", "gsc_practice", "gsc_practiceid", "gsc_practiceid", JoinOperator.Inner));
            qe.LinkEntities[3].Columns.AddColumns("gsc_name", "gsc_practiceid");
            qe.LinkEntities[3].EntityAlias = "Practice";

            return qe;
        }
    }
}