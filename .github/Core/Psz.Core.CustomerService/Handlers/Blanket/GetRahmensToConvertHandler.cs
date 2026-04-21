using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetRahmensToConvertHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<RahmensToConvertModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetRahmensToConvertHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<List<RahmensToConvertModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var convertListEntity = Infrastructure.Data.Access.Joins.CTS.Divers.GetRahmensToConvert();
				var documentsNumbers = convertListEntity?.Where(x => !x.Rahmen_Nr.StringIsNullOrEmptyOrWhiteSpaces()).Select(y => y.Rahmen_Nr).ToList()
					.Union(convertListEntity?.Where(x => !x.Rahmen_Nr2.StringIsNullOrEmptyOrWhiteSpaces()).Select(y => y.Rahmen_Nr2).ToList()).ToList();
				var alreadyConverted = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetAlreadyConverted(documentsNumbers);
				if(alreadyConverted != null && alreadyConverted.Count > 0)
				{
					var docs = alreadyConverted.Select(x => x.Bezug).ToList();
					convertListEntity = convertListEntity.Where(x => !docs.Contains(x.Rahmen_Nr) && !docs.Contains(x.Rahmen_Nr2)).ToList();
				}
				var response = convertListEntity?.Select(x => new RahmensToConvertModel(x)).ToList();

				return ResponseModel<List<RahmensToConvertModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<RahmensToConvertModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<RahmensToConvertModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<RahmensToConvertModel>>.SuccessResponse();
		}

	}
}
