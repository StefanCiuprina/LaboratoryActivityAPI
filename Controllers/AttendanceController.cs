using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Attendance;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LaboratoryActivityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {

        IAttendanceRepository _attendanceRepository;
        ILabRepository _labRepository;
        IGroupRepository _groupRepository;

        public AttendanceController(LabActivityContext context, UserManager<ApplicationUser> userManager)
        {
            _attendanceRepository = new AttendanceRepository(context, userManager);
            _labRepository = new LabRepository(context);
            _groupRepository = new GroupRepository(context);
        }

        [HttpGet]
        [Route("Lab{labId}")]
        public async Task<ActionResult<IEnumerable<AttendanceOutputModel>>> GetAttendancesForLab(int labId)
        {
            return await _attendanceRepository.GetAllForLab(labId);
        }

        [HttpGet]
        [Route("Student{studentId}")]
        public async Task<ActionResult<IEnumerable<AttendanceOutputModel>>> GetAttendancesForStudent(string studentId)
        {
            return await _attendanceRepository.GetAllForStudent(studentId);
        }


        [HttpPut("{stateId}")]
        public async Task<IActionResult> PutAttendanceModel(AttendanceInputModel attendanceInputModel, int stateId)
        {
            var result = await _attendanceRepository.SetState(attendanceInputModel, stateId);

            if (result.Equals("no content"))
            {
                return NoContent();
            }
            else if (result.Equals("not found"))
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("{labId}")]
        public async Task<ActionResult<AttendanceModel>> PostAttendanceModel(int labId)
        {
            var labModel = await _labRepository.GetById(labId);
            if(labModel == null)
            {
                return BadRequest();
            }
            var groupModel = await _groupRepository.GetById(labModel.GroupId);
            if(groupModel == null)
            {
                return BadRequest();
            }

            object result;

            if(groupModel.Students == null)
            {
                return BadRequest();
            }
            foreach (var studentModel in groupModel.Students)
            {
                var model = new AttendanceInputModel
                {
                    AttendanceId = 0,
                    LabId = labId,
                    StudentId = studentModel.StudentId
                };
                result = await _attendanceRepository.Add(model);
                if(result.Equals("bad request"))
                {
                    return BadRequest();
                }
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("Lab{id}")]
        public async Task<IActionResult> DeleteAttendancesByLab(int id)
        {
            var result = await _attendanceRepository.DeleteAllByLab(id);

            if (result.Equals("no content"))
            {
                return NoContent();
            }
            else if (result.Equals("not found"))
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("Student{id}")]
        public async Task<IActionResult> DeleteAttendancesByStudent(string id)
        {
            var result = await _attendanceRepository.DeleteAllByStudent(id);

            if (result.Equals("no content"))
            {
                return NoContent();
            }
            else if (result.Equals("not found"))
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
