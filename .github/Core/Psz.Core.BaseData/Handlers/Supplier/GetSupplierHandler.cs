using Psz.Core.BaseData.Models.Supplier;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class GetSupplierHandler: IHandle<Models.Supplier.GetSupplierRequestModel,
		Common.Models.ResponseModel<Models.Supplier.SupplierModel>>
	{
		private Models.Supplier.GetSupplierRequestModel _data { get; set; }

		public GetSupplierHandler(Models.Supplier.GetSupplierRequestModel data)
		{
			this._data = data;
		}

		public ResponseModel<Models.Supplier.SupplierModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var supplierEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_data.SupplierId);

				var addressEntity = supplierEntity.Nummer.HasValue
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierEntity.Nummer.Value)
					: null;

				var contactPersonsEntities = addressEntity != null
					? Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByNummer(addressEntity.Nr)
					: new List<Infrastructure.Data.Entities.Tables.BSD.AnsprechpartnerEntity>();

				// >>>>>>> HEAVY! CHANGE TO SELECT WHERE
				var discountGroupsEntities = Infrastructure.Data.Access.Tables.BSD.RabatthauptgruppenAccess.Get();
				var assignmentConditionsEntities = Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get();
				var industriesEntities = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get();
				var currenciesEntities = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get();
				var slipCirclesSpecificationsEntities = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Get();
				var suppliersGroupsEntities = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get();

				var discountGroupEntity = supplierEntity.Rabattgruppe.HasValue
						? discountGroupsEntities.Find(e => e.Rabatthauptgruppe == supplierEntity.Rabattgruppe)
						: null;

				var assignementConditionsEntity = supplierEntity.Konditionszuordnungs_Nr.HasValue
					? assignmentConditionsEntities.Find(e => e.Nr == supplierEntity.Konditionszuordnungs_Nr.Value)
					: null;

				var industryEntity = !string.IsNullOrEmpty(supplierEntity.Branche)
					? industriesEntities.Find(e => e.Name.Trim() == supplierEntity.Branche.Trim())
					: null;

				var currencyEntity = supplierEntity.Wahrung.HasValue
					? currenciesEntities.Find(e => e.Nr == supplierEntity.Wahrung.Value)
					: null;

				var slipCicleEntity = supplierEntity.Belegkreis.HasValue
					? slipCirclesSpecificationsEntities.Find(e => e.ID == supplierEntity.Belegkreis.Value)
					: null;

				var suppliersGroupEntity = !string.IsNullOrEmpty(supplierEntity.Lieferantengruppe)
					? suppliersGroupsEntities.Find(e => e.Lieferantengruppe.Trim() == supplierEntity.Lieferantengruppe.Trim())
					: null;
				// <<<<<<< HEAVY! CHANGE TO SELECT WHERE

				var lieferantenExtensionEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(supplierEntity.Nr);

				var supplierData = new Models.Supplier.SupplierModel(supplierEntity,
					addressEntity,
					contactPersonsEntities,
					discountGroupEntity,
					assignementConditionsEntity,
					industryEntity,
					currencyEntity,
					slipCicleEntity,
					suppliersGroupEntity,
					lieferantenExtensionEntity);

				return ResponseModel<Models.Supplier.SupplierModel>.SuccessResponse(supplierData);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<SupplierModel> Validate()
		{
			if(this._data.User == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<SupplierModel>.AccessDeniedResponse();
			}

			var supplierEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_data.SupplierId);
			if(supplierEntity == null)
			{
				return new ResponseModel<SupplierModel>()
				{
					Errors = new List<ResponseModel<SupplierModel>.ResponseError>() {
						new ResponseModel<SupplierModel>.ResponseError {Key = "1", Value = "Supplier not found"}
					}
				};
			}

			return ResponseModel<SupplierModel>.SuccessResponse();
		}
	}
}
