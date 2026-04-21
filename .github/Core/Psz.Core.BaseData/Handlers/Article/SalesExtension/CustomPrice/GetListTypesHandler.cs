using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetListTypesHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		public GetListTypesHandler(UserModel user)
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

				var salesItemTypeEntites = Enum.GetValues(typeof(Enums.ArticleEnums.CustomPriceType)).Cast<Enums.ArticleEnums.CustomPriceType>().ToList();
				if(salesItemTypeEntites != null && salesItemTypeEntites.Count > 0)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(salesItemTypeEntites
							.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription())).Distinct().ToList());
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
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
	}
}
