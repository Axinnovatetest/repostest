using Psz.Core.MaterialManagement.Orders.Models.Wareneingang;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Wareneingang
{
	public class GetLagerortHandler: IHandle<string, ResponseModel<List<GetLagerortModel>>>
	{

		private UserModel user { get; set; }

		public GetLagerortHandler(UserModel user)
		{
			this.user = user;
		}



		public ResponseModel<List<GetLagerortModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<List<GetLagerortModel>> Perform()
		{
			var lagerort = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.GetActive();

			return ResponseModel<List<GetLagerortModel>>.SuccessResponse(lagerort.Select(x => new GetLagerortModel(x)).ToList());

		}
		public ResponseModel<List<GetLagerortModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<GetLagerortModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetLagerortModel>>.SuccessResponse();
		}
	}
}
