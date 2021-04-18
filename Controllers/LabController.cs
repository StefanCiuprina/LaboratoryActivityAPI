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
        public async Task<ActionResult<IEnumerable<LabModel>>> GetLab()
        {
            return await _labRepository.GetAll();
        }

        [HttpGet]
        [Route("LabNames")]
        public IList<string> GetAvailableLabNames()
        {
            return _labRepository.GetLabNames();
        }

        [HttpPut]
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
