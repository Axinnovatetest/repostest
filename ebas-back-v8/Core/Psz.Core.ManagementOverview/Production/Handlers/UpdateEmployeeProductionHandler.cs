using Infrastructure.Data.Access.Joins.MGO;
using Psz.Core.Common.Models;
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
		public ResponseModel<int> UpdateEmployeeProduction(Identity.Models.UserModel user, Models.EmployeeProductionModel data)
		{
			try
			{
				var validationResponse = this.ValidateUpdate(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				botransaction.beginTransaction();
				ProductionAccess.UpdateEmployeeProductionithTransaction(
					new Infrastructure.Data.Entities.Tables.MGO.EmployeeProductionEntity
					{
						Id = data.Id,
						KW = data.KW,
						Jahr = data.Jahr,
						AnzahlDirektMitarbeiter = data.AnzahlDirektMitarbeiter,
						AnzahlDirektMitarbeiterWertschoepfend= data.AnzahlDirektMitarbeiterWertschoepfend,
						AnzahlDirektMitarbeiterNichtWertschoepfend = data.AnzahlDirektMitarbeiterNichtWertschoepfend,
						AnzahlIndirektMitarbeiter = data.AnzahlIndirektMitarbeiter,
						GeplanteEinstellungenDirekt = data.GeplanteEinstellungenDirekt,
						GeplanteEinstellungenIndirekt = data.GeplanteEinstellungenIndirekt,
						Austritte = data.Austritte
					}, botransaction.connection, botransaction.transaction);
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(1);
				}
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Problem in Running");

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateUpdate(Identity.Models.UserModel user, Models.EmployeeProductionModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var employeeProductionEntity = ProductionAccess.GetEmployeeProductionByLagerJahrKW(data.Lagerort_id, data.Jahr, data.KW);
			if(employeeProductionEntity != null && employeeProductionEntity.Id > 0 && employeeProductionEntity.Id!=data.Id)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Employee Production found");



			return ResponseModel<int>.SuccessResponse();
		}
	}
}
