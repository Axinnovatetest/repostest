using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Company
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<long>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Settings.Models.Company.GetModel _data { get; set; }
		public UpdateHandler(Identity.Models.UserModel user, Settings.Models.Company.GetModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<long> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var oldCompany = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.Id);
				var companyEntity = this._data.ToEntity();
				companyEntity.Name = this._data.Name;
				companyEntity.Id = this._data.Id;

				companyEntity.LastUpdateTime = DateTime.Now;
				companyEntity.LastUpdateUserId = this._user.Id;

				if(companyEntity.DirectorId != oldCompany.DirectorId)
				{
					// - update Director's company if he's not already running another
					var companiesDirected = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { (int)companyEntity.DirectorId });
					if(companiesDirected == null || companiesDirected.Count <= 0)
					{
						var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int)this._data.DirectorId);
						userEntity.CompanyId = companyEntity.Id;
						userEntity.LastEditDate = DateTime.Now;
						userEntity.LastEditUserId = this._user.Id;

						Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userEntity);
					}
				}

				return ResponseModel<long>.SuccessResponse(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Update(companyEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);

				Infrastructure.Services.Logging.Logger.Log(e, e.StackTrace);
				throw;
			}
		}
		public ResponseModel<long> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<long>.AccessDeniedResponse();
			}

			var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.Id);
			if(companyEntity == null)
				return ResponseModel<long>.FailureResponse($"Company not found");

			if(this._data.DirectorId.HasValue && this._data.DirectorId.Value > 0 && Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.DirectorId.Value) == null)
				return ResponseModel<long>.FailureResponse($"User not found");

			if(string.IsNullOrWhiteSpace(this._data.Name) || string.IsNullOrEmpty(this._data.Name))
				return ResponseModel<long>.FailureResponse($"Invalid value [{this._data.Name}] for company name");

			var comp = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByName(this._data.Name);
			if(comp != null && comp.Id != this._data.Id)
				return ResponseModel<long>.FailureResponse(new List<string> { $"Company [{this._data.Name}] already exists" });

			// - 2025-07-16 - allow change of director or logo w/o removing current year budget
			if(hasBudgetRequiredChanged(comp))
			{
				var companyAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(this._data.Id, DateTime.Today.Year);
				if(companyAllocation != null)
					return ResponseModel<long>.FailureResponse($"Company has budget allocation, please remove it before proceeding.");

				var companySupplements = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { this._data.Id }, DateTime.Today.Year);
				if(companySupplements != null && companySupplements.Count > 0)
					return ResponseModel<long>.FailureResponse($"Company has budget allocation, please remove it before proceeding.");
			}

			if(this._data.DirectorId.HasValue && this._data.DirectorId.Value > 0 && Psz.Core.FinanceControl.Helpers.Processings.Budget.User.HasDifferentAllocation(this._data.DirectorId ?? -1, null, this._data.Id))
				return ResponseModel<long>.FailureResponse($"User has budget allocation, please remove it before proceeding.");

			return ResponseModel<long>.SuccessResponse();
		}
		bool hasBudgetRequiredChanged(Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity)
		{
			return !this._data.Address.IsSameAs(companyEntity.Address, false, true)
				|| !this._data.Address2.IsSameAs(companyEntity.Address2, false, true)
				|| !this._data.City.IsSameAs(companyEntity.City, false, true)
				|| !this._data.Email.IsSameAs(companyEntity.Email, false, true)
				|| !this._data.Type.IsSameAs(companyEntity.Type, false, true)
				|| !this._data.Country.IsSameAs(companyEntity.Country, false, true)
				|| !this._data.LagalName.IsSameAs(companyEntity.LagalName, false, true)
				|| !this._data.Name.IsSameAs(companyEntity.Name, false, true)
				|| !this._data.PostalCode.IsSameAs(companyEntity.PostalCode, false, true)
				|| !this._data.Telephone.IsSameAs(companyEntity.Telephone, false, true)
				|| !this._data.Telephone2.IsSameAs(companyEntity.Telephone2, false, true)
				|| this._data.IsActive != companyEntity.IsActive;
		}
	}

}
