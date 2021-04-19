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

        Task<ApplicationUser> GetById(string id);

        Task<ApplicationUserModel> GetByUsernameAsModel(string username);

        Task<ApplicationUserModel> GetByIdAsModel(string id);

        Task<Object> Add(ApplicationUserModel model);

        Task<Object> Update(ApplicationUserModel model);

        Task<Object> SetStudentRegistered(ApplicationUserModel model);

        Task<Object> Delete(string id);

        Task<bool> IsStudentRegistered(string id);
    }
}
