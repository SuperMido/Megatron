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
		private readonly ApplicationDbContext _dbContext;
		public FacultyRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public IEnumerable<Faculty> GetFaculties()
		{
			return _dbContext.Faculties.ToList();
		}
	}
}
