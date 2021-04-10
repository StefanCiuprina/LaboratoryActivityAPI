using LaboratoryActivityAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.IRepositories
{
    public interface IStudentRepository
    {
        Task<List<ApplicationUserModel>> GetAll();

        void SaveChanges();

        Task<Object> Add(ApplicationUserModel model);

        Task<Object> Update(ApplicationUserModel model);

        Task<Object> Delete(string id);

        void Delete(StudentModel student);

        StudentModel GetById(int id);
    }
}
