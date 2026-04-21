using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetPositionsColorsStatsHandler: IHandle<Identity.Models.UserModel, ResponseModel<PositionsColorsResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetPositionsColorsStatsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<PositionsColorsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var rahmenPosWithAb = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetABLinkedTorahmens();
				var PosThatHasExtensions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNrs(rahmenPosWithAb);
				var Colors = Helpers.BlanketHelper.GetPositionsColors(PosThatHasExtensions.Select(x => x.AngeboteArtikelNr).ToList());
				var response = new PositionsColorsResponseModel(Colors);

				return ResponseModel<PositionsColorsResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<PositionsColorsResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<PositionsColorsResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<PositionsColorsResponseModel>.SuccessResponse();
		}

	}
}
