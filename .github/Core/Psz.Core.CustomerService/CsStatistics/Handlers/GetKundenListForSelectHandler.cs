using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetKundenListForSelectHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KundenListSelectModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetKundenListForSelectHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KundenListSelectModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<KundenListSelectModel>();

				var AdressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get()?
					.Where(a => a.Kundennummer.HasValue && a.Adresstyp.HasValue && a.Adresstyp.Value == 1).ToList();
				var KundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get();

				var join = from q1 in AdressenEntity
						   join q2 in KundenEntity
									   on q1.Nr equals q2.Nummer
						   select new KundenListSelectModel
						   {
							   Kundenummer = (int)q1.Kundennummer,
							   Name1 = q1.Name1,
							   Ort = q1.Ort,
						   };
				response = join.ToList();

				return ResponseModel<List<KundenListSelectModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KundenListSelectModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KundenListSelectModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KundenListSelectModel>>.SuccessResponse();
		}
	}
}
