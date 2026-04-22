using Psz.Core.FinanceControl.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Supplier
{
	public class GetSuppliersHandler: IHandle<Identity.Models.UserModel,
		Common.Models.ResponseModel<List<Models.Supplier.SupplierModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSuppliersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Supplier.SupplierModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var suppliersEntities = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get();
				var suppliersIds = suppliersEntities.Select(e => e.Nr).ToList();
				var addressesEntities = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.GetWhereLieferantennummerIsNotNull();
				var addressesIds = addressesEntities.Select(e => e.Nr).ToList();
				var contactPersonsEntities = Infrastructure.Data.Access.Tables.FNC.AnsprechpartnerAccess.GetByNummers(addressesIds);
				var discountGroupsEntities = Infrastructure.Data.Access.Tables.FNC.RabatthauptgruppenAccess.Get();
				var assignmentConditionsEntities = Infrastructure.Data.Access.Tables.FNC.KonditionsZuordnungstabelleEntity.Get();
				var industriesEntities = Infrastructure.Data.Access.Tables.FNC.IndustryAccess.Get();
				var currenciesEntities = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get();
				var slipCirclesSpecificationsEntities = Infrastructure.Data.Access.Tables.FNC.BelegkreiseVorgabenAccess.Get();
				var suppliersGroupsEntities = Infrastructure.Data.Access.Tables.FNC.PszLieferantengruppenAccess.Get();

				var response = new List<Models.Supplier.SupplierModel>();

				foreach(var supplierEntity in suppliersEntities)
				{
					var addressEntity = addressesEntities.Find(e => e.Nr == supplierEntity.Nummer);
					if(addressEntity == null)
					{
						continue;
					}

					var supplierContactPersonsEntities = contactPersonsEntities.FindAll(e => e.Nummer == addressEntity.Nr);

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

					var supplier = new Models.Supplier.SupplierModel(supplierEntity,
						addressEntity,
						supplierContactPersonsEntities,
						discountGroupEntity,
						assignementConditionsEntity,
						industryEntity,
						currencyEntity,
						slipCicleEntity,
						suppliersGroupEntity);

					response.Add(supplier);
				}

				return ResponseModel<List<Models.Supplier.SupplierModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<SupplierModel>> Validate()
		{
			if(this._user == null/*
                    || this._user.Access.____*/)
			{
				return ResponseModel<List<SupplierModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<SupplierModel>>.SuccessResponse();
		}
	}
}
