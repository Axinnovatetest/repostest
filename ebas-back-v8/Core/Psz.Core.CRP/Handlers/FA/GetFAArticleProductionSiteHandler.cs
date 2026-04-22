using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAArticleProductionSiteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAArticleProductionSiteHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
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

				var response = -1;
				var articleProductionExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(_data);
				if(articleProductionExtensionEntity != null && articleProductionExtensionEntity.ProductionPlace1_Id.HasValue)
				{
					var _prodPlace = -1;
					// - 2022-12-22 - return EnumValue
					foreach(int i in Enum.GetValues(typeof(Common.Enums.ArticleEnums.ArticleProductionPlace)))
					{
						if(i == articleProductionExtensionEntity.ProductionPlace1_Id.Value)
						{
							_prodPlace = i;
							break;
						}
					}
					response = _prodPlace;
				}
				return ResponseModel<int>.SuccessResponse(response);
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
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse("Article not found .");
			return ResponseModel<int>.SuccessResponse();
		}
	}
}