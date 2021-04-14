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

namespace LaboratoryActivityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        IGroupRepository _groupRepository;

        private readonly LabActivityContext _context; //TO DELETE

        public GroupController(LabActivityContext context)
        {
            _context = context; //TO DELETE
            _groupRepository = new GroupRepository(context);
        }

        // GET: api/Group
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupModel>>> GetGroup()
        {
            return await _groupRepository.GetAll();
        }

        //=======================================================TO SEE IF NEEDED
        // GET: api/Group/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupModel>> GetGroupModel(int id)
        {
            var groupModel = await _context.Group.FindAsync(id);

            if (groupModel == null)
            {
                return NotFound();
            }

            return groupModel;
        }
        //=======================================================

        // PUT: api/Group/5
        [HttpPut]
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

        // POST: api/Group
        [HttpPost]
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

        // DELETE: api/Group/5
        [HttpDelete("{id}")]
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

        private bool GroupModelExists(int id)
        {
            return _context.Group.Any(e => e.GroupId == id);
        }
    }
}
