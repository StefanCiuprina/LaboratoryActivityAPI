using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Submission;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LaboratoryActivityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        ISubmissionRepository _submissionRepository;
        public SubmissionController(LabActivityContext context, UserManager<ApplicationUser> userManager)
        {
            _submissionRepository = new SubmissionRepository(context, userManager);
        }

        [Authorize(Roles = "Teacher,Student")]
        [HttpGet("{assignmentId}")]
        public async Task<ActionResult<IEnumerable<SubmissionOutputModel>>> GetSubmissionForAssignment(int assignmentId)
        {
            return await _submissionRepository.GetAllForAssignment(assignmentId);
        }

        [HttpGet]
        [Route("Student{studentId}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<ActionResult<IEnumerable<SubmissionModel>>> GetSubmissionForStudent(string studentId)
        {
            return await _submissionRepository.GetAllForStudent(studentId);
        }

        [HttpGet]
        [Route("Submission{submissionId}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<SubmissionModel> GetSubmissionForStudent(int submissionId)
        {
            return await _submissionRepository.GetById(submissionId);
        }

        [HttpGet]
        [Route("ByStudentAndAssignment{assignmentId}/{studentId}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<SubmissionModel> GetSubmissionForStudent(int assignmentId, string studentId)
        {
            return await _submissionRepository.GetByAssignmentAndStudent(assignmentId, studentId);
        }

        [HttpPut]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PutSubmissionModel(SubmissionInputModel submissionInputModel)
        {
            var result = await _submissionRepository.Update(submissionInputModel);

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

        [HttpPut]
        [Route("Grade")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> PutSubmissionModelGrade(SubmissionInputModel submissionInputModel)
        {
            var result = await _submissionRepository.SetGrade(submissionInputModel);

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

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<object> PostSubmissionModel(SubmissionInputModel submissionInputModel)
        {
            object result;
            result = await _submissionRepository.Add(submissionInputModel);

            if (result.Equals("bad request"))
            {
                return BadRequest();
            }
            else
            {
                return result;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<IActionResult> DeleteSubmissionModel(int id)
        {
            var result = await _submissionRepository.Delete(id);

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
        [Route("Assignment{id}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<IActionResult> DeleteSubmissionModelByAssignment(int id)
        {
            var result = await _submissionRepository.DeleteAllByAssignment(id);

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
