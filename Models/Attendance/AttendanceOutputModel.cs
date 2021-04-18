using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models.Attendance
{
    public class AttendanceOutputModel
    {
        public int AttendanceId { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
}
