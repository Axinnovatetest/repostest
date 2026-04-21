using Psz.Core.MaterialManagement.Orders.Models.Users;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Users
{
	public class GetHandler: IHandle<GetRequestModel, ResponseModel<List<GetResponseModel>>>
	{

		private GetRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetHandler(UserModel user, GetRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetResponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<List<GetResponseModel>> Perform(UserModel user, GetRequestModel data)
		{
			#region Prepare Data
			var filteredData = Infrastructure.Data.Access.Tables.COR.UserAccess.GetLikeNameLegacyNotNull(data.Filter);
			#endregion
			if(filteredData == null || filteredData.Count == 0)
				return ResponseModel<List<GetResponseModel>>.NotFoundResponse();

			return ResponseModel<List<GetResponseModel>>.SuccessResponse(filteredData.Select(x => new GetResponseModel(x)).ToList());
		}

		public ResponseModel<List<GetResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<GetResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetResponseModel>>.SuccessResponse();
		}
	}
}
