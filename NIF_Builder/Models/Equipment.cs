using System.ComponentModel.DataAnnotations;

namespace NIF_Builder.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }

        [Required, StringLength(100), Display(Name = "Equipment Name")]
        public string EquipmentName { get; set; } = default!;
        public virtual ICollection<ProjectEquipment> ProjectEquipments { get; set; } = new List<ProjectEquipment>();
    }
}
