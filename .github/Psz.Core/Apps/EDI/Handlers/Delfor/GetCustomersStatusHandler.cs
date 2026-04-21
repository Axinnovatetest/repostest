using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetCustomersStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Delfor.GetCustomerModel>>>
	{
		private bool? _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomersStatusHandler(Identity.Models.UserModel user, bool? data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Delfor.GetCustomerModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<Models.Delfor.GetCustomerModel>();
				var finalCustomersList = new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
				var duns = new List<string>();
				var customersNummers = _user.IsGlobalDirector || _user.SuperAdministrator || _user.Access.Purchase.AllCustomers ?
					 Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get()?.Select(c => c.Nummer ?? -1).OrderBy(y => y).ToList()
					 : Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(_user.Id)?.Select(c => c.CustomerNumber).OrderBy(y => y).ToList();

				var customersAdresses = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNummers)?
					.Where(c => c.EDI_Aktiv.HasValue && c.EDI_Aktiv.Value).ToList();


				var customersWithDuns = customersAdresses.Where(c => !string.IsNullOrEmpty(c.Duns) && !string.IsNullOrWhiteSpace(c.Duns)).ToList();
				finalCustomersList.AddRange(customersWithDuns);
				duns.AddRange(customersWithDuns?.Select(x => x.Duns).ToList());


				var customersWithoutDuns = customersAdresses.Where(c => string.IsNullOrEmpty(c.Duns) || string.IsNullOrWhiteSpace(c.Duns)).ToList();
				var adressenExtensions = Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByAddressNr(customersWithoutDuns?.Select(x => x.Nr).ToList())?
					.Where(y => !string.IsNullOrEmpty(y.Duns) && !string.IsNullOrWhiteSpace(y.Duns)).ToList();

				var customersDunsFromExtensions = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(adressenExtensions?.Select(x => x.AdressenNr).ToList());
				finalCustomersList.AddRange(customersDunsFromExtensions);
				duns.AddRange(customersDunsFromExtensions?.Select(x => x.Duns).ToList());
				duns = duns?.Distinct()?.ToList();


				var documentEntities = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetLastByBuyerDUNS(duns) ?? new List<Infrastructure.Data.Entities.Tables.CTS.HeaderEntity>();
				var errorEntites = Infrastructure.Data.Access.Tables.CTS.ErrorAccess.GetByBuyerDUNS(duns, _data);

				foreach(var customerEntity in finalCustomersList)
				{
					var customerDocs = documentEntities?.Where(x => x.BuyerDUNS.Trim() == customerEntity?.Duns?.Trim()).ToList();
					var customerErrs = errorEntites?.Where(x => x.BuyerDuns.Trim() == customerEntity?.Duns?.Trim()).ToList();
					var customerDUNS = !string.IsNullOrEmpty(customerEntity.Duns) && !string.IsNullOrWhiteSpace(customerEntity.Duns)
						? customerEntity.Duns
						: Infrastructure.Data.Access.Tables.PRS.AdressenExtensionAccess.GetByAddressNr(customerEntity.Nr)?.Duns;
					var errorCount = customerErrs?.Where(e => !e.Validated.HasValue || !e.Validated.Value)?.ToList().Count() ?? 0;
					var validatedErrorCount = customerErrs?.Where(e => e.Validated.HasValue && e.Validated.Value)?.ToList().Count() ?? 0;
					response.Add(new Models.Delfor.GetCustomerModel(customerEntity, customerDocs?.Count() ?? 0, errorCount, validatedErrorCount, customerDUNS));
				}

				// - 2023-03-08 - add Without Customer data
				var errs = Infrastructure.Data.Access.Tables.CTS.ErrorAccess.GetWoBuyerDUNS(null);
				var errsCount = errs?.Where(e => !e.Validated.HasValue || !e.Validated.Value)?.ToList().Count() ?? 0;
				response.Add(new Models.Delfor.GetCustomerModel
				{
					CustomerDUNS = "",
					CustomerId = -1,
					CustomerName = "",
					CustomerNumber = "",
					ErrorDocumentsCount = errsCount,
					UnvalidatedDocumentsCount = 0,
					ValidatedErrorDocumentCount = (errs?.Count() ?? 0) - errsCount
				});

				// - 
				return ResponseModel<List<Models.Delfor.GetCustomerModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Delfor.GetCustomerModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Delfor.GetCustomerModel>>.AccessDeniedResponse();
			}
			var userCustomerEntities = this._user.IsGlobalDirector || this._user.SuperAdministrator || (this._user.Access?.CustomerService.EDI ?? false)
					? Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.Get()
					: Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(this._user.Id);
			if(userCustomerEntities == null || userCustomerEntities.Count <= 0)
				return ResponseModel<List<Models.Delfor.GetCustomerModel>>.FailureResponse("No customers found for user.");

			return ResponseModel<List<Models.Delfor.GetCustomerModel>>.SuccessResponse();
		}
	}
}