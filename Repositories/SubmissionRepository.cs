using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Submission;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaboratoryActivityAPI.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        protected readonly LabActivityContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        public SubmissionRepository(LabActivityContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }
        public async Task<object> Add(SubmissionInputModel submissionInputModel)
        {
            var assignmentModel = await _context.Assignment.FindAsync(submissionInputModel.AssignmentId);

            if (assignmentModel == null)
            {
                return "bad request";
            }

            var model = new SubmissionModel
            {
                AssignmentId = submissionInputModel.AssignmentId,
                StudentId = submissionInputModel.StudentId,
                Link = submissionInputModel.Link,
                Comment = submissionInputModel.Comment,
                Grade = 0,
                SubmissionDate = DateTime.Now
            };
            _context.Submission.Add(model);
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

        public async Task<string> Delete(int submissionId)
        {
            var submissionModel = await _context.Submission.FindAsync(submissionId);
            if (submissionModel == null)
            {
                return "not found";
            }

            _context.Submission.Remove(submissionModel);
            await _context.SaveChangesAsync();

            return "no content";
        }

        public async Task<string> DeleteAllByAssignment(int assignmentId)
        {
            var submissions = await _context.Submission.Where(submission => submission.AssignmentId == assignmentId).ToListAsync();
            if (submissions == null)
            {
                return "not found";
            }

            _context.Submission.RemoveRange(submissions);
            await _context.SaveChangesAsync();

            return "no content";
        }

        public async Task<List<SubmissionOutputModel>> GetAllForAssignment(int assignmentId)
        {
            var submissions = await _context.Submission.Where(submission => submission.AssignmentId == assignmentId).ToListAsync();

            var submissionsOutput = new List<SubmissionOutputModel>();

            foreach (var submission in submissions) {

                var student = await _userManager.FindByIdAsync(submission.StudentId);
                var studentName = student.FullName;

                var submissionOutputModel = new SubmissionOutputModel()
                {
                    SubmissionId = submission.SubmissionId,
                    AssignmentId = submission.AssignmentId,
                    StudentId = submission.StudentId,
                    StudentName = studentName,
                    Link = submission.Link,
                    Comment = submission.Comment,
                    Grade = submission.Grade,
                    SubmissionDate = submission.SubmissionDate
                };

                submissionsOutput.Add(submissionOutputModel);
            }

            return submissionsOutput;
        }

        public async Task<List<SubmissionModel>> GetAllForStudent(string studentId)
        {
            return await _context.Submission.Where(submission => submission.StudentId == studentId).ToListAsync();
        }

        public async Task<SubmissionModel> GetByAssignmentAndStudent(int assignmentId, string studentId)
        {
            return await _context.Submission.Where(submission => (submission.AssignmentId == assignmentId && submission.StudentId == studentId)).FirstOrDefaultAsync();
        }

        public async Task<SubmissionModel> GetById(int submissionId)
        {
            return await _context.Submission.Where(submission => submission.SubmissionId == submissionId).FirstOrDefaultAsync();
        }

        public async Task<object> SetGrade(SubmissionInputModel submissionInputModel)
        {
            var model = await _context.Submission.FindAsync(submissionInputModel.SubmissionId);
            if (model == null)
            {
                return "bad request";
            }
            model.Grade = submissionInputModel.Grade;
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionModelExists(model.SubmissionId))
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

        public async Task<object> Update(SubmissionInputModel submissionInputModel)
        {
            var model = await _context.Submission.FindAsync(submissionInputModel.SubmissionId);
            if (model == null)
            {
                return "bad request";
            }
            model.Link = submissionInputModel.Link;
            model.Comment = submissionInputModel.Comment;
            model.SubmissionDate = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionModelExists(model.SubmissionId))
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

        private bool SubmissionModelExists(int id)
        {
            return _context.Submission.Any(e => e.SubmissionId == id);
        }
    }
}
