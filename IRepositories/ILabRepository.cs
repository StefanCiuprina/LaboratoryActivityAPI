using LaboratoryActivityAPI.Models;
using LaboratoryActivityAPI.Models.Lab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.IRepositories
{
    public interface ILabRepository
    {
        Task<List<LabModel>> GetAll();

        Task<LabModel> GetByName(string name);

        Task<LabModel> GetById(int labId);

        Task<object> Add(LabInputModel model);

        Task<string> Update(LabInputModel model);

        Task<string> Delete(int id);

        List<string> GetLabNames();

    }
}
