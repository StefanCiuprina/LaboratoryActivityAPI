using LaboratoryActivityAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.IRepositories
{
    public interface IStudentRepository
    {
        IQueryable<StudentModel> GetAll();

        void SaveChanges();

        Task<Object> Add(ApplicationUserModel model);

        void Update(StudentModel student);

        Task<Object> Delete(string id);

        void Delete(StudentModel student);

        StudentModel GetById(int id);
    }
}
