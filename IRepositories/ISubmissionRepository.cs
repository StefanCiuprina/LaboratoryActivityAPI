using LaboratoryActivityAPI.Models.Submission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.IRepositories
{
    public interface ISubmissionRepository
    {
        Task<List<SubmissionModel>> GetAllForAssignment(int assignmentId);

        Task<List<SubmissionModel>> GetAllForStudent(string studentId);

        Task<object> Add(SubmissionInputModel model);

        Task<object> Update(SubmissionInputModel model);

        Task<object> SetGrade(SubmissionInputModel model);

        Task<string> Delete(int submissionId);

        Task<string> DeleteAllByAssignment(int assignmentId);
    }
}
