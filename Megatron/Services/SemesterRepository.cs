using Megatron.Data;
using Megatron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Megatron.ViewModels;

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
            return _dbContext.Semesters.ToList();
        }

        public Semester GetSemesterById(int id)
        {
            return _dbContext.Semesters.SingleOrDefault(s => s.Id == id);
        }

        public SemesterViewModel CreateSemester(SemesterViewModel semesterViewModel)
        {
            if (CheckExistSemester(semesterViewModel.Semester.SemesterName))
            {
                var model = new SemesterViewModel()
                {
                    Semester = semesterViewModel.Semester,
                    StatusMessage = "Error: Semester already exists!"
                };
                return model;
            }
            
            var semesterInDb = GetAllSemesters().OrderByDescending(s => s.Id).FirstOrDefault();
            var checkValidationSemester = CheckSemesterValidation(semesterViewModel, semesterInDb);
            if (checkValidationSemester)
            {
                var newSemester = new Semester
                {
                    SemesterName = semesterViewModel.Semester.SemesterName,
                    SemesterStartDate = semesterViewModel.Semester.SemesterStartDate,
                    SemesterClosureDate = semesterViewModel.Semester.SemesterClosureDate,
                    SemesterEndDate = semesterViewModel.Semester.SemesterEndDate
                };
                _dbContext.Semesters.Add(newSemester);
                _dbContext.SaveChanges();
                return null;
            }

            if (semesterViewModel.Semester.SemesterStartDate > semesterViewModel.Semester.SemesterClosureDate)
            {
                var model = new SemesterViewModel()
                {
                    Semester = semesterViewModel.Semester,
                    StatusMessage = "Error: Closure Date must be greater than the Start Date!"
                };
                return model;
            }

            if (semesterViewModel.Semester.SemesterClosureDate > semesterViewModel.Semester.SemesterEndDate)
            {
                var model = new SemesterViewModel()
                {
                    Semester = semesterViewModel.Semester,
                    StatusMessage = "Error: Final Date must be greater than the Closure Date!"
                };
                return model;
            }

            if (semesterInDb.SemesterEndDate > semesterViewModel.Semester.SemesterStartDate)
            {
                var model = new SemesterViewModel()
                {
                    Semester = semesterViewModel.Semester,
                    StatusMessage = "Error: Please check Start Date again!"
                };
                return model;
            }

            var errorModel = new SemesterViewModel()
            {
                Semester = semesterViewModel.Semester,
                StatusMessage = "Error: There are something wrong then create semester, please check again!"
            };
            return errorModel;
        }

        public SemesterViewModel EditSemester(SemesterViewModel semesterViewModel)
        {
            var semesterExist = GetSemesterById(semesterViewModel.Semester.Id);
            if (semesterExist == null)
            {
                var model = new SemesterViewModel()
                {
                    Semester = semesterViewModel.Semester,
                    StatusMessage = "Error: Semester already exists!"
                };
                return model;
            }

            if (semesterViewModel.Semester.SemesterStartDate < semesterViewModel.Semester.SemesterClosureDate &&
                semesterViewModel.Semester.SemesterClosureDate < semesterViewModel.Semester.SemesterEndDate)
            {
                semesterExist.SemesterName = semesterViewModel.Semester.SemesterName;
                semesterExist.SemesterStartDate = semesterViewModel.Semester.SemesterStartDate;
                semesterExist.SemesterClosureDate = semesterViewModel.Semester.SemesterClosureDate;
                semesterExist.SemesterEndDate = semesterViewModel.Semester.SemesterEndDate;
                semesterExist.UpdateAt = DateTime.Now;

                _dbContext.SaveChanges();
                return null;
            }

            if (semesterViewModel.Semester.SemesterStartDate > semesterViewModel.Semester.SemesterClosureDate)
            {
                var model = new SemesterViewModel()
                {
                    Semester = semesterViewModel.Semester,
                    StatusMessage = "Error: Closure Date must be greater than the Start Date!"
                };
                return model;
            }

            if (semesterViewModel.Semester.SemesterClosureDate > semesterViewModel.Semester.SemesterEndDate)
            {
                var model = new SemesterViewModel()
                {
                    Semester = semesterViewModel.Semester,
                    StatusMessage = "Error: Final Date must be greater than the Closure Date!"
                };
                return model;
            }

            var errorModel = new SemesterViewModel()
            {
                Semester = semesterViewModel.Semester,
                StatusMessage = "Error: There are something wrong then create semester, please check again!"
            };
            return errorModel;
        }

        public SemesterViewModel GetSemesterViewModel(int id)
        {
            var semesterInDb = _dbContext.Semesters.SingleOrDefault(s => s.Id == id);
            var model = new SemesterViewModel()
            {
                Semester = semesterInDb
            };
            return model;
        }

        public bool CheckExistSemester(string name)
        {
            return _dbContext.Semesters.Any(s => s.SemesterName.Contains(name));
        }

        public Semester GetActiveSemester()
        {
            var currentDateTime = DateTime.Now;
            if (CheckSemesterDateValidToSubmit(currentDateTime))
            {
                return _dbContext.Semesters.FirstOrDefault(s =>
                    s.SemesterStartDate < currentDateTime && s.SemesterClosureDate > currentDateTime);
            }

            return null;
        }

        public Semester GetActiveSemesterForContributor()
        {
            var currentDateTime = DateTime.Now;
            if (CheckSemesterDateValidToSubmitAndEdit(currentDateTime))
            {
                return _dbContext.Semesters.FirstOrDefault(s =>
                    s.SemesterStartDate < currentDateTime && s.SemesterEndDate > currentDateTime);
            }

            return null;
        }

        public void AddArticleSemester(int articleId, int semesterId)
        {
            var articleSemester = new SemesterArticle()
            {
                ArticleId = articleId,
                SemesterId = semesterId
            };

            _dbContext.SemesterArticles.Add(articleSemester);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Semester> GetListSemestersAfterFinalDate()
        {
            var currentDateTime = GetCurrentDateTime();

            var semesterAfterFinalDate = _dbContext.Semesters.Where(s => s.SemesterEndDate < currentDateTime).ToList();

            return semesterAfterFinalDate.Any() ? semesterAfterFinalDate : null;
        }

        private bool CheckSemesterDateValidToSubmit(DateTime currentDateTime)
        {
            var semesterInDb = _dbContext.Semesters.Where(s =>
                s.SemesterStartDate < currentDateTime && s.SemesterClosureDate > currentDateTime).ToList();
            return semesterInDb.Any();
        }

        private bool CheckSemesterDateValidToSubmitAndEdit(DateTime currentDateTime)
        {
            var semesterInDb = _dbContext.Semesters.Where(s =>
                s.SemesterStartDate < currentDateTime && s.SemesterEndDate > currentDateTime).ToList();
            return semesterInDb.Any();
        }

        private static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        private bool CheckSemesterValidation(SemesterViewModel semesterViewModel, Semester semesterInDb)
        {
            if (semesterInDb == null || (semesterViewModel.Semester.SemesterStartDate <
                                         semesterViewModel.Semester.SemesterClosureDate &&
                                         semesterViewModel.Semester.SemesterClosureDate <
                                         semesterViewModel.Semester.SemesterEndDate &&
                                         semesterInDb.SemesterEndDate < semesterViewModel.Semester.SemesterStartDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}