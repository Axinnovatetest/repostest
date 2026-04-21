using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Technic
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetTechniciansHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>>>
	{
		private UserModel _user { get; set; }
		public GetTechniciansHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Technic.GetTechnicians();
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Technic.TechnicianResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>>.SuccessResponse();
		}
	}
}
