using System.ComponentModel.DataAnnotations.Schema;

namespace NIF_Builder.Models
{
    public class ProjectEquipment
    {

        public int ProjectEquipmentID { get; set; }

        [ForeignKey("Project")]
        public int ProjectID { get; set; }

        [ForeignKey("Equipment")]
        public int EquipmentID { get; set; }

        public virtual Project Project { get; set; } = default!;
        public virtual Equipment Equipment { get; set; } = default!;
    }
}
