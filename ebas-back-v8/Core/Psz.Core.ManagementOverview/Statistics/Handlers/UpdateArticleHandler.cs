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
	public class UpdateArticleHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private UpdateArticleRequestModel _data { get; set; }

		public UpdateArticleHandler(Identity.Models.UserModel user, UpdateArticleRequestModel data)
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
				///
				ObjectLogEntity item1 = ObjectLogHelper.getLog(this._user, 0, "DEL", "",
					$"{this._data.DelNotice}",
					ObjectLogEnums.Objects.Article.GetDescription(),
					ObjectLogEnums.LogType.Edit);


				ObjectLogEntity item2 = ObjectLogHelper.getLog(this._user, 0, "Kupferbasis", "",
					$"150",
					ObjectLogEnums.Objects.Article.GetDescription(),
					ObjectLogEnums.LogType.Edit);

				string LogDescription = "Update DEL TO " + _data.DelNotice + " And Kupferbasis to 150";


				int response = Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.UpdateArticle(_data.DelNotice,
					item1, item2, LogDescription
				);


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
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
