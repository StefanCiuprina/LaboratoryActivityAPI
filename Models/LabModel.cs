using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models
{
    public class LabModel
    {
        [Key]
        public int LabId { get; set; }

        [Column()]
        public int GroupId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupModel Group { get; set; }

        public virtual ICollection<AttendanceModel> Attendances { get; set; }

        public virtual AssignmentModel Assignment { get; set; }
    }
}
