using Megatron.Models;
using System.Collections.Generic;

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