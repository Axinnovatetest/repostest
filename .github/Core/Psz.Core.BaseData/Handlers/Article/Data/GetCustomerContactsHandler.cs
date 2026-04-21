using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	public class GetCustomerContactsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public int? _data { get; set; }
		public GetCustomerContactsHandler(Identity.Models.UserModel user, int? data)
		{
			this._user = user;
			_data = data;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data ?? -1);
			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(articleEntity?.CustomerNumber ?? -1);
			var contactPersonEbtity = articleEntity is not null && articleEntity.CustomerNumber is not null && articleEntity.CustomerNumber > 0
				? Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(adressenEntity?.Nr ?? -1)
				: Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Get();
			if(contactPersonEbtity != null && contactPersonEbtity.Count > 0)
			{
				var filterd_entity = contactPersonEbtity.Where(item => item.Ansprechpartner != null && item.Ansprechpartner != "").ToList();
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
							filterd_entity.Select(x => new KeyValuePair<int, string>(
									x.Nr,
									x.Ansprechpartner
									)
								).Distinct().ToList()
							);
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
