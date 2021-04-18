using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Assignment;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Repositories;

namespace LaboratoryActivityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        IAssignmentRepository _assignmentRepository;

        public AssignmentController(LabActivityContext context)
        {
            _assignmentRepository = new AssignmentRepository(context);
        }

        /*[HttpGet("{labId}")]
        public async Task<List<AssignmentModel>> GetForLab(int labId)
        {
            var assignmentList = new List<AssignmentModel>();
            var assignment = await _assignmentRepository.GetForLab(labId);
            assignmentList.Add(assignment);
            return assignmentList;
        }*/

        [HttpGet("{labId}")]
        public async Task<AssignmentModel> GetForLab(int labId)
        {
            return await _assignmentRepository.GetForLab(labId);
        }

        [HttpGet]
        [Route("Group{groupId}")]
        public async Task<List<AssignmentModel>> GetAllForGroup(int groupId)
        {
            return await _assignmentRepository.GetAllForGroup(groupId);
        }

        [HttpPut]
        public async Task<IActionResult> PutAssignmentModel(AssignmentInputModel assignmentInputModel)
        {
            var result = await _assignmentRepository.Update(assignmentInputModel);

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
        public async Task<object> PostAssignmentModel(AssignmentInputModel assignmentInputModel)
        {
            var result = await _assignmentRepository.Add(assignmentInputModel);

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
        public async Task<IActionResult> DeleteAssignmentModel(int id)
        {
            var result = await _assignmentRepository.Delete(id);

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
        [Route("Lab{id}")]
        public async Task<IActionResult> DeleteAssignmentModelByLab(int id)
        {
            var result = await _assignmentRepository.DeleteByLab(id);

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
