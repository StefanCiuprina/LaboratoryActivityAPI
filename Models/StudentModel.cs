using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models
{
    public class StudentModel
    {
        [Key]
        public int StudentId { get; set; }

        [Column()]
        public int GroupId { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Hobby { get; set; }

        [Column(TypeName = "nvarchar(128)")]
        public string Token { get; set; }

        public string? Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser User { get; set; }
    }
}
