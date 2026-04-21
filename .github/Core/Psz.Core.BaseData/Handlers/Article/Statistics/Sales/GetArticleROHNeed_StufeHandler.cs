using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Sales
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticleROHNeed_StufeHandler: IHandle<UserModel, ResponseModel<List<string>>>
	{
		private UserModel _user { get; set; }
		public GetArticleROHNeed_StufeHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<string>> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//-
				return ResponseModel<List<string>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetStufe()
						?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<string>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<string>>.AccessDeniedResponse();
			}


			return ResponseModel<List<string>>.SuccessResponse();
		}
	}
}
