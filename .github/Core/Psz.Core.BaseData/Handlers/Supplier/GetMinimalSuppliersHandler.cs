using Psz.Core.BaseData.Models.Supplier;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class GetMinimalSuppliersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Supplier.MinimalSupplierModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetMinimalSuppliersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Supplier.MinimalSupplierModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var adressenEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLiferentenAddresses(); // GetWhereLieferantennummerIsNotNull();
				var lieferantensEntities = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get();

				var response = new List<Models.Supplier.MinimalSupplierModel>();

				foreach(var lieferantEntity in lieferantensEntities)
				{
					var adressEntity = adressenEntities.Find(e => e.Nr == lieferantEntity.Nummer);

					response.Add(new Models.Supplier.MinimalSupplierModel(lieferantEntity, adressEntity));
				}

				return ResponseModel<List<Models.Supplier.MinimalSupplierModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<MinimalSupplierModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<MinimalSupplierModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<MinimalSupplierModel>>.SuccessResponse();
		}
	}
}
