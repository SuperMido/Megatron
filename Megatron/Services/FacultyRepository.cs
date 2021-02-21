using Megatron.Data;
using Megatron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public bool editFaculty(Faculty faculty)
        {
            _db.Faculties.Update(faculty);
            _db.SaveChanges();
            return true;
        }

        public bool deleteFaculty(int id)
        {
            var getFacultyInDb = GetFacultyById(id);

            if (getFacultyInDb == null) return false;

            _db.Faculties.Remove(getFacultyInDb);
            _db.SaveChanges();
            return true;
        }

        bool IFacultyRepository.createFaculty(Faculty faculty)
        {
            if (checkExistFaculty(faculty.FacultyName))
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

        public bool checkExistFaculty(string name)
        {
            return _db.Faculties.Any(p => p.FacultyName.Contains(name));
        }
    }
}