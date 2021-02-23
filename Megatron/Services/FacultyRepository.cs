using Megatron.Data;
using Megatron.Models;
using System.Collections.Generic;
using System.Linq;

namespace Megatron.Services
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly ApplicationDbContext _db;
        public FacultyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Faculty> GetFaculties()
        {
            return _db.Faculties.ToList();
        }

        public Faculty GetFacultyById(int id)
        {
            return _db.Faculties.SingleOrDefault(p => p.Id == id);
        }

        public bool EditFaculty(Faculty faculty)
        {
            _db.Faculties.Update(faculty);
            _db.SaveChanges();
            return true;
        }

        public bool DeleteFaculty(int id)
        {
            var facultyInDb = GetFacultyById(id);

            if (facultyInDb == null) return false;

            _db.Faculties.Remove(facultyInDb);
            _db.SaveChanges();
            return true;
        }

        bool IFacultyRepository.CreateFaculty(Faculty faculty)
        {
            if (CheckExistFaculty(faculty.FacultyName))
            {
                return false;
            }
            var newFaculty = new Faculty
            {
                FacultyName = faculty.FacultyName,
                Description = faculty.Description
            };
            _db.Faculties.Add(newFaculty);
            _db.SaveChanges();
            return true;
        }

        public bool CheckExistFaculty(string name)
        {
            return _db.Faculties.Any(f => f.FacultyName.Contains(name));
        }
    }
}