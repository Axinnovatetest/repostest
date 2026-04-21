using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Quality.MHDTag
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetListHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		public GetListHandler(UserModel user)
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

				var tagEntites = Infrastructure.Data.Access.Tables.BSD.ArtikelMHD_TagAccess.Get();
				if(tagEntites != null && tagEntites.Count > 0)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(tagEntites
							.Select(x => new KeyValuePair<int, string>(x.Id, $"{x.Tag}".Trim())).Distinct().ToList());
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
