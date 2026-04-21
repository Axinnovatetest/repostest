using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.Industry
{
	public class GetSuppliersIndustriesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSuppliersIndustriesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var industryEntities = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.GetByType(2)
					?? new List<Infrastructure.Data.Entities.Tables.BSD.IndustryEntity>();

				// - add missing Industry values from Lieferanten table
				if(GetCustomerIndustriesHandler.refreshIndustry(this._user, industryEntities, Enums.AddressEnums.Industries.Supplier))
				{
					industryEntities = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.GetByType((int)Enums.AddressEnums.Industries.Supplier);
				}

				// - 
				var S = industryEntities.OrderBy(t => t.Name != null)
						.ThenByDescending(t => t.Name).ToArray();
				var response = new List<KeyValuePair<string, string>>();

				foreach(var industryEntity in S)
				{
					response.Add(new KeyValuePair<string, string>(industryEntity.Name, $"{industryEntity.Id} || {industryEntity.Name}"));
				}

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}
