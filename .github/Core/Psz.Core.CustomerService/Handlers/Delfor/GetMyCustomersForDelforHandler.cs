using Psz.Core.BaseData;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetMyCustomersForDelforHandler: IHandle<Core.Identity.Models.UserModel, ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>>>
	{
		private readonly UserModel _user;
		private readonly string _data;

		public GetMyCustomersForDelforHandler(Core.Identity.Models.UserModel user, string data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				if(_user.Access.Purchase.AllCustomers)
				{
					return ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>>.SuccessResponse(Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(), _data));

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

				return ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>>.SuccessResponse(Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers), _data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>> Validate()
		{
			if(_user == null)
				return ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>>.AccessDeniedResponse();
			return ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>>.SuccessResponse();
		}

		public static List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel> Get(List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> customersDb, string searchText)
		{
			try
			{
				var customers = new List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>();

				var customersNumbers = customersDb
					.Where(e => e.Nummer.HasValue)
					.Where(e => !e.Edi_Aktiv_Delfor.HasValue || !e.Edi_Aktiv_Delfor.Value)
					.Select(e => e.Nummer.Value).ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNumbers);
				adressesDb = adressesDb.FindAll(e => e.Adresstyp == 1);

				foreach(var customerDb in customersDb)
				{
					var adressDb = adressesDb.Find(e => e.Nr == customerDb.Nummer);
					if(adressDb == null)
					{
						continue;
					}

					customers.Add(new Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel()
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

					if(!searchText.IsNullOrEmptyOrWitheSpaces())
						customers = customers.Where(x => x.Name.ToLower().Contains(searchText) || x.AdressCustomerNumber.ToString().Contains(searchText)).ToList();
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
