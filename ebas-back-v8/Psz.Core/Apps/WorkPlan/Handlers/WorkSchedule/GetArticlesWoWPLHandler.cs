using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	using Psz.Core.Apps.WorkPlan.Models.WorkSchedule;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticlesWoWPLHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticlesWoWPLModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ArticlesWoWorkPlanRequestModel _data;

		public GetArticlesWoWPLHandler(Identity.Models.UserModel user, ArticlesWoWorkPlanRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<ArticlesWoWPLModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var articleEntities = Infrastructure.Data.Access.Joins.MTM.CRP.ArticleAccess.GetArticlesWoWPL(_data.warengruppeEF, _data.wStuckliste, _data.wFa, _data.wOpenFa, _data.lager, _data.faDateFrom, _data.faDateTill)
					?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleWpl>();

				return ResponseModel<List<ArticlesWoWPLModel>>.SuccessResponse(articleEntities.Select(x => new ArticlesWoWPLModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ArticlesWoWPLModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<ArticlesWoWPLModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<ArticlesWoWPLModel>>.SuccessResponse();
		}
	}
}
