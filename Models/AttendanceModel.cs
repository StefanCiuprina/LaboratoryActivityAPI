using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models
{
    public class AttendanceModel
    {
        [Key]
        public int AttendanceId { get; set; }

        [Column()]
        public int LabId { get; set; }

        [Column()]
        public string StudentId { get; set; }

        [Column()]
        public int StateId { get; set; }

        [ForeignKey("LabId")]
        public virtual LabModel Lab { get; set; }

        [ForeignKey("StudentId")]
        public virtual StudentModel Student { get; set; }

        [ForeignKey("StateId")]
        public virtual StateModel State { get; set; }
    }
}
