using Megatron.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Services
{
	public interface IFacultyRepository
	{
		IEnumerable<Faculty> GetFaculties();
	}
}
