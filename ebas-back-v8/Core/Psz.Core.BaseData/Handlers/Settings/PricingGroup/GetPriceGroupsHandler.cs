using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.PricingGroup
{

	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetPricingGroupsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetPricingGroupsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<KeyValuePair<int, string>>();
				var preisgruppenEntities = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.GetPricingGroups();
				var S = preisgruppenEntities.OrderBy(t => t.Bemerkung != null)
.ThenByDescending(t => t.Bemerkung).ToArray();
				if(preisgruppenEntities != null && preisgruppenEntities.Count > 0)
				{
					foreach(var preisgruppenEntity in S)
					{
						responseBody.Add(new KeyValuePair<int, string>(preisgruppenEntity.Preisgruppe ?? -1, $"{((int)preisgruppenEntity.Preisgruppe).ToString().Trim()} || {preisgruppenEntity.Bemerkung.Trim()}"));
					}
					responseBody = responseBody.Distinct().ToList();
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
