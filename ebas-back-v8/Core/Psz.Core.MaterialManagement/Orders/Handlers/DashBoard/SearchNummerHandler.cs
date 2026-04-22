using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public partial class SearchNummerHandler: IHandle<SearchFaultyArticleByArticlenummerRequestModel, ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>>>
	{
		private SearchFaultyArticleByArticlenummerRequestModel data { get; set; }
		private Identity.Models.UserModel user { get; set; }

		public SearchNummerHandler(UserModel user, SearchFaultyArticleByArticlenummerRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>> Handle()
		{

			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		private ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>> Perform(UserModel user, SearchFaultyArticleByArticlenummerRequestModel data)
		{
			var articles = Infrastructure.Data.Access.Joins.MTM.Order.ArticlesInOpenFaAccess.getArticles(
										MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId),
										MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryId, data.UnitId)?.Item2,
										MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryId, data.UnitId)?.Item1,
										this.data.nummer
										);

			return ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>>.SuccessResponse(articles.Select(x => new SearchFaultyArticleByArticlenummerResponseModel(x)).ToList());
		}

		public ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>> Validate()
		{
			if(this.user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>>.AccessDeniedResponse();
			}

			if(this.data is null || string.IsNullOrWhiteSpace(this.data.nummer) || this.data.nummer.Trim().Length < 3)
			{
				return ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>>.FailureResponse("");
			}

			return ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>>.SuccessResponse();
		}
	}
}
