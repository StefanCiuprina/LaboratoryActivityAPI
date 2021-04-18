using LaboratoryActivityAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratoryActivityAPI.IRepositories
{
    public interface IStateRepository
    {
        Task<List<StateModel>> GetAll();

        Task<StateModel> GetById(int id);

        Task<StateModel> GetDefaultState();

    }
}
