using Megatron.Models;
using Megatron.ViewModels;
using System.Collections.Generic;

namespace Megatron.Services
{
    public interface IChartRepository
    {
        List<double> CountArticleOfFaculty();
        List<string> GetFacultyList();

        List<double> PercentContributionsOfFaculty(int year);

        List<double> CountContributorsOfFaculty();

        int GetArticleWithoutComment();

        int GetArticlesWithoutComment14Days();
    }
}