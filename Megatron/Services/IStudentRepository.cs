using Megatron.Models;
using Megatron.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Services
{
	public interface IStudentRepository
	{
		ArticleFacultyViewModel ArticleFacultyViewModel();

		ArticleFacultyViewModel SubmitArticle(ArticleFacultyViewModel articleFacultyViewModel);
		bool CheckExistArticleTitle(string title);
		bool EditArticle(Article article);
		Article GetArticleById(int id);
		IEnumerable<Article> GetAllArticle();
	}
}