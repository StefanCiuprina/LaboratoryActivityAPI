using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        protected AuthenticationContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        protected DbSet<StudentModel> _dbSet;

        public StudentRepository(AuthenticationContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _dbSet = dbContext.Set<StudentModel>();
        }

        public async Task<Object> Add(ApplicationUserModel model)
        {
            string id = await createStudentAccount(model);
            if (!id.Equals(""))
            {
                var result = addStudentDetails(id, model);
                return result;
            } else
            {
                return "bad request";
            }
        }

        public async Task<Object> Delete(string id)
        {
            var studentModel = await _dbContext.Student.FindAsync(id);
            if (studentModel == null)
            {
                return "not found";
            }

            _dbContext.Student.Remove(studentModel);
            await _dbContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(id);

            try
            {
                var result = await _userManager.DeleteAsync(user);
                return "no content";
            }
            catch (Exception)
            {
                return "bad request";
            }
        }

        public void Delete(StudentModel student)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public StudentModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(StudentModel student)
        {
            throw new NotImplementedException();
        }

        private async Task<string> createStudentAccount(ApplicationUserModel model)
        {
            var role = "Student";
            var defaultPassword = "12345";
            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, defaultPassword);
                await _userManager.AddToRoleAsync(applicationUser, role);
                if (result.Succeeded)
                {
                    return applicationUser.Id;
                } else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            };
        }

        private async Task<Object> addStudentDetails(string id, ApplicationUserModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            var student = new StudentModel
            {
                GroupId = model.GroupId,
                Hobby = model.Hobby,
                Token = model.Token,
                User = user,
            };

            _dbContext.Student.Add(student);
            try
            {
                await _dbContext.SaveChangesAsync();
                user.Student = student;
                await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateException)
            {
                if (StudentModelExists(id))
                {
                    return "conflict";
                }
                else
                {
                    return "bad request";
                }
            }

            return student;
        }
        private bool StudentModelExists(string id)
        {
            return _dbContext.Student.Any(e => e.StudentId == id);
        }
    }
}
