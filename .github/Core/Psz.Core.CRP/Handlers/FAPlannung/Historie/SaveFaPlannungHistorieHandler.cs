using Infrastructure.Services.Utils;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<int> SaveFaPlannungHistorie(UserModel user, Models.FAPlanning.Historie.HistorieFaPlannungSaveRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			if(data.Details == null || data.Details.Count <= 0)
				return ResponseModel<int>.FailureResponse("Empty data nothing to save.");
			var transaction = new TransactionsManager();
			try
			{
				var _header = new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity
				{
					DateHistorie = data.HistoryDate,
					DateImport = DateTime.Now,
					ImportTyeName = Enums.CRPEnums.FaPlannungHistorieImportType.ByExcel.GetDescription(),
					ImportTypeId = (int)Enums.CRPEnums.FaPlannungHistorieImportType.ByExcel,
					importUserId = user.Id,
					ImportUsername = user.Name,
				};
				var _details = data.Details.Select(d => d.ToEntity()).ToList();
				transaction.beginTransaction();
				var headerId = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_headerAccess.InsertWithTransaction(_header, transaction.connection, transaction.transaction);
				_details.ForEach(d =>
				{
					d.HeaderId = headerId;
				});
				Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_detailsAccess.InsertWithTransaction(_details, transaction.connection, transaction.transaction);

				if(transaction.commit())
					return ResponseModel<int>.SuccessResponse(headerId);
				else
				{
					transaction.rollback();
					return ResponseModel<int>.FailureResponse("Error in transaction, save failed");
				}
			} catch(Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}