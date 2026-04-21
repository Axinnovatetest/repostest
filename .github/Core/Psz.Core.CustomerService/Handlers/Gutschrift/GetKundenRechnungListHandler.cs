using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class GetKundenRechnungListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{

		private int _kundenNr { get; set; }
		private string _searchText { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetKundenRechnungListHandler(Identity.Models.UserModel user, int kundennr, string searchtext)
		{
			this._user = user;
			this._kundenNr = kundennr;
			this._searchText = searchtext;
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

				var response = new List<KeyValuePair<int, string>>();
				var rechnungEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTypsAndKundenNrs(new List<string> { Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Invoice) },
					new List<int> { _kundenNr }, new Infrastructure.Data.Access.Settings.PaginModel { FirstRowNumber = 0, RequestRows = 100 }, _searchText);
				if(rechnungEntities != null && rechnungEntities.Count > 0)
					response = rechnungEntities?.Select(r => new KeyValuePair<int, string>(r.Nr, $"{r.Angebot_Nr}||{r.Projekt_Nr}||{r.Bezug}"))?.ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
