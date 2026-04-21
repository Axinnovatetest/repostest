using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class CreateSuperbillHandler: IHandle<UserModel, ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>>
	{
		private UserModel _user { get; set; }
		private List<Models.Article.Statistics.ControllingAnalysis.SuperbillCreateRequestModel> _data { get; set; }
		public CreateSuperbillHandler(UserModel user, List<Models.Article.Statistics.ControllingAnalysis.SuperbillCreateRequestModel> data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				// -
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					this._data.Select(x =>
						ObjectLogHelper.getLog(this._user, 0, "SuperbillROH", "",
						$"ArticleNumber: {x.ArticleNumber} | Quantity: {x.Quantity}",
						Enums.ObjectLogEnums.Objects.SuperbillROH.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add)
						)?.ToList(), botransaction.connection, botransaction.transaction);

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetSuperbillROH_MultiQuery(
						this._data.Select(x => new Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROHInput
						{
							Artikelnummer = x.ArticleNumber,
							Menge = x.Quantity
						})?.ToList(),
						botransaction.connection, botransaction.transaction, isCreate: true);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.SuccessResponse(results);
				}
				else
				{
					return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.AccessDeniedResponse();
			}

			return ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>.SuccessResponse();
		}
	}
}
