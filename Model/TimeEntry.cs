using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace TE_System.Models
{
    public class TimeEntry
    {
        public string ID { get; set; }
        public string StaffFullname { get; set; }
        public string HeaderID { get; set; }
        [Display(Name = "Project")]
        [Required(ErrorMessage = "Please select a project.")]
        public string ProjectID { get; set; }
        [Display(Name = "Activity")]
        [Required(ErrorMessage = "Please select an activity.")]
        public string ActivityTypeID { get; set; }
        [Required(ErrorMessage = "Please select a staff type.")]
        [Display(Name = "Role")]
        public string StaffTypeID { get; set; }
        [Required(ErrorMessage = "Please select a practice.")]
        [Display(Name = "Practice")]
        public string PracticeID { get; set; }
        [Display(Name = "Billable")]
        public bool Billability { get; set; }
        [Display(Name = "On Site")]
        public bool OnSite { get; set; }
        [Required(ErrorMessage = "Description field is required.")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "Description has a minimum length of 10 and maximum length of 250")]
        public string Description { get; set; }
        [Display(Name = "Mon")]
        public decimal? MondayHours { get; set; }
        [Display(Name = "Tue")]
        public decimal? TuesdayHours { get; set; }
        [Display(Name = "Wed")]
        public decimal? WednesdayHours { get; set; }
        [Display(Name = "Thu")]
        public decimal? ThursdayHours { get; set; }
        [Display(Name = "Fri")]
        public decimal? FridayHours { get; set; }
        [Display(Name = "Sat")]
        public decimal? SaturdayHours { get; set; }
        [Display(Name = "Sun")]
        public decimal? SundayHours { get; set; }
        [Display(Name = "Total")]
        public decimal? TotalWeekHours { get; set; }
        public bool isValidInfo { get; set; }
        public bool isValidBillability { get; set; }
        public IEnumerable<Annotation> EntryRemarks { get; set; }
    }
    public class TimeEntryViewModel {
        public List<TimeEntry> Entries { get; set; }
        public TimeEntry Entry { get; set; }
    }   
}