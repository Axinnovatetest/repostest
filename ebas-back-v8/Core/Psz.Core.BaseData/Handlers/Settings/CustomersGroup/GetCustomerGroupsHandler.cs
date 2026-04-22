using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.CustomersGroup
{
	public class GetCustomerGroupsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomerGroupsHandler(Identity.Models.UserModel user)
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

				var pszKundengruppenEntities = Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity>();

				// - 
				if(refreshGroups(pszKundengruppenEntities))
				{
					pszKundengruppenEntities = Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Get();
				}
				var S = pszKundengruppenEntities.OrderBy(t => t.Kundengruppe != null)
					.ThenByDescending(t => t.Kundengruppe).ToArray();
				var response = new List<KeyValuePair<string, string>>();

				foreach(var pszKundengruppenEntity in S)
				{
					response.Add(new KeyValuePair<string, string>(pszKundengruppenEntity.ID.ToString(), $"{pszKundengruppenEntity.ID}||{pszKundengruppenEntity.Kundengruppe}"));
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
		public static bool refreshGroups(
			List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity> currentEntities)
		{
			var newEntities = new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity>();
			List<string> industriesFromCustomers = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetGroups()
				?? new List<string>();
			foreach(var item in industriesFromCustomers)
			{
				if(!currentEntities.Exists(x => x.Kundengruppe.Trim().ToLower() == item.Trim().ToLower()))
				{
					newEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.PSZ_KundengruppenEntity
					{
						Kundengruppe = item
					});
				}
			}

			// - 
			if(newEntities.Count > 0)
			{
				Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Insert(newEntities);
				return true;
			}

			// - 
			return false;
		}
	}
}
