using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Statistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.ManagementOverview.Statistics.Handlers
{
	public class GetArticleHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleHistoryResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetArticleHistoryHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<ArticleHistoryResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				///
				var reasonEntities = Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.GetArticleHistory();

				var responseBody = new List<ArticleHistoryResponseModel> { };
				if(reasonEntities is not null)
				{
					foreach(var item in reasonEntities)
					{
						responseBody.Add(new ArticleHistoryResponseModel
						{
							ChangeTime = item.LastUpdateTime.HasValue ? item.LastUpdateTime.ToString() : string.Empty,
							Log = item.LogDescription,
							ChangeUser = item.LastUpdateUserFullName
						});
					}
				}

				return ResponseModel<List<ArticleHistoryResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}



		public ResponseModel<List<ArticleHistoryResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<ArticleHistoryResponseModel>>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<ArticleHistoryResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<ArticleHistoryResponseModel>>.SuccessResponse();
		}
	}
}
