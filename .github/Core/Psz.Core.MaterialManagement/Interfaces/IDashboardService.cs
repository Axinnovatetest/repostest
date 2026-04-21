using Psz.Core.MaterialManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Interfaces
{
	public interface IDashboardService
	{
		public ResponseModel<DashboardResponseModel> GetDashboardData(UserModel user, int data);
	}
}
