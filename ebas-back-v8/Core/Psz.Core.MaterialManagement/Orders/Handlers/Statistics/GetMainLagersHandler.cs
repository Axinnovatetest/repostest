using Psz.Core.SharedKernel.Interfaces;
using System.IO;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetMainLagersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<int>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetMainLagersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<List<int>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var lagers = Psz.Core.MaterialManagement.Helpers.SpecialHelper.GetLMainLagers();

				if(lagers is null)
					throw new InvalidDataException("No Lager Found !");

				return ResponseModel<List<int>>.SuccessResponse(lagers);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<int>> Validate()
		{
			if(this._user is null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<int>>.AccessDeniedResponse();
			}

			return ResponseModel<List<int>>.SuccessResponse();
		}

	}
}
