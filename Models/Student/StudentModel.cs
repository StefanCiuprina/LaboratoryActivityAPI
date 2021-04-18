using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LaboratoryActivityAPI.Models.Group;
using LaboratoryActivityAPI.Models.Attendance;
using LaboratoryActivityAPI.Models.Submission;

namespace LaboratoryActivityAPI.Models.Student
{
    public class StudentModel
    {
        [Key]
        public string StudentId { get; set; }

        [Column()]
        public int GroupId { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Hobby { get; set; }

        [Column(TypeName = "nvarchar(128)")]
        public string Token { get; set; }

        [Column()]
        public bool Registered { get; set; }

        [ForeignKey("StudentId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupModel Group { get; set; }

        public virtual ICollection<AttendanceModel> Attendances { get; set; }
        public virtual ICollection<SubmissionModel> Submissions { get; set; }
    }
}
