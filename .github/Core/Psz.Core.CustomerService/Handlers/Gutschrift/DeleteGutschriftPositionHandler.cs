using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class DeleteGutschriftPositionHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteGutschriftPositionHandler(Identity.Models.UserModel user, int data)
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

				var response = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Delete(_data);

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
			var gutschriftPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data);
			if(gutschriftPositionEntity == null)
				return ResponseModel<int>.FailureResponse("gutschrift item not found .");
			//var technicArticles = Module.BSD.TechnicArticleIds;
			if(!Helpers.HorizonsHelper.ArticleIsTechnic(gutschriftPositionEntity.ArtikelNr ?? -1))
			{
				var horizonCheck = Helpers.HorizonsHelper.userHasGSPosHorizonRight(gutschriftPositionEntity.Liefertermin ?? new DateTime(1900, 1, 1), _user, out List<string> messages);
				if(!horizonCheck)
					return ResponseModel<int>.FailureResponse(messages);
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
