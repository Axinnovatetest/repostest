using Psz.Core.SharedKernel.Interfaces;
using Psz.Core.Common.Models;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.HourlyRate
{
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.HourlyRate.HourlyRateRequestModel _data { get; set; }
		public AddHandler(Identity.Models.UserModel user, Models.Settings.HourlyRate.HourlyRateRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
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
				var entity = this._data.ToEntity();
				entity.CreationTime = DateTime.Now;
				entity.CreationUserId = this._user.Id;
				entity.CreationUserName = this._user.Username;
				var siteEntity = Psz.Core.BaseData.Handlers.Article.Configuration.Production.GetListProductionPlaceHandler.getProductionPlaces().Where(x => x.Key == (this._data.ProductionSiteId ?? -1))?.ToList();
				if(siteEntity is not null && siteEntity.Count > 0)
				{
					entity.ProductionSiteName = siteEntity[0].Value;
				}
				var responseBody = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.InsertWithTransaction(entity, botransaction.connection, botransaction.transaction);

				// -
				var log = ObjectLogHelper.getLog(this._user, responseBody,
						Enums.ObjectLogEnums.Objects.ArticleConfig_HourlyRates.GetDescription(),
						null,
						$"{entity.ProductionSiteName} | {entity.HourlyRate}", Enums.ObjectLogEnums.Objects.ArticleConfig_HourlyRates.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(responseBody);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(!this._data.HourlyRate.HasValue)
			{
				return ResponseModel<int>.FailureResponse($"[Hourly Rate] invalid value [{_data.HourlyRate}].");
			}
			if(!this._data.ProductionSiteId.HasValue || this._data.ProductionSiteId <= 0)
			{
				return ResponseModel<int>.FailureResponse($"[Production Site] invalid data [{_data.ProductionSiteName}].");
			}
			if(Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Get(this._data.ProductionSiteId ?? -1) is null)
			{
				return ResponseModel<int>.FailureResponse($"[Hourly Rate] site [{_data.ProductionSiteName}] not found.");
			}

			// - 
			var sameVersion = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.GetByProdutionSiteId(this._data.ProductionSiteId ?? -1);
			if(sameVersion?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Hourly Rate for Site [{sameVersion[0].ProductionSiteName}] already exists.");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
