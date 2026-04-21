using Psz.Core.SharedKernel.Interfaces;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.HourlyRate
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.HourlyRate.HourlyRateRequestModel _data { get; set; }

		public EditHandler(Identity.Models.UserModel user, Models.Settings.HourlyRate.HourlyRateRequestModel data)
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

				//TODO: - insert process here
				botransaction.beginTransaction();

				var entity = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.Get(this._data.Id);
				var data = this._data.ToEntity();
				data.LastEditUserId = this._user.Id;
				data.LastEditUserName = this._user.Username;
				if(this._data.ProductionSiteId != entity.ProductionSiteId)
				{
					var siteEntity = Psz.Core.BaseData.Handlers.Article.Configuration.Production.GetListProductionPlaceHandler.getProductionPlaces().Where(x => x.Key == (this._data.ProductionSiteId ?? -1))?.ToList();
					if(siteEntity is not null && siteEntity.Count > 0)
					{
						data.ProductionSiteName = siteEntity[0].Value;
					}
				}
				var responseBody = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.UpdateWithTransaction(data, botransaction.connection, botransaction.transaction);

				// -
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				if(entity.HourlyRate != this._data.HourlyRate)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"HourlyRate: Value",
						  $"{entity.HourlyRate}",
						  $"{this._data.HourlyRate}", Enums.ObjectLogEnums.Objects.ArticleConfig_HourlyRates.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.ProductionSiteId != this._data.ProductionSiteId)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"HourlyRate: Site",
						  $"{entity.ProductionSiteName}",
						  $"{this._data.ProductionSiteName}", Enums.ObjectLogEnums.Objects.ArticleConfig_HourlyRates.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}

				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

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

			if(!this._data.HourlyRate.HasValue || this._data.HourlyRate <= 0)
			{
				return ResponseModel<int>.FailureResponse($"[Hourly Rate] invalid value [{_data.HourlyRate}]");
			}
			var siteEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Get(this._data.ProductionSiteId ?? -1);
			if(siteEntity is null)
			{
				return ResponseModel<int>.FailureResponse($"[Hourly Rate] site [{_data.ProductionSiteName}] not found.");
			}

			var entity = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.Get(this._data.Id);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			// - changing site
			if(entity.ProductionSiteId != this._data.ProductionSiteId)
			{
				var sameVersion = Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.GetByProdutionSiteId(this._data.ProductionSiteId ?? -1)
					?.Where(x => x.Id != this._data.Id)?.ToList();
				if(sameVersion != null && sameVersion.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Hourly rate already exists for Site [{this._data.ProductionSiteName}]");
				}
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
