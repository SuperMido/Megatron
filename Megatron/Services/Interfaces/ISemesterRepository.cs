using Megatron.Models;
using System.Collections.Generic;
using Megatron.ViewModels;

namespace Megatron.Services
{
    public interface ISemesterRepository
    {
        IEnumerable<Semester> GetAllSemesters();

        SemesterViewModel CreateSemester(SemesterViewModel semesterViewModel);

        SemesterViewModel EditSemester(SemesterViewModel semesterViewModel);
        SemesterViewModel GetSemesterViewModel(int id);

        Semester GetSemesterForArticle(int id);
        
        Semester GetSemesterById(int id);

        bool CheckExistSemester(string name);

        Semester GetActiveSemester();
        Semester GetActiveSemesterForContributor();
        void AddArticleSemester(int articleId, int semesterId);
    }
}