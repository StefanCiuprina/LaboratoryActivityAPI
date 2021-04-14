using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Group;
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
    public class GroupRepository : IGroupRepository
    {
        protected readonly LabActivityContext _context;
        public GroupRepository(LabActivityContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<object> Add(GroupInputModel groupInputModel)
        {
            var model = new GroupModel
            {
                GroupId = groupInputModel.GroupId,
                Name = groupInputModel.Name,
                NumberOfStudents = 0
            };
            _context.Group.Add(model);
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
            var groupModel = await _context.Group.FindAsync(id);
            if (groupModel == null)
            {
                return "not found";
            }

            _context.Group.Remove(groupModel);
            await _context.SaveChangesAsync();

            return "no content";
        }

        public Task<List<GroupModel>> GetAll()
        {
            return _context.Group.ToListAsync();
        }

        public async Task<GroupModel> GetByName(string name)
        {
            return await _context.Group.FirstOrDefaultAsync(group => group.Name == name);
        }

        public async Task<string> Update(GroupInputModel groupInputModel)
        {
            var model = await _context.Group.FindAsync(groupInputModel.GroupId);
            model.Name = groupInputModel.Name;
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupModelExists(model.GroupId))
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

        private bool GroupModelExists(int id)
        {
            return _context.Group.Any(e => e.GroupId == id);
        }
    }
}
