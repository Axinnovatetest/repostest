using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetSuppliersListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<AdressenListModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetSuppliersListHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<AdressenListModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new List<AdressenListModel>();

				var addressesEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetAllSupplierAddresses();
				var lieferantensEntities = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get();
				var wahrungenEntity = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get();

				var query = (from adress in addressesEntities
							 join lieferanten in lieferantensEntities on adress.Nr equals lieferanten.Nummer
							 select adress).Distinct();


				foreach(var adressEntity in query)
				{
					response.Add(new AdressenListModel(adressEntity));
				}
				foreach(var rs in response)
				{
					foreach(var lief in lieferantensEntities)
					{
						if(rs.Nradressen == lief.Nummer)
						{
							rs.Symbol = wahrungenEntity.FirstOrDefault(x => x.Nr == lief.Wahrung)?.Symbol;
							rs.Nrsupplier = lief.Nr;

						}
					}
				}
				return ResponseModel<List<AdressenListModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<AdressenListModel>> Validate()
		{
			if(this._user == null/*
                    || this._user.Access.____*/)
			{
				return ResponseModel<List<AdressenListModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<AdressenListModel>>.SuccessResponse();
		}
	}
}
