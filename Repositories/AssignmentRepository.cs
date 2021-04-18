using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Assignment;
using Microsoft.EntityFrameworkCore;

namespace LaboratoryActivityAPI.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        protected readonly LabActivityContext _context;

        public AssignmentRepository(LabActivityContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<object> Add(AssignmentInputModel assignmentInputModel)
        {
            var lab = await _context.Lab.FindAsync(assignmentInputModel.LabId);
            if(lab == null)
            {
                return "bad request";
            }
            var model = new AssignmentModel
            {
                LabId = assignmentInputModel.LabId,
                Name = assignmentInputModel.Name,
                Deadline = assignmentInputModel.Deadline,
                Description = assignmentInputModel.Description
            };
            _context.Assignment.Add(model);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return "bad request";
            }
            return model;
        }

        public async Task<string> Delete(int assignmentId)
        {
            var assignmentModel = await _context.Assignment.FindAsync(assignmentId);
            if (assignmentModel == null)
            {
                return "not found";
            }

            _context.Assignment.Remove(assignmentModel);
            await _context.SaveChangesAsync();

            return "no content";
        }

        public async Task<string> DeleteByLab(int labId)
        {
            var assignment = await _context.Assignment.Where(assignmentModel => assignmentModel.LabId == labId).FirstOrDefaultAsync();
            if (assignment == null)
            {
                return "not found";
            }

            _context.Assignment.Remove(assignment);
            await _context.SaveChangesAsync();

            return "no content";
        }

        public async Task<List<AssignmentModel>> GetAllForGroup(int groupId)
        {
            var group = await _context.Group.Where(groupModel => groupModel.GroupId == groupId).FirstOrDefaultAsync();
            if(group == null)
            {
                return null;
            }

            var labs = await _context.Lab.Where(lab => lab.GroupId == groupId).ToListAsync();
            if(labs == null)
            {
                return null;
            }

            var assignments = new List<AssignmentModel>();
            foreach(var lab in labs)
            {
                var assignment = await _context.Assignment.Where(assignmentModel => assignmentModel.LabId == lab.LabId).FirstOrDefaultAsync();
                if(assignment != null)
                {
                    assignment.Submissions = await _context.Submission.Where(submission => submission.AssignmentId == assignment.AssignmentId).ToListAsync();
                    assignments.Add(assignment);
                }
            }

            return assignments;
        }

        public async Task<AssignmentModel> GetForLab(int labId)
        {
            var assignment = await _context.Assignment.Where(assignmentModel => assignmentModel.LabId == labId).FirstOrDefaultAsync();
            if(assignment == null)
            {
                return null;
            }
            assignment.Submissions = await _context.Submission.Where(submission => submission.AssignmentId == assignment.AssignmentId).ToListAsync();
            return assignment;
        }

        public async Task<object> Update(AssignmentInputModel assignmentInputModel)
        {
            var model = await _context.Assignment.FindAsync(assignmentInputModel.AssignmentId);
            if (model == null)
            {
                return "bad request";
            }

            //USER CANNOT MODIFY LAB ID FOR ASSIGNMENT
            model.Name = assignmentInputModel.Name;
            model.Deadline = assignmentInputModel.Deadline;
            model.Description = assignmentInputModel.Description;

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentModelExists(model.AssignmentId))
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

        private bool AssignmentModelExists(int id)
        {
            return _context.Assignment.Any(e => e.AssignmentId == id);
        }
    }
}
