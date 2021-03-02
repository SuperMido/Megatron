using Megatron.Models;
using Megatron.ViewModels;
using System.Collections.Generic;

namespace Megatron.Services
{
    public interface IChartRepository
    {
        List<double> CountArticleOfFaculty();
        List<string> GetFacultyList();

        List<double> ArticleApprovedOfFaculty();

        List<int> CountArticleOfTopContributors();
        List<string> GetContributorList();


    }
}