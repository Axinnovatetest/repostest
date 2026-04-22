using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class UpdateGutschriftPositionDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private GutschriftItemModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateGutschriftPositionDataHandler(Identity.Models.UserModel user, GutschriftItemModel data)
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

				var gutschriftPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.Nr);

				// -
				gutschriftPositionEntity.POSTEXT = _data.POSTEXT;
				gutschriftPositionEntity.GSInternComment = _data.InternComment;
				gutschriftPositionEntity.GSExternComment = _data.Comment;

				// - 
				var response = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateGSText(gutschriftPositionEntity);

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

			if(Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.Nr) == null)
				return ResponseModel<int>.FailureResponse("Gutschrift not found.");

			if(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.AngebotNr) == null)
				return ResponseModel<int>.FailureResponse("Gutschrift position not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
