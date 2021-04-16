using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Models.Lab
{
    public class LabInputModel
    {
        public int LabId { get; set; }

        public int GroupId { get; set; }

        public string Name { get; set; }

        public DateTime DateTime { get; set; }

        public string Title { get; set; }

        public string Curricula { get; set; }

        public string Description { get; set; }
    }
}
