using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	using Psz.Core.Apps.WorkPlan.Models.WorkSchedule;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticleFaTimeDiffHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleFaTimeDiffModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int? _data;

		public GetArticleFaTimeDiffHandler(Identity.Models.UserModel user, int? data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<ArticleFaTimeDiffModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var articleEntities = Infrastructure.Data.Access.Joins.MTM.CRP.ArticleFaTimeAccess.GetArticlesFaTimeDiff(this._data)
					?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleFaTimeEntity>();

				return ResponseModel<List<ArticleFaTimeDiffModel>>.SuccessResponse(articleEntities.Select(x => new ArticleFaTimeDiffModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ArticleFaTimeDiffModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<ArticleFaTimeDiffModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<ArticleFaTimeDiffModel>>.SuccessResponse();
		}
	}
}
