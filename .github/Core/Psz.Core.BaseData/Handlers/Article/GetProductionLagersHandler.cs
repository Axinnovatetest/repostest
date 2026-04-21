using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public partial class GetProductionLagersHandler: IHandle<string, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetProductionLagersHandler(Identity.Models.UserModel user)
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

				// -
				var results = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetProductionLagers(Module.AppSettings.ProductionLagerIds);

				// -
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
					results?.Select(x => new KeyValuePair<int, string>(x.Lagerort_id, x.Lagerort))?.Distinct()?.ToList());

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

			// -
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
