using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetMyCustomersForCreateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CustomerModel>>>
	{
		private bool? _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetMyCustomersForCreateHandler(bool? data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<CustomerModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				//  - 2025-11-28 - PS - remove archived customers
				var archivedCustomerIds = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetArchivedIds();
				if(_user.Access.Purchase.AllCustomers)
				{
					return ResponseModel<List<CustomerModel>>.SuccessResponse(Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetForCreate()?.Where(x=> archivedCustomerIds?.Contains(x.Nr) == false)?.ToList(), _data));
				}

				var customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(true, _user.Id)
					?.Select(e => e.CustomerNumber)
					?.ToList() ?? new List<int>();

				var notPrimaryCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(false, _user.Id)
					?.Where(e => DateTime.Now >= e.ValidFromTime.Date && DateTime.Now <= e.ValidIntoTime.Date.AddDays(1))
					?.Select(e => e.CustomerNumber)
					?.ToList() ?? new List<int>();

				// - 2022-05-17 - Sabine - open Customers to CS
				if(_user.Access.CustomerService.ModuleActivated &&
					(_user.Access.CustomerService.ConfirmationCreate ||
					_user.Access.CustomerService.DeliveryNoteCreate ||
					_user.Access.CustomerService.FaCreate ||
					_user.Access.CustomerService.ConfirmationEdit ||
					_user.Access.CustomerService.DeliveryNoteEdit ||
					_user.Access.CustomerService.FaEdit
					))
				{
					var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummers(Module.CTS.OpenCsCustomers?.Select(x => x.Number)?.ToList() ?? new List<int>());
					customersNumbers.AddRange(addressEntities?.Select(x => x.Nr) ?? new List<int>());
				}

				customersNumbers.AddRange(notPrimaryCustomersNumbers);
				customersNumbers = customersNumbers.Distinct().ToList();

				return ResponseModel<List<CustomerModel>>.SuccessResponse(Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummersForCreate(customersNumbers)?.Where(x => archivedCustomerIds?.Contains(x.Nr) == false)?.ToList(), _data));


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<CustomerModel>> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<List<CustomerModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<CustomerModel>>.SuccessResponse();
		}
		public static List<CustomerModel> Get(List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> customersDb, bool? isEDIActive = null)
		{
			try
			{
				var customers = new List<CustomerModel>();

				var customersNumbers = customersDb
					.Where(e => e.Nummer.HasValue)
					.Select(e => e.Nummer.Value).ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetForCreate(customersNumbers);
				if(isEDIActive.HasValue)
				{
					adressesDb = adressesDb.FindAll(e => e.Adresstyp == 1
						&& (e.EDI_Aktiv.HasValue ? e.EDI_Aktiv == isEDIActive.Value : false));
				}
				else
				{
					adressesDb = adressesDb.FindAll(e => e.Adresstyp == 1);
				}


				foreach(var customerDb in customersDb)
				{
					var adressDb = adressesDb.Find(e => e.Nr == customerDb.Nummer);
					if(adressDb == null)
					{
						continue;
					}

					customers.Add(new CustomerModel()
					{
						Id = customerDb.Nr,
						Type = adressDb?.Anrede,
						CustomerNumber = customerDb.Nummer ?? -1,
						AdressCustomerNumber = adressDb?.Kundennummer,

						Name = adressDb?.Name1,
						Name2 = adressDb?.Name2,
						Name3 = adressDb?.Name3,

						Contact = adressDb.Briefanrede,
						Department = customerDb.RG_Abteilung,
						CountryPostcode = customerDb.RG_Land_PLZ_ORT,
						StreetPOBox = customerDb.RG_Strasse_Postfach,
						Duns = adressDb?.Duns,

						Country = adressDb?.Land,
						Email = adressDb?.EMail,
						Fax = adressDb?.Fax,
						Phone = adressDb?.Telefon,
						POBox = adressDb?.Postfach,
						Street = adressDb?.StraBe
					});
				}

				return customers;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
