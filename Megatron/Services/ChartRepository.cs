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
            
            for (var i =0; i < facultyList.Count; i++)
            {
                var facultyId = facultyList[i].Id;
                var articleCount = articleList.Count(a => a.FacultyId == facultyId);
                data.Add(articleCount);
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
        public List<double> ArticleApprovedOfFaculty()
        {
            List<double> data = new List<double>();

            var facultyList = _dbContext.Faculties.OrderBy(f => f.FacultyName).Distinct().ToList();
            var articleList = _dbContext.Articles.Where(a => a.Status == true).ToList();

            for (var i = 0; i < facultyList.Count; i++)
            {
                var facultyId = facultyList[i].Id;
                var articleCount = articleList.Count(a => a.FacultyId == facultyId);
                data.Add(articleCount);
            }
            return data;
        }
        public List<int> CountArticleOfTopContributors()
        {
            List<int> count = new List<int>();

            SortedList<string, int> data = new SortedList<string, int>();

            var listUsers = (from user in _dbContext.ApplicationUsers
                             join userRoles in _dbContext.UserRoles on user.Id equals userRoles.UserId
                             join role in _dbContext.Roles on userRoles.RoleId equals role.Id
                             select new { UserId = user.Id, FullName = user.FullName, Email = user.Email, RoleName = role.Name, DateCreate = user.CreateAt })
                .ToList();

            var studentList = listUsers.Where(u => u.RoleName == "Student").OrderBy(s => s.UserId).ToList();
            var articleList = _dbContext.Articles.Where(a => a.Status == true).ToList();

            for (var i = 0; i < studentList.Count; i++)
            {
                var student = studentList[i].FullName;
                var articleCount = articleList.Count(a => a.Author == student);
                data.Add(student, articleCount);
            }
            var orderByVal = data.OrderBy(v => v.Value);
            foreach (var c in orderByVal)
            {
                count.Add(c.Value);
            }

            return count;
        }
        public List<string> GetContributorList()
        {
            List<string> users = new List<string>();
            SortedList<string, int> data = new SortedList<string, int>();

            var listUsers = (from user in _dbContext.ApplicationUsers
                             join userRoles in _dbContext.UserRoles on user.Id equals userRoles.UserId
                             join role in _dbContext.Roles on userRoles.RoleId equals role.Id
                             select new { UserId = user.Id, FullName = user.FullName, Email = user.Email, RoleName = role.Name, DateCreate = user.CreateAt })
                .ToList();

            var studentList = listUsers.Where(u => u.RoleName == "Student").OrderBy(s => s.UserId).ToList();
            var articleList = _dbContext.Articles.Where(a => a.Status == true).ToList();

            for (var i = 0; i < studentList.Count; i++)
            {
                var student = studentList[i].FullName;
                var articleCount = articleList.Count(a => a.Author == student);
                data.Add(student, articleCount);
            }
            var orderByval = data.OrderBy(v => v.Value);
            foreach (var c in orderByval)
            {
                users.Add(c.Key);
            }

            return users;
        }
    }
}
