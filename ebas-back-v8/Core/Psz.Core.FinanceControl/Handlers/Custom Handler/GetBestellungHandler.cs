using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.FinanceControl.Models.Budget.Reception;

namespace Psz.Core.FinanceControl.Handlers
{
	public class GetBestellungHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetBestellungHandler(Identity.Models.UserModel user, int model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				if(_data <= 0)
					return ResponseModel<int>.SuccessResponse();

				var fetchedData = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetBestellungen(this._data);
				return ResponseModel<int>.SuccessResponse(validateResult(fetchedData));

				//var response = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetBestellungen(this._data);
				//return ResponseModel<int>.SuccessResponse(validateResult (response));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}

		private int validateResult(Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity validateentity) {
			if (validateentity is null)
					return -1;

			return validateentity.Nr;

		}


	}
}


