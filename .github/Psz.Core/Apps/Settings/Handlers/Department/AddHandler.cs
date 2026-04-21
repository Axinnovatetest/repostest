using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Department
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<long>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Settings.Models.Department.UpdateModel _data { get; set; }
		public AddHandler(Identity.Models.UserModel user, Settings.Models.Department.UpdateModel data)
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

				var departmentEntity = this._data.ToEntity(null);
				departmentEntity.CreationTime = DateTime.Now;
				departmentEntity.CreationUserId = this._user.Id;

				var insertedId = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Insert(departmentEntity);
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int)this._data.HeadUserId);
				userEntity.LastEditDate = DateTime.Now;
				userEntity.LastEditUserId = this._user.Id;
				userEntity.CompanyId = (int)departmentEntity.CompanyId;
				userEntity.DepartmentId = (int)insertedId;

				return ResponseModel<long>.SuccessResponse(Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userEntity));
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


			if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId) == null)
				return ResponseModel<long>.FailureResponse($"Company not found");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int)this._data.HeadUserId) == null)
				return ResponseModel<long>.FailureResponse($"User not found");

			if(string.IsNullOrWhiteSpace(this._data.Name) || string.IsNullOrEmpty(this._data.Name))
				return ResponseModel<long>.FailureResponse($"Invalid value [{this._data.Name}] for department name");

			var departments = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByName(this._data.Name) ?? new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
			foreach(var dept in departments)
			{
				if(dept != null && dept.Name == this._data.Name && dept.CompanyId == this._data.CompanyId)
					return ResponseModel<long>.FailureResponse($"Department [{this._data.Name} | {this._data.CompanyName}] already exists");
			}

			if(this._data.HeadUserId > 0 && Psz.Core.FinanceControl.Helpers.Processings.Budget.User.HasDifferentAllocation((int)this._data.HeadUserId, (int)this._data.Id, (int)this._data.CompanyId))
				return ResponseModel<long>.FailureResponse($"User has budget allocation, please remove it before changing company or department.");

			return ResponseModel<long>.SuccessResponse();
		}
	}

}
