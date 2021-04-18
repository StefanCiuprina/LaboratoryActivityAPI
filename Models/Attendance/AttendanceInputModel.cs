using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models.Attendance
{
    public class AttendanceInputModel
    {
        public int AttendanceId { get; set; }
        public int LabId { get; set; }
        public string StudentId { get; set; }

    }
}
