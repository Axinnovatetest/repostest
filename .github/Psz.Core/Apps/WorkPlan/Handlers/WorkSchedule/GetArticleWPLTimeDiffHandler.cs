using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	using Psz.Core.Apps.WorkPlan.Models.WorkSchedule;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticleWPLTimeDiffHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleWPLTimeDiffModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int? _countryId { get; set; }
		private int? _hallId { get; set; }
		private decimal? _minDiff { get; set; }

		public GetArticleWPLTimeDiffHandler(Identity.Models.UserModel user, int? countryId, int? hallId, decimal? minDiff)
		{
			this._user = user;
			_countryId = countryId;
			_hallId = hallId;
			_minDiff = minDiff;
		}

		public ResponseModel<List<ArticleWPLTimeDiffModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var articleEntities = Infrastructure.Data.Access.Joins.MTM.CRP.ArticleAccess.GetArticleWPLTimeDiff(countryId: _countryId, hallId: _hallId, minDiff: _minDiff)
					?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleEntity>();

				return ResponseModel<List<ArticleWPLTimeDiffModel>>.SuccessResponse(articleEntities.Select(x => new ArticleWPLTimeDiffModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ArticleWPLTimeDiffModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<ArticleWPLTimeDiffModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<ArticleWPLTimeDiffModel>>.SuccessResponse();
		}
	}
}
