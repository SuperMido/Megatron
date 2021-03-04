using Megatron.Data;
using Megatron.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Services
{
    public class ChartRepository : IChartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<double> CountArticleOfFaculty()
        {
            List<double> data = new List<double>();

            var facultyList = _dbContext.Faculties.OrderBy(f => f.FacultyName).Distinct().ToList();
            var articleList = _dbContext.Articles.ToList();
            var currentYear = DateTime.Now.Year;
            for (var i =0; i < facultyList.Count; i++)
            {
                var facultyId = facultyList[i].Id;
                var articleCount = articleList.Count(a => a.FacultyId == facultyId && a.CreateAt.Year == currentYear);
                data.Add(articleCount);
            }
            return data;
        }

        public List<double> CountContributorsOfFaculty()
        {
            List<double> data = new List<double>();
            var facultyList = _dbContext.Faculties.OrderBy(f => f.FacultyName).Distinct().ToList();
            var articleList = _dbContext.Articles.Select( s => new { author = s.Author, faculty = s.FacultyId }).ToList();
            for (var i =0; i< facultyList.Count; i++)
            {
                var facultyId = facultyList[i].Id;
                var studentCount = articleList.Distinct().Count(a => a.faculty == facultyId);
                data.Add(studentCount);
            }
            return data;
        }

        public List<string> GetFacultyList()
        {
            List<string> data = new List<string>();
            var facultyList = _dbContext.Faculties.OrderBy(f => f.FacultyName).Distinct().ToList();
            for (var i = 0; i < facultyList.Count; i++)
            {
                data.Add(facultyList[i].FacultyName);
            }
            return data;
        }
        public List<double> PercentContributionsOfFaculty(int year)
        {
            List<double> data = new List<double>();

            var facultyList = _dbContext.Faculties.OrderBy(f => f.FacultyName).Distinct().ToList();
            var articleList = _dbContext.Articles.ToList();

            for (var i = 0; i < facultyList.Count; i++)
            {
                var facultyId = facultyList[i].Id;
                var articleCount = articleList.Count(a => a.FacultyId == facultyId && a.CreateAt.Year == year);
                data.Add(articleCount);
            }
            return data;
        }

    }
}
