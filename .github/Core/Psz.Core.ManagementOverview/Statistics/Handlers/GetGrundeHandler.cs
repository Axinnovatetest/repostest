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
	public class GetGrundeHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<GrundeResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetGrundeHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<GrundeResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					//return validationResponse;
				}
				///
				var reasonEntities = Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.GetGrunde();

				var responseBody = new List<GrundeResponseModel> { };
				if(reasonEntities is not null)
				{
					foreach(var item in reasonEntities)
					{
						responseBody.Add(new GrundeResponseModel
						{
							Id = item.Id,
							Name = item.Name?.Trim()
						});
					}
				}

				return ResponseModel<List<GrundeResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}



		public ResponseModel<List<GrundeResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<GrundeResponseModel>>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<GrundeResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<GrundeResponseModel>>.SuccessResponse();
		}
	}
}
