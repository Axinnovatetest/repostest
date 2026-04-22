using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetListProductionPlaceHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		public GetListProductionPlaceHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(getProductionPlaces()?
						.Select(x => new KeyValuePair<int, string>((int)x.Key, x.Value)).Distinct().ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}


			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
		public static List<KeyValuePair<int, string>> getProductionPlaces()
		{
			var productionPlaceEntites = Enum.GetValues(typeof(Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)).Cast<Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace>().ToList();
			return productionPlaceEntites
					?.Select(x => new KeyValuePair<int, string>((int)x, $"{x.GetDescription()}".Trim()))
					?.Distinct()?.ToList();
		}
	}
}
