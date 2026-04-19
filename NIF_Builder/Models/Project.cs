using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIF_Builder.Models
{
    public class Project
    {
        public int ProjectID { get; set; }

        [Required, StringLength(100), Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required, Column(TypeName = "date"), Display(Name = "Start Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required, Column(TypeName = "date"), Display(Name = "Estimate End Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EstimateEndDate { get; set; }

        [Display(Name = "Budget")]
        public int Budget { get; set; }

        [StringLength(255), Display(Name = "Project Document")]
        public string ProjectDocuments { get; set; }

        [Display(Name = "Work in Progress")]
        public bool WorkInProgress { get; set; }
        public virtual ICollection<ProjectEquipment> ProjectEquipments { get; set; } = new List<ProjectEquipment>();
    }
}
