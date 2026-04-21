using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.SlipCircle
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetSlipCirclesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSlipCirclesHandler(Identity.Models.UserModel user)
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

				var belegkreiseVorgabenEntities = Infrastructure.Data.Access.Tables.BSD.BelegkreiseVorgabenAccess.Get();
				var S = belegkreiseVorgabenEntities.OrderBy(t => t.Bezeichnung != null)
				.ThenBy(t => t.ID).ToArray();
				var response = new List<KeyValuePair<int, string>>();

				foreach(var belegkreiseVorgabenEntity in S)
				{
					response.Add(new KeyValuePair<int, string>((int)belegkreiseVorgabenEntity.Belegkreis, $"{belegkreiseVorgabenEntity.Belegkreis} || {belegkreiseVorgabenEntity.Bezeichnung}"));
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
