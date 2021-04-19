using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models.Submission
{
    public class SubmissionOutputModel
    {
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public string StudentId { get; set; }

        public string StudentName { get; set; }
        public string Link { get; set; }
        public string Comment { get; set; }
        public int Grade { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
