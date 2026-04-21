using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetBestandLagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetBestandLagerHandler(Identity.Models.UserModel user)
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

				var response = new List<KeyValuePair<string, string>>();
				var lagerortEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get();
				if(lagerortEntity != null && lagerortEntity.Count > 0)
					response = lagerortEntity.Select(a => new KeyValuePair<string, string>(a.Lagerort, a.Lagerort)).ToList();

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}
