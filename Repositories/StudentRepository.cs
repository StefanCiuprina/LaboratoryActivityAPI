﻿using LaboratoryActivityAPI.IRepositories;
using LaboratoryActivityAPI.Models;
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

        public async Task<List<ApplicationUserModel>> GetAll()
        {
            
            return await Task.Run(async () =>
            {
                var studentAccounts = _userManager.GetUsersInRoleAsync("Student").Result.ToList();
                var studentDetails = await _dbContext.Student.ToListAsync();

                var students = new List<ApplicationUserModel>();
                for(int i = 0; i < studentAccounts.Count(); i++)
                {
                    var studentAccount = studentAccounts[i];
                    var studentDetail = studentDetails[i];
                    ApplicationUserModel student = new ApplicationUserModel()
                    {
                        Id = studentAccount.Id,
                        UserName = studentAccount.UserName,
                        Email = studentAccount.Email,
                        FullName = studentAccount.FullName,
                        GroupId = studentDetail.GroupId,
                        Hobby = studentDetail.Hobby,
                        Token = studentDetail.Token
                    };
                    students.Add(student);
                }
                return students;
            });

        }

        public StudentModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public async Task<Object> Update(ApplicationUserModel model)
        {

            var user = await _userManager.FindByIdAsync(model.Id);

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FullName = model.FullName;

            IdentityResult result;

            try
            {
                result = await _userManager.UpdateAsync(user);
            }
            catch (Exception)
            {
                return "bad request";
            }

            var studentModel = new StudentModel()
            {
                StudentId = model.Id,
                GroupId = model.GroupId,
                Hobby = model.Hobby,
                Token = model.Token,
                User = user
            };

            _dbContext.Entry(studentModel).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return "no content";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentModelExists(model.Id))
                {
                    return "not found";
                }
                else
                {
                    return "bad request";
                }
            }

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
