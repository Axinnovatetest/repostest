using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers
{
	public class GetSufeHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Statistics.SupplierStufeResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetSufeHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Statistics.SupplierStufeResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var responseBody = new List<Models.Statistics.SupplierStufeResponseModel>();
				var adressenEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetAllSupplierAddresses()
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>();
				var lierferantEntities = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummers(adressenEntities.Select(x => x.Nr)?.ToList());
				foreach(var entity in adressenEntities)
				{
					var l = lierferantEntities.FirstOrDefault(x => x.Nummer == entity.Nr);
					responseBody.Add(new Models.Statistics.SupplierStufeResponseModel(entity, l));
				}
				// -
				return ResponseModel<List<Models.Statistics.SupplierStufeResponseModel>>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Statistics.SupplierStufeResponseModel>> Validate()
		{
			if(this._user is null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Statistics.SupplierStufeResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Statistics.SupplierStufeResponseModel>>.SuccessResponse();
		}
	}
}
