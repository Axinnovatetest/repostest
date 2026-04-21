using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class GetListROHHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		public GetListROHHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var articleROHentity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetROH();
			if(articleROHentity != null && articleROHentity.Count > 0)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
						articleROHentity.Select(x => new KeyValuePair<int, string>(
								x.ArtikelNr,
								x.ArtikelNummer
								)
							).Distinct().ToList()
						);
			}
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
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
