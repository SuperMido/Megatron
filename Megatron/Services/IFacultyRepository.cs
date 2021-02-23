using Megatron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Services
{
    public interface IFacultyRepository
    {
        IEnumerable<Faculty> GetFaculties();
        Faculty GetFacultyById(int id);
        bool CreateFaculty(Faculty faculty);
        bool EditFaculty(Faculty faculty);
        bool DeleteFaculty(int id);
        bool CheckExistFaculty(string name);
    }
}