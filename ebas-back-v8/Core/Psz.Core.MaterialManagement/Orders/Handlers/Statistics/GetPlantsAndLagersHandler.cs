using Psz.Core.SharedKernel.Interfaces;
using System.IO;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetPlantsAndLagersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<string>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetPlantsAndLagersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<List<string>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var lagers = Psz.Core.MaterialManagement.Helpers.SpecialHelper.GetAllPlants();

				if(lagers is null)
					throw new InvalidDataException("No Lager Found !");

				var rest = lagers.Select(x => x.Plant).ToList();

				return ResponseModel<List<string>>.SuccessResponse(rest);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<string>> Validate()
		{
			if(this._user is null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<string>>.AccessDeniedResponse();
			}

			return ResponseModel<List<string>>.SuccessResponse();
		}

	}
}
