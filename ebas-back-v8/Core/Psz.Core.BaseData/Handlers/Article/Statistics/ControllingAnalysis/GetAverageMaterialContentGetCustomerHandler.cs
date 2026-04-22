using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAverageMaterialContentGetCustomerHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		public GetAverageMaterialContentGetCustomerHandler(UserModel user)
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

				var customers = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get() ?? new List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity>();
				var addresses = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customers.Select(x => x.Nummer ?? -1).ToList()) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse((customers.Join(addresses, c => c.Nummer, a => a.Nr, (c, a) => new KeyValuePair<int, string>(a.Nr, a.Name1))
					?.ToList()));
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
