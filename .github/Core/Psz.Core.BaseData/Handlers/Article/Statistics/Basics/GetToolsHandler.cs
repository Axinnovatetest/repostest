using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetToolsHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Basics.ToolsResponseModel>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public GetToolsHandler(UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Basics.ToolsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetTools(this._data);

				return ResponseModel<Models.Article.Statistics.Basics.ToolsResponseModel>.SuccessResponse(
					new Models.Article.Statistics.Basics.ToolsResponseModel
					{
						TN = results?.Where(x => x.Key == "tn").Select(x => x.Value)?.OrderBy(x => x).ToList(),
						AL = results?.Where(x => x.Key == "al").Select(x => x.Value)?.OrderBy(x => x).ToList(),
						CZ = results?.Where(x => x.Key == "cz").Select(x => x.Value)?.OrderBy(x => x).ToList(),
					}
					);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Basics.ToolsResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Basics.ToolsResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data) == null)
				return ResponseModel<Models.Article.Statistics.Basics.ToolsResponseModel>.FailureResponse("Article not found");

			return ResponseModel<Models.Article.Statistics.Basics.ToolsResponseModel>.SuccessResponse();
		}
	}
}
