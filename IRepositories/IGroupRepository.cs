using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Group;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LaboratoryActivityAPI.IRepositories
{
    public interface IGroupRepository
    {
        Task<List<GroupModel>> GetAll();

        Task<GroupModel> GetByName(string name);

        Task<object> Add(GroupInputModel model);

        Task<string> Update(GroupInputModel model);

        Task<string> Delete(int id);

    }
}
