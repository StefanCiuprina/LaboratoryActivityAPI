using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models.Assignment
{
    public class AssignmentInputModel
    {
        public int AssignmentId { get; set; }
        public int LabId { get; set; }

        public string Name { get; set; }

        public DateTime Deadline { get; set; }

        public string Description { get; set; }

    }
}
