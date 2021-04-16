using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Lab;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Repositories
{
    
    public class LabRepository : ILabRepository
    {
        protected readonly LabActivityContext _context;
        public LabRepository(LabActivityContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<object> Add(LabInputModel labInputModel)
        {
            var groupModel = await _context.Group.FindAsync(labInputModel.GroupId);

            if ((await labNameExists(labInputModel.Name) > 0) || groupModel == null)
            {
                return "bad request";
            }
            var model = new LabModel
            {
                GroupId = labInputModel.GroupId,
                Name = labInputModel.Name,
                DateTime = labInputModel.DateTime,
                Title = labInputModel.Title,
                Description = labInputModel.Description,
                Curricula = labInputModel.Curricula,
            };
            _context.Lab.Add(model);
            try
            {
                await _context.SaveChangesAsync();
            } catch (Exception)
            {
                return "bad request";
            }
            return model;
        }

        public async Task<string> Delete(int id)
        {
            var labModel = await _context.Lab.FindAsync(id);
            if (labModel == null)
            {
                return "not found";
            }
            
            _context.Lab.Remove(labModel);
            await _context.SaveChangesAsync();

            return "no content";
        }

        public async Task<List<LabModel>> GetAll()
        {
            List<LabModel> labs = await _context.Lab.ToListAsync();

            foreach (var lab in labs)
            {
                lab.Attendances = await _context.Attendance.Where(attendance => attendance.LabId == lab.LabId).ToListAsync();
                lab.Assignment = await _context.Assignment.Where(assignment => assignment.LabId == lab.LabId).FirstOrDefaultAsync();
            }

            return labs;
        }

        public async Task<LabModel> GetByName(string name)
        {
            return await _context.Lab.FirstOrDefaultAsync(lab => lab.Name == name);
        }

        public async Task<string> Update(LabInputModel labInputModel)
        {

            if (await labNameExists(labInputModel.Name) != labInputModel.LabId)
            {
                return "bad request";
            }

            var model = await _context.Lab.FindAsync(labInputModel.LabId);
            if(model == null)
            {
                return "bad request";
            }
            model.Name = labInputModel.Name;
            model.DateTime = labInputModel.DateTime;
            model.Title = labInputModel.Title;
            model.Curricula = labInputModel.Curricula;
            model.Description = labInputModel.Description;
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabModelExists(model.GroupId))
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

        public async Task<int> labNameExists(string name)
        {
            var lab = await _context.Lab.Where(l => l.Name == name).FirstOrDefaultAsync();
            if(lab == null)
            {
                return 0;
            }
            return lab.LabId;
        }

        public List<string> GetLabNames()
        {
            var labNames = new List<string>();
            const string pre = "Lab no. ";

            //valid lab names are from 1 to 14
            for(int i = 1; i <= 14; i++)
            {
                var currentLabName = pre + i;
                labNames.Add(currentLabName);
            }

            return labNames;
        }

        private bool LabModelExists(int id)
        {
            return _context.Lab.Any(e => e.LabId == id);
        }
    }
}
