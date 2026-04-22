using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Company
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<long>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Settings.Models.Company.GetModel _data { get; set; }
		public AddHandler(Identity.Models.UserModel user, Settings.Models.Company.GetModel data)
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

				var companiesDirected = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { (int)this._data.DirectorId });
				var companyEntity = this._data.ToEntity();
				companyEntity.IsActive = true;
				companyEntity.CreateTime = DateTime.Now;
				companyEntity.CreateUserId = this._user.Id;

				var insertedId = Infrastructure.Data.Access.Tables.STG.CompanyAccess.InsertWoLogo(companyEntity);

				// - update Director's company if he's not already running another
				if(companiesDirected == null || companiesDirected.Count <= 0)
				{
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int)this._data.DirectorId);
					userEntity.CompanyId = insertedId;
					userEntity.LastEditDate = DateTime.Now;
					userEntity.LastEditUserId = this._user.Id;
					Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userEntity);
				}

				return ResponseModel<long>.SuccessResponse(insertedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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

			if(string.IsNullOrWhiteSpace(this._data.Name) || string.IsNullOrEmpty(this._data.Name))
				return ResponseModel<long>.FailureResponse($"Invalid value [{this._data.Name}] for company name");

			if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByName(this._data.Name) != null)
				return ResponseModel<long>.FailureResponse($"Company already exists");

			if(this._data.DirectorId.HasValue && this._data.DirectorId.Value > 0 && Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.DirectorId.Value) == null)
				return ResponseModel<long>.FailureResponse($"User not found");

			if(this._data.DirectorId.HasValue && this._data.DirectorId.Value > 0 && Psz.Core.FinanceControl.Helpers.Processings.Budget.User.HasDifferentAllocation(this._data.DirectorId ?? -1, null, null))
				return ResponseModel<long>.FailureResponse($"User has budget allocation, please remove it before changing company or department.");

			return ResponseModel<long>.SuccessResponse();
		}
	}

}
