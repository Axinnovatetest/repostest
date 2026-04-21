using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.SuppliersGroup
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetSupplierGroupsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSupplierGroupsHandler(Identity.Models.UserModel user)
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

				var pszLieferantengruppenEntities = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get();
				var S = pszLieferantengruppenEntities.OrderBy(t => t.Lieferantengruppe != null)
						.ThenByDescending(t => t.Lieferantengruppe).ToArray();
				var response = new List<KeyValuePair<string, string>>();

				foreach(var pszLieferantengruppenEntity in S)
				{
					response.Add(new KeyValuePair<string, string>(pszLieferantengruppenEntity.Lieferantengruppe.ToString(), $"{pszLieferantengruppenEntity.ID}||{pszLieferantengruppenEntity.Lieferantengruppe}"));
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
