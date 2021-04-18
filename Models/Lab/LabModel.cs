using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LaboratoryActivityAPI.Models.Group;
using LaboratoryActivityAPI.Models.Attendance;
using LaboratoryActivityAPI.Models.Assignment;

namespace LaboratoryActivityAPI.Models.Lab
{
    public class LabModel
    {
        [Key]
        public int LabId { get; set; }

        [Column()]
        public int GroupId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column()]
        public DateTime DateTime { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Curricula { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupModel Group { get; set; }

        public virtual ICollection<AttendanceModel> Attendances { get; set; }

        public virtual AssignmentModel Assignment { get; set; }
    }
}
