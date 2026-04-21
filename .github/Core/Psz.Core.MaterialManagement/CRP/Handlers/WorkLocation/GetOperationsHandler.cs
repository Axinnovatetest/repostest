using Infrastructure.Data.Access.Tables.WPL;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation
{
	public class GetOperationsHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<List<Models.WorkLocation.OperationModel>>>
	{
		private Psz.Core.Identity.Models.UserModel user { get; set; }

		public GetOperationsHandler(Psz.Core.Identity.Models.UserModel user)
		{
			this.user = user;
		}

		public ResponseModel<List<Models.WorkLocation.OperationModel>> Handle()
		{
			try
			{
				if(user == null)
				{
					throw new Psz.Core.SharedKernel.Exceptions.UnauthorizedException();
				}

				var operationEntities = StandardOperationAccess.Get().FindAll(e => !e.IsArchived);

				var response = new List<Models.WorkLocation.OperationModel>();

				foreach(var operationEntity in operationEntities)
				{
					response.Add(new Models.WorkLocation.OperationModel()
					{
						Id = operationEntity.Id,
						Name = operationEntity.Name,
					});
				}

				return ResponseModel<List<Models.WorkLocation.OperationModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.WorkLocation.OperationModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
