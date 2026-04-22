using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetArticleCustomerContactForFACreationHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
	{
		private readonly UserModel _user;
		private readonly int _data;

		public GetArticleCustomerContactForFACreationHandler(Identity.Models.UserModel user, int data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<string> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data);
				var kreis = articleEntity.ArtikelNummer.Split('-')[0];
				var kriesEntity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(kreis);
				return ResponseModel<string>.SuccessResponse(kriesEntity[0].CS_Kontakt);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(_user == null)
				return ResponseModel<string>.AccessDeniedResponse();
			return ResponseModel<string>.SuccessResponse();
		}
	}
}