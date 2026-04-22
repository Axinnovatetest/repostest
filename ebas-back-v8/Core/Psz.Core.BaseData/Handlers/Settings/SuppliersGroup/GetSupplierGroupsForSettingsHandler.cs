using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.SuppliersGroup
{
	public class GetSupplierGroupsForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.SuppliersGroup.SuppliersGroupModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSupplierGroupsForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.SuppliersGroup.SuppliersGroupModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var pszLieferantengruppenEntities = (Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.BSD.PszLieferantengruppenEntity>()).OrderBy(x => x.ID);

				var response = new List<Models.SuppliersGroup.SuppliersGroupModel>();

				foreach(var pszLieferantengruppenEntity in pszLieferantengruppenEntities)
				{
					response.Add(new Models.SuppliersGroup.SuppliersGroupModel(pszLieferantengruppenEntity));
				}

				return ResponseModel<List<Models.SuppliersGroup.SuppliersGroupModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.SuppliersGroup.SuppliersGroupModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.SuppliersGroup.SuppliersGroupModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.SuppliersGroup.SuppliersGroupModel>>.SuccessResponse();
		}
	}
}
