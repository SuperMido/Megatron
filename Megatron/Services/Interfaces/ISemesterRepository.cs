using Megatron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Services
{
        public interface ISemesterRepository
        {
            IEnumerable<Semester> GetAllSemesters();

            bool CreateSemester(Semester semester);

            bool EditSemester(Semester semester);

            bool DeleteSemester(int id);

            Semester GetSemesterById(int id);

            bool CheckExistSemester(string name);
        }
}
