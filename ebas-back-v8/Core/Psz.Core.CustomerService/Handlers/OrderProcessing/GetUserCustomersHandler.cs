using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetUserCustomersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CustomerModel>>>
	{
		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetUserCustomersHandler(string data, Identity.Models.UserModel user)
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
				var customers = new List<CustomerModel>();
				List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> myCustomers = new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
				if(_user.Access.Purchase.AllCustomers)
					myCustomers = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get();
				else
				{
					var customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(true, _user.Id)
					.Select(e => e.CustomerNumber)
					.ToList();

					var _notPrimaryCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(false, _user.Id)
						.Where(e => DateTime.Now >= e.ValidFromTime.Date && DateTime.Now <= e.ValidIntoTime.Date.AddDays(1))
						.Select(e => e.CustomerNumber)
						.ToList();

					customersNumbers.AddRange(_notPrimaryCustomersNumbers);

					customersNumbers = customersNumbers.Distinct().ToList();
					myCustomers = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers);
				}
				var _CustomersNumbers = myCustomers
					.Where(e => e.Nummer.HasValue)
					.Select(e => e.Nummer.Value).ToList();
				var notPrimaryCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(false, _user.Id)
					.Where(e => DateTime.Now >= e.ValidFromTime.Date && DateTime.Now <= e.ValidIntoTime.Date.AddDays(1))
					.Select(e => e.CustomerNumber)
					.ToList();
				_CustomersNumbers.AddRange(notPrimaryCustomersNumbers);

				_CustomersNumbers = _CustomersNumbers.Distinct().ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(_CustomersNumbers);
				adressesDb = adressesDb.FindAll(e => e.Adresstyp == 1);
				foreach(var customerDb in myCustomers)
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
				if(customers != null && customers.Count > 0)
					customers = customers.Where(x => x.AdressCustomerNumber.HasValue).ToList();
				if(string.IsNullOrEmpty(_data) || string.IsNullOrWhiteSpace(_data))
					return ResponseModel<List<CustomerModel>>.SuccessResponse(customers);

				var _serchText = _data.Trim().ToLower();
				var final = customers?.Where(x => x.Name.ToLower().Contains(_serchText) || x.AdressCustomerNumber.ToString().Contains(_serchText)).ToList();
				return ResponseModel<List<CustomerModel>>.SuccessResponse(final);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
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
	}
}
