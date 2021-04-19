using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Lab;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace LaboratoryActivityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : ControllerBase
    {

        ILabRepository _labRepository;

        public LabController(LabActivityContext context)
        {
            _labRepository = new LabRepository(context); 
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<ActionResult<IEnumerable<LabModel>>> GetLab()
        {
            return await _labRepository.GetAll();
        }

        [HttpGet]
        [Route("Group{groupId}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<ActionResult<IEnumerable<LabModel>>> GetLabsForGroup(int groupId)
        {
            return await _labRepository.GetAllForGroup(groupId);
        }

        [HttpGet]
        [Route("LabNames")]
        [Authorize(Roles = "Teacher,Student")]
        public IList<string> GetAvailableLabNames()
        {
            return _labRepository.GetLabNames();
        }

        [HttpPut]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> PutLabModel(LabInputModel labModel)
        {
            var result = await _labRepository.Update(labModel);

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
        [Authorize(Roles = "Teacher")]
        public async Task<object> PostLabModel(LabInputModel labModel)
        {
            object result;
            if (await _labRepository.GetByName(labModel.Name) == null)
            {
                result = await _labRepository.Add(labModel);
            }
            else
            {
                result = "bad request";
            }

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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteLabModel(int id)
        {
            var result = await _labRepository.Delete(id);

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
