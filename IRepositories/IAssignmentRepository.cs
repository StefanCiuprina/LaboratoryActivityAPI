using LaboratoryActivityAPI.Models.Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.IRepositories
{
    public interface IAssignmentRepository
    {
        Task<AssignmentModel> GetForLab(int labId);

        Task<List<AssignmentModel>> GetAllForGroup(int groupId);

        Task<object> Add(AssignmentInputModel model);

        Task<object> Update(AssignmentInputModel model);

        Task<string> Delete(int assignmentId);

        Task<string> DeleteByLab(int labId);

    }
}
