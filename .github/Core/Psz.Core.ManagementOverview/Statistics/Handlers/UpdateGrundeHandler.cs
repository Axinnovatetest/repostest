using Infrastructure.Data.Entities.Tables.PRS;
using Infrastructure.Services;
using OfficeOpenXml;
using Psz.Core.BaseData.Enums;
using Psz.Core.BaseData.Handlers;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Statistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.ManagementOverview.Statistics.Handlers
{
	public class UpdateGrundeHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private UpdateGrundeRequestModel _data { get; set; }

		public UpdateGrundeHandler(Identity.Models.UserModel user, UpdateGrundeRequestModel data)
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
					//return validationResponse;
				}
				///

				var grundEntity = Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.GetGrund(_data.Id);
				//ObjectLogEntity item = ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Grund", this._data.IdOldGrunde.HasValue? _data.IdOldGrunde.Value.ToString():string.Empty,
				ObjectLogEntity item = ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Grund", $"{grundEntity.Grund}",
				$"{this._data.IdGrunde}",
				$"{ObjectLogEnums.Objects.Article.GetDescription()} ({grundEntity.Typ}|Lager {grundEntity.LagerId}|{grundEntity.Articlenummer}|{grundEntity.Datum?.ToString("dd.MM.yyyy")})",
				ObjectLogEnums.LogType.Edit);

				int response = Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.UpdateGrunde(_data.Id, _data.IdGrunde, item);


				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}



		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.GetGrund(_data.Id) == null)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: "Item not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
