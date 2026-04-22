using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.DiscountGroup
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetDiscountGroupsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetDiscountGroupsHandler(Identity.Models.UserModel user)
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

				var rabatthauptgruppenEntities = Infrastructure.Data.Access.Tables.BSD.RabatthauptgruppenAccess.Get();
				var S = rabatthauptgruppenEntities.OrderBy(t => t.Beschreibung != null)
   .ThenByDescending(t => t.Beschreibung).ToArray();
				var response = new List<KeyValuePair<int, string>>();

				foreach(var rabatthauptgruppenEntity in S)
				{
					response.Add(new KeyValuePair<int, string>(rabatthauptgruppenEntity.ID, $"{rabatthauptgruppenEntity.ID}||{rabatthauptgruppenEntity.Beschreibung}"));
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
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
