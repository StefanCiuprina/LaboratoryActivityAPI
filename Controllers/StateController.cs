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

namespace LaboratoryActivityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        IStateRepository _stateRepository;
        public StateController(LabActivityContext context)
        {
            _stateRepository = new StateRepository(context);
        }

        // GET: api/State
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateModel>>> GetAllStates()
        {
            return await _stateRepository.GetAll();
        }


    }
}
