using System.Linq;
using Megatron.Data;
using Megatron.ViewModels;

namespace Megatron.Services
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISemesterRepository _semesterRepository;

        public ReportRepository(ApplicationDbContext dbContext, ISemesterRepository semesterRepository)
        {
            _dbContext = dbContext;
            _semesterRepository = semesterRepository;
        }

        public HomeViewModel HomeViewModel()
        {
            var model = new HomeViewModel()
            {
                Articles = _dbContext.Articles.ToList(),
                Faculties = _dbContext.Faculties.ToList(),
                Semesters = _dbContext.Semesters.ToList(),
                Semester = _semesterRepository.GetActiveSemesterForContributor()
            };

            return model;
        }
    }
}