using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Technic
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetTechniciansForAddHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public GetTechniciansForAddHandler(UserModel user, string data)
		{
			this._user = user;
			this._data = data;
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

				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.GetActive(this._data);
				if(userEntities != null && userEntities.Count > 0)
				{
					var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Technic.GetTechnicians();
					if(statisticsEntities != null && statisticsEntities.Count > 0)
					{
						userEntities = userEntities.Where(x => statisticsEntities.Exists(y => y.Name?.ToLower() == x.Name?.ToLower()) == false)?.ToList();
					}

					// -
					return ResponseModel<List<Models.Article.Statistics.Technic.TechnicianResponseModel>>.SuccessResponse(userEntities
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
