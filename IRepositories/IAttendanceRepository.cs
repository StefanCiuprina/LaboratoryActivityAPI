using LaboratoryActivityAPI.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.IRepositories
{
    public interface IAttendanceRepository
    {

        Task<List<AttendanceOutputModel>> GetAllForLab(int labId);

        Task<List<AttendanceOutputModel>> GetAllForStudent(string studentId);

        Task<object> Add(AttendanceInputModel model);

        Task<object> SetState(AttendanceInputModel attendanceInputModel, int stateId);

        Task<string> DeleteAllByLab(int labId);

        Task<string> DeleteAllByStudent(string studentId);
    }
}
