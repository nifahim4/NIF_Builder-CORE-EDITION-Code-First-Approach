using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIF_Builder.Models.ViewModels
{
    public class ProjectVM
    {
        public int ProjectID { get; set; }

        [Required(ErrorMessage = "Project name is required!!")]
        [StringLength(100)]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [Display(Name = "Estimate End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EstimateEndDate { get; set; }

        [Display(Name = "Budget")]
        public int Budget { get; set; }

        [Display(Name = "Work in Progress")]
        public bool WorkInProgress { get; set; }
        [Display(Name = "Upload Document")]
        public IFormFile? ProjectDocumentFile { get; set; }
        public string ProjectDocuments { get; set; }


        public List<int> EquipmentList { get; set; } = new List<int>();
    }
}
