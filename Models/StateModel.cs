using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LaboratoryActivityAPI.Models.Attendance;

namespace LaboratoryActivityAPI.Models
{
    public class StateModel
    {
        [Key]
        public int StateId { get; set; }

        [Column(TypeName = "nvarchar(128)")]
        public string Name { get; set; }

        public virtual ICollection<AttendanceModel> Attendances { get; set; }
    }
}
