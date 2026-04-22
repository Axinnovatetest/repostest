using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logs
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetFullHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.ObjectLog.ObjectLogModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetFullHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.ObjectLog.ObjectLogModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<Models.ObjectLog.ObjectLogModel> results = null;
				var objectNames = new List<string> { };
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_Class.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_Packaging.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_ProductGroup.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_ProjectType.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_ManagerUsers.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_ExternalStatus.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_InternalStatus.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_CheckStatus.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.ArticleConfig_MHDTag.GetDescription());
				objectNames.Add(Enums.ObjectLogEnums.Objects.Technician.GetDescription());

				var logEntities = Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.GetByObjetsAndId(objectNames);

				if(logEntities != null && logEntities.Count > 0)
				{
					results = new List<Models.ObjectLog.ObjectLogModel>();
					foreach(var logEntity in logEntities)
					{
						results.Add(new Models.ObjectLog.ObjectLogModel(logEntity));
					}
				}

				return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.ObjectLog.ObjectLogModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.SuccessResponse();
		}
	}
}
