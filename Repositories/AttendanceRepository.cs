using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Models.Attendance;
using LaboratoryActivityAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LaboratoryActivityAPI.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        protected readonly LabActivityContext _context;
        protected readonly IStateRepository _stateRepository;
        protected readonly IStudentRepository _studentRepository;
        
        public AttendanceRepository(LabActivityContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _context = dbContext;
            _stateRepository = new StateRepository(dbContext);
            _studentRepository = new StudentRepository(dbContext, userManager);
        }
        public async Task<object> Add(AttendanceInputModel attendanceInputModel)
        {
            int stateId = (await _stateRepository.GetDefaultState()).StateId;
            var model = new AttendanceModel
            {
                LabId = attendanceInputModel.LabId,
                StudentId = attendanceInputModel.StudentId,
                StateId = stateId
            };
            _context.Attendance.Add(model);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return "bad request";
            }
            return model;
        }

        public async Task<string> DeleteAllByLab(int labId)
        {
            var attendances = await _context.Attendance.Where(attendance => attendance.LabId == labId).ToListAsync();
            if (attendances == null)
            {
                return "not found";
            }

            _context.Attendance.RemoveRange(attendances);
            await _context.SaveChangesAsync();

            return "no content";
        }

        public async Task<string> DeleteAllByStudent(string studentId)
        {
            var attendances = await _context.Attendance.Where(attendance => attendance.StudentId == studentId).ToListAsync();
            if (attendances == null)
            {
                return "not found";
            }

            _context.Attendance.RemoveRange(attendances);
            await _context.SaveChangesAsync();

            return "no content";
        }

        public async Task<List<AttendanceOutputModel>> GetAllForLab(int labId)
        {
            var attendances =  await _context.Attendance.Where(attendance => attendance.LabId == labId).ToListAsync();
            var attendancesOutputModels = new List<AttendanceOutputModel>();

            foreach(var attendance in attendances)
            {
                var studentUserModel = await _studentRepository.GetById(attendance.StudentId);
                var stateModel = await _stateRepository.GetById(attendance.StateId);
                var attendanceOutput = new AttendanceOutputModel
                {
                    AttendanceId = attendance.AttendanceId,
                    StudentId = studentUserModel.Id,
                    StudentName = studentUserModel.FullName,
                    StateId = attendance.StateId,
                    StateName = stateModel.Name
                };

                attendancesOutputModels.Add(attendanceOutput);
            }

            return attendancesOutputModels;
        }

        public async Task<List<AttendanceOutputModel>> GetAllForStudent(string studentId)
        {
            var attendances = await _context.Attendance.Where(attendance => attendance.StudentId == studentId).ToListAsync();
            var attendancesOutputModels = new List<AttendanceOutputModel>();

            foreach (var attendance in attendances)
            {
                var studentUserModel = await _studentRepository.GetById(attendance.StudentId);
                var stateModel = await _stateRepository.GetById(attendance.StateId);
                var attendanceOutput = new AttendanceOutputModel
                {
                    AttendanceId = attendance.AttendanceId,
                    StudentId = studentUserModel.Id,
                    StudentName = studentUserModel.FullName,
                    StateId = attendance.StateId,
                    StateName = stateModel.Name
                };

                attendancesOutputModels.Add(attendanceOutput);
            }

            return attendancesOutputModels;
        }

        public async Task<object> SetState(AttendanceInputModel attendanceInputModel, int stateId)
        {
            var model = await _context.Attendance.FindAsync(attendanceInputModel.AttendanceId);
            model.StateId = stateId;
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceModelExists(model.AttendanceId))
                {
                    return "not found";
                }
                else
                {
                    throw;
                }
            }

            return "no content";
        }

        private bool AttendanceModelExists(int id)
        {
            return _context.Attendance.Any(e => e.AttendanceId == id);
        }
    }
}
