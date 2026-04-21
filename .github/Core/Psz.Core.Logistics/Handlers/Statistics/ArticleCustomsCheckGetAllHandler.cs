using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class ArticleCustomsCheckGetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Statistics.ArticleCustomsCheckResponseModels>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public ArticleCustomsCheckGetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Statistics.ArticleCustomsCheckResponseModels>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<List<Models.Statistics.ArticleCustomsCheckResponseModels>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.LGT.ArticleCustomsNumberCheckAccess.Get()
					?.Select(x => new Models.Statistics.ArticleCustomsCheckResponseModels(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Statistics.ArticleCustomsCheckResponseModels>> Validate()
		{
			if(this._user == null || (this._user.Access?.Logistics?.StatiticsArticleCustomsNumber != true && this._user.IsGlobalDirector != true && this._user.SuperAdministrator != true))
			{
				return ResponseModel<List<Models.Statistics.ArticleCustomsCheckResponseModels>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Statistics.ArticleCustomsCheckResponseModels>>.SuccessResponse();
		}
	}
}
