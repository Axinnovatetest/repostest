using Infrastructure.Data.Access.Joins.MGO;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Production.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Handlers
{
	public partial class ProductionService: IProductionService
	{
		public ResponseModel<int> DeleteEmployeeProduction(Identity.Models.UserModel user, int id)
		{
			try
			{
				var validationResponse = this.ValidateDelete(user, id);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				botransaction.beginTransaction();
				ProductionAccess.DeleteEmployeeProductionithTransaction(id, botransaction.connection, botransaction.transaction);
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(1);
				}
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Problem in Running");

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{id}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateDelete(Identity.Models.UserModel user, int id)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			var employee=ProductionAccess.GetEmployeeProductionById(id);
			if(employee != null)
			{
				int anneeActuelle = DateTime.Now.Year;
				DateTime dateActuelle = DateTime.Now;
				CultureInfo cultureInfo = CultureInfo.CurrentCulture;
				Calendar calendrier = cultureInfo.Calendar;
               int semaineActuelle = calendrier.GetWeekOfYear(dateActuelle, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
				if (employee.Jahr< anneeActuelle ||(employee.Jahr == anneeActuelle && employee.KW <= semaineActuelle))
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Date not accesible to delete");
				}
			}




			return ResponseModel<int>.SuccessResponse();
		}
	}
}
