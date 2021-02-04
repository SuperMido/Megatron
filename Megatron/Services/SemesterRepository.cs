using Megatron.Data;
using Megatron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Services
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SemesterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Semester> GetAllSemesters()
        {
            return _dbContext.Semesters;
        }
        public Semester GetSemesterById(int id)
        {
            return _dbContext.Semesters.SingleOrDefault(s => s.Id == id);
        }
        public bool CreateSemester(Semester semester)
        {
            if (CheckExistSemester(semester.SemesterName))
            {
                return false;
            }
            var newSemester = new Semester
            {
                SemesterName = semester.SemesterName,
                SemesterStartDate = semester.SemesterStartDate,
                SemesterEndDate = semester.SemesterEndDate
            };
            _dbContext.Semesters.Add(newSemester);
            _dbContext.SaveChanges();
            return true;
        }
        public bool EditSemester(Semester semester)
        {
            var semesterExist = GetSemesterById(semester.Id);
            if (semesterExist == null) return false;
            semesterExist.SemesterName = semester.SemesterName;
            semesterExist.SemesterStartDate = semester.SemesterStartDate;
            semesterExist.SemesterEndDate = semester.SemesterEndDate;

            _dbContext.SaveChanges();
            return true;
        }
        public bool DeleteSemester(int id)
        {
            var semesterExist = GetSemesterById(id);
            if (semesterExist == null) return false;
            _dbContext.Semesters.Remove(semesterExist);
            _dbContext.SaveChanges();
            return true;

        }

        public bool CheckExistSemester(string name)
        {
            return _dbContext.Semesters.Any(s => s.SemesterName.Contains(name));
        }
    }
}
