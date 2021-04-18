using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LaboratoryActivityAPI.Repositories
{
    public class StateRepository : IStateRepository
    {
        protected readonly LabActivityContext _context;
        public StateRepository(LabActivityContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<List<StateModel>> GetAll()
        {
            List<StateModel> states = await _context.State.ToListAsync();

            foreach (var state in states)
            {
                state.Attendances = await _context.Attendance.Where(attendance => attendance.StateId == state.StateId).ToListAsync();
            }

            return states;
        }

        public async Task<StateModel> GetById(int id)
        {
            var state = await _context.State.Where(st => st.StateId == id).FirstOrDefaultAsync();
            if (state != null)
            {
                state.Attendances = await _context.Attendance.Where(attendance => attendance.StateId == state.StateId).ToListAsync();
            }
            return state;
        }

        public async Task<StateModel> GetDefaultState()
        {
            return await _context.State.FirstOrDefaultAsync();
        }
    }
}
