using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models
{
    public class AssignmentModel
    {
        [Key]
        public int AssignmentId { get; set; }

        [Column()]
        public int LabId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column()]
        public DateTime Deadline { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; }

        [ForeignKey("LabId")]
        public virtual LabModel Lab { get; set; }

        public virtual ICollection<SubmissionModel> Submissions{ get; set; }
    }
}
