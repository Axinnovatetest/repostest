using Psz.Core.BaseData.Models.Supplier;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class GetSuppliersListHandler: IHandle<Identity.Models.UserModel, Common.Models.ResponseModel<List<Models.Supplier.AdressenListModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private bool? _data { get; set; }
		public GetSuppliersListHandler(Identity.Models.UserModel user, bool? includeLieferAddress = null)
		{
			this._user = user;
			this._data = includeLieferAddress;
		}
		public ResponseModel<List<Models.Supplier.AdressenListModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new List<Models.Supplier.AdressenListModel>();

				var addressesEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetAllSupplierAddresses(this._data);
				var lieferantensEntities = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get();
				var wahrungenEntity = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get();

				var query = (from adress in addressesEntities
							 join lieferanten in lieferantensEntities on adress.Nr equals lieferanten.Nummer
							 select adress).Distinct();


				foreach(var adressEntity in query)
				{
					response.Add(new Models.Supplier.AdressenListModel(adressEntity));
				}
				foreach(var rs in response)
				{
					foreach(var lief in lieferantensEntities)
					{
						if(rs.Nr == lief.Nummer)
						{
							rs.Symbol = wahrungenEntity.FirstOrDefault(x => x.Nr == lief.Wahrung)?.Symbol;
						}
					}
				}
				return ResponseModel<List<Models.Supplier.AdressenListModel>>.SuccessResponse(response);
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
