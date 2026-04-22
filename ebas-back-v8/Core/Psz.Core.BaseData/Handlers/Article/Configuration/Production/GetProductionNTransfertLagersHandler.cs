using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetProductionNTransfertLagersHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		public GetProductionNTransfertLagersHandler(UserModel user)
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

				var stockLagers = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(Module.AppSettings.ProductionLagerIds ?? new List<int>()); // - new List<int> { 6, 7, 15, 26, 42, 60 };
				stockLagers.AddRange(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetTransfertLagers() ?? new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>());
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(stockLagers?.Distinct()
						.Select(x => new KeyValuePair<int, string>((int)x.Lagerort_id, x.Lagerort)).Distinct().ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
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
