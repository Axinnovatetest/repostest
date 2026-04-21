using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Production.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Interfaces
{
	public interface IProductionService
	{
		ResponseModel<Models.PackEmployeeProductionModel> GetListeEmployeeProduction(Identity.Models.UserModel user, int lager,int typeLoading);
		ResponseModel<int> AddEmployeeProduction(Identity.Models.UserModel user, Models.EmployeeProductionModel data);
		ResponseModel<int> DeleteEmployeeProduction(Identity.Models.UserModel user, int id);
		ResponseModel<int> UpdateEmployeeProduction(Identity.Models.UserModel user, Models.EmployeeProductionModel data);
		ResponseModel<Models.PackGeplanteStundenModel> GetListePlanungStunden(Identity.Models.UserModel user, int lager, int typeLoading);
		ResponseModel<byte[]> GetListePlanungStundenByBlocExcel(Identity.Models.UserModel user, LagerRequest request);
		ResponseModel<PackGeplantStundenByTypModel> GetListePlanungStundenByTyp(Identity.Models.UserModel user, int lager, int typeLoading);
		ResponseModel<PackWertProductionModel> GetListeWertLager(Identity.Models.UserModel user, int lager);
	}
}
