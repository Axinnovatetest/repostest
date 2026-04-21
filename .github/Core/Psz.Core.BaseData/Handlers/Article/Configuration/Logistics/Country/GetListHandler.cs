using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Logistics.Country
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetListHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private UserModel _user { get; set; }
		public GetListHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var countryEntites = Infrastructure.Data.Access.Tables.BSD.Artikelstammdaten_Ursprungsland_VorgabenAccess.Get();
				if(countryEntites != null && countryEntites.Count > 0)
				{
					return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(countryEntites
							.Select(x => new KeyValuePair<string, string>(x.Land, $"{x.Land} | {x.Hinweis}".Trim())).Distinct().ToList());
				}

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}


			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}
