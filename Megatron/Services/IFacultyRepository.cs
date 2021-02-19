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
        bool createFaculty(Faculty faculty);
        bool editFaculty(Faculty faculty);
        bool deleteFaculty(int id);
    }
}