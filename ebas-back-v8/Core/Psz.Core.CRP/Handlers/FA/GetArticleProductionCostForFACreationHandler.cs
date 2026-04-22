using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetArticleProductionCostForFACreationHandler: IHandle<Identity.Models.UserModel, ResponseModel<decimal>>
	{
		private readonly Identity.Models.UserModel _user;
		private readonly Models.FA.ArticleProductionCostForFACreationRequestModel _data;

		public GetArticleProductionCostForFACreationHandler(Identity.Models.UserModel user, Models.FA.ArticleProductionCostForFACreationRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<decimal> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;
			try
			{
				var saleItemEntity = new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity();
				if(_data.FromAB.HasValue && _data.FromAB.Value)
				{
					var type = GetTypeNameForCTS(_data.TypeId);
					saleItemEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndTypeName(_data.ArtikelNr, type);
				}
				else
					saleItemEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndTypeId(_data.ArtikelNr, _data.TypeId);
				return ResponseModel<decimal>.SuccessResponse(saleItemEntity.Produktionskosten ?? 0m);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<decimal> Validate()
		{
			if(_user == null)
				return ResponseModel<decimal>.AccessDeniedResponse();
			return ResponseModel<decimal>.SuccessResponse();
		}
		internal static string GetTypeNameForCTS(int typeId)
		{
			return ((Enums.FAEnums.ItemType)typeId).GetDescription();
		}
	}
}