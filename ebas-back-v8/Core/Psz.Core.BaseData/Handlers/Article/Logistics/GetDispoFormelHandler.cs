using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class GetDispoFormelHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetDispoFormelHandler(Identity.Models.UserModel user)
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
			var dispoformelEntity = Infrastructure.Data.Access.Tables.INV.DispoformelnAccess.Get();
			if(dispoformelEntity != null && dispoformelEntity.Count > 0)
			{
				var Final_list = dispoformelEntity
							.Select(x => new KeyValuePair<int, string>((int)x.Dispo_ID, x.Beschreibung)).Distinct().OrderBy(y => y.Key).ToList();
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
