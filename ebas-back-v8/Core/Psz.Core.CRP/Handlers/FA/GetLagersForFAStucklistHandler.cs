using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetLagersForFAStucklistHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetLagersForFAStucklistHandler(Identity.Models.UserModel user)
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

				var lagersEntities = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForFAStcklist(Module.BSD.ProductionLagerIds/*new List<int> { 6, 7, 15, 26, 42, 60 }*/); // - 2022-12-23 - Khelil - remove Virtual AL
				List<KeyValuePair<int, string>> response = new List<KeyValuePair<int, string>>();
				if(lagersEntities != null && lagersEntities.Count > 0)
					response = lagersEntities.Select(x => new KeyValuePair<int, string>(x.LagerortId, x.Lagerort)).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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