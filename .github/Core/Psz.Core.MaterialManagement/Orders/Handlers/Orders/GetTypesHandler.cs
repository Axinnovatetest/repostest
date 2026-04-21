using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class GetTypesHandler: IHandle<GetRequestModel, ResponseModel<List<TypesResponseModel>>>
	{
		private UserModel user { get; set; }

		public GetTypesHandler(UserModel user)
		{
			this.user = user;
		}
		public ResponseModel<List<TypesResponseModel>> Handle()
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

		private ResponseModel<List<TypesResponseModel>> Perform()
		{
			return ResponseModel<List<TypesResponseModel>>.SuccessResponse(
				Enum.GetValues(typeof(Enums.OrderEnums.OrderTypes)).Cast<Enums.OrderEnums.OrderTypes>()
								   .Select(x => new TypesResponseModel { Id = (int)x, Name = x.GetDescription() })
								   .ToList()
				);
		}

		public ResponseModel<List<TypesResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<TypesResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<TypesResponseModel>>.SuccessResponse();
		}
	}
}
