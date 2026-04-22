using Infrastructure.Data.Access.Joins.MGO;
using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.ManagementOverview.Production.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Handlers
{
	public partial class ProductionService: IProductionService
	{
		public ResponseModel<int> AddEmployeeProduction(Identity.Models.UserModel user, Models.EmployeeProductionModel data)
		{
			try
			{
				var validationResponse = this.ValidateAddEmployeeProduction(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				///
				int insertedUsers = 0;
				insertedUsers=ProductionAccess.InsertEmployeeProduction(new Infrastructure.Data.Entities.Tables.MGO.EmployeeProductionEntity
				{
					Lagerort_id=data.Lagerort_id,
					KW=data.KW,
					Jahr=data.Jahr,
					AnzahlDirektMitarbeiter=data.AnzahlDirektMitarbeiter,
					AnzahlDirektMitarbeiterWertschoepfend = data.AnzahlDirektMitarbeiterWertschoepfend,
					AnzahlDirektMitarbeiterNichtWertschoepfend = data.AnzahlDirektMitarbeiterNichtWertschoepfend,
					AnzahlIndirektMitarbeiter =data.AnzahlIndirektMitarbeiter,
					GeplanteEinstellungenDirekt=data.GeplanteEinstellungenDirekt,
					GeplanteEinstellungenIndirekt=data.GeplanteEinstellungenIndirekt,
					Austritte=data.Austritte
				});

				

				return ResponseModel<int>.SuccessResponse(insertedUsers);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateAddEmployeeProduction(Identity.Models.UserModel user, Models.EmployeeProductionModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var employeeProductionEntity = ProductionAccess.GetEmployeeProductionByLagerJahrKW(data.Lagerort_id,data.Jahr, data.KW);
			if(employeeProductionEntity != null && employeeProductionEntity.Id>0)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Employee Production found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
