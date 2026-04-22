using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class GetFullLogHandler: IHandle<int, ResponseModel<List<LogResponseModel>>>
	{

		private int data { get; set; }
		private UserModel user { get; set; }

		public GetFullLogHandler(UserModel user, int data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<LogResponseModel>> Handle()
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

		private ResponseModel<List<LogResponseModel>> Perform()
		{

			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data);

			return ResponseModel<List<LogResponseModel>>.SuccessResponse(
				Infrastructure.Data.Access.Tables.MTM.BestellungenProcessing_LogAccess.GetByOrderid(this.data, bestellung.Typ)
				?.Select(x => new LogResponseModel(x))
				?.ToList()
				);
		}

		public ResponseModel<List<LogResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<LogResponseModel>>.AccessDeniedResponse();
			}
			var bestellung = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(data);
			if(bestellung is null)
				return ResponseModel<List<LogResponseModel>>.NotFoundResponse();

			return ResponseModel<List<LogResponseModel>>.SuccessResponse();
		}
	}
}
