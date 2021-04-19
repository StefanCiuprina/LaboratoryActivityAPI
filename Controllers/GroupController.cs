using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Repositories;
using LaboratoryActivityAPI.Models.Group;
using Microsoft.AspNetCore.Authorization;

namespace LaboratoryActivityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        IGroupRepository _groupRepository;

        public GroupController(LabActivityContext context)
        {
            _groupRepository = new GroupRepository(context);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<ActionResult<IEnumerable<GroupModel>>> GetGroup()
        {
            return await _groupRepository.GetAll();
        }

        [HttpPut]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> PutGroupModel(GroupInputModel groupModel)
        {
            var result = await _groupRepository.Update(groupModel);

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
        public async Task<object> PostGroupModel(GroupInputModel groupModel)
        {
            object result;
            if (await _groupRepository.GetByName(groupModel.Name) == null)
            {
                result = await _groupRepository.Add(groupModel);
            } else
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
        public async Task<IActionResult> DeleteGroupModel(int id)
        {
            var result = await _groupRepository.Delete(id);

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
