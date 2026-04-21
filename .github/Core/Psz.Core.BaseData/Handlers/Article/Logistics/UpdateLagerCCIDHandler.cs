using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class UpdateLagerCCIDHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Logistics.UpdateCCIDRequestModel _data { get; set; }
		public UpdateLagerCCIDHandler(UserModel user, Models.Article.Logistics.UpdateCCIDRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				// -
				List<string> _changes = new List<string>();
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var response = -1;
				var lagerStatusEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.Get(this._data.ID);
				if(lagerStatusEntity.CCID != this._data.NewCCID || lagerStatusEntity.CCID_Datum != this._data.NewCCIDDate)
				{
					response = Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateCCID(lagerStatusEntity.ID, this._data.NewCCID, this._data.NewCCIDDate);
				}

				// - save logs 
				if(lagerStatusEntity.CCID != this._data.NewCCID)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					ObjectLogHelper.getLog(this._user, this._data.ArticleId, $"CCID (Lager {this._data.LagerName})", $"{lagerStatusEntity.CCID}",
						$"{this._data.NewCCID}",
						$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
						Enums.ObjectLogEnums.LogType.Edit));
				}
				if(lagerStatusEntity.CCID_Datum != this._data.NewCCIDDate)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.ArticleId, $"CCID Date (Lager {this._data.LagerName})", $"{lagerStatusEntity.CCID_Datum}",
							$"{this._data.NewCCIDDate}",
							$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
							Enums.ObjectLogEnums.LogType.Edit));
				}

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.ArticleId);

				// -
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access?.MasterData?.EditLagerCCID == true)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<int>.FailureResponse("Article not found");

			if(Infrastructure.Data.Access.Tables.PRS.LagerAccess.Get(this._data.ID) == null)
				return ResponseModel<int>.FailureResponse("Lager not found");

			if(this._data.NewCCID == true && !this._data.NewCCIDDate.HasValue)
				return ResponseModel<int>.FailureResponse("CCID Date invalid");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
