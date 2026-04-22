using System.Text;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.HistoryFG;
using Psz.Core.SharedKernel.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Psz.Core.CRP.Handlers.HistoryFG
{
	public class InsertFGXlsIntoDetailsHandler: IHandle<Identity.Models.UserModel, ResponseModel<long>>
	{

		private Core.Identity.Models.UserModel _user;
		private Psz.Core.CRP.Models.HistoryFG.HistoryDataDetailsRequestFGModel _data;

		public InsertFGXlsIntoDetailsHandler(Core.Identity.Models.UserModel user, HistoryDataDetailsRequestFGModel data)
		{
			_user = user;
			this._data = data;
		}


		public ResponseModel<long> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//var validationRes = validateVM();
				//if(validationRes.Count > 0)
				//{
				//	return ResponseModel<long>.FailureResponse(validationRes);
				//}
				// Check if HistoryDetailsFGBestand is empty, null, or contains only zero values
				if(_data.DataFgXls == null || !_data.DataFgXls.Any() )
				{
					return ResponseModel<long>.FailureResponse(key: "1", value: "No valid data to insert");
				}
				
				var HistoryDetailsFGBestand = _data.DataFgXls.Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity
				{

					ArticleNumber = x.ArticleNumber,
					CustomerName = x.CustomerName,
					CustomerNumber = x.CustomerNumber,
					ArticleDesignation1 = x.ArticleDesignation1,
					ArticleDesignation2 = x.ArticleDesignation2,
					ArticleReleaseStatus = x.ArticleReleaseStatus,
					CsContact = x.CsContact,
					WarehouseName = x.WarehouseName,
					StockQuantity = Convert.ToDecimal(x.StockQuantity),
					TotalSalesPrice = Convert.ToDecimal(x.TotalSalesPrice),
					TotalCostsWithCu = Convert.ToDecimal(x.TotalCostsWithCu),
					TotalCostsWithoutCu = Convert.ToDecimal(x.TotalCostsWithoutCu),
					UnitSalesPrice = Convert.ToDecimal(x.UnitSalesPrice),
					UBG = x.UBG,
					EdiStandard = x.EdiStandard,
					HeaderId = x.HeaderId,
				}).ToList();
					
				botransaction.beginTransaction();
				#region Create Script
				var header = new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity
				{
					CreateDate = DateTime.Now,
					CreateUserId = _user.Id,
					CreatedUserName = _user.Username,
					ImportDate = _data.DatumImport,
					ImportType = (int)Enums.CRPEnums.HistoryFGImportTypes.byExcel,
				};


				//var headerId = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryHeaderFGBestandAccess.Insert(header);
				var headerId = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryHeaderFGBestandAccess.InsertWithTransaction(header, botransaction.connection, botransaction.transaction);

				HistoryDetailsFGBestand.ForEach(p =>
				{
					p.HeaderId = headerId;
				});
				var data = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryDetailsFGBestandAccess.InsertWithTransaction(HistoryDetailsFGBestand, botransaction.connection, botransaction.transaction);
				

				#endregion
				if(botransaction.commit())
				{
					return ResponseModel<long>.SuccessResponse(data);
				}
				else
				{
					return ResponseModel<long>.FailureResponse(key: "1", value: "Transaction error");
				}

			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}

			
		public ResponseModel<long> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<long>.AccessDeniedResponse();
			}

			return ResponseModel<long>.SuccessResponse();
		}

	}

}

