using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class GetLagerListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetLagerListHandler(Identity.Models.UserModel user)
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
			var lagerortEntiy = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get();
			if(lagerortEntiy != null && lagerortEntiy.Count > 0)
			{
				var Final_list = lagerortEntiy
							.Select(x => new KeyValuePair<int, string>(x.LagerortId, x.Lagerort)).Distinct().OrderBy(y => y.Key).ToList();
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(Final_list);
			}
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}
			//***
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
