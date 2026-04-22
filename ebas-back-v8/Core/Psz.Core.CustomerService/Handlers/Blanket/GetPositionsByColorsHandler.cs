using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetPositionsByColorsHandler: IHandle<Identity.Models.UserModel, ResponseModel<PositionsByColorsModel>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetPositionsByColorsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<PositionsByColorsModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var response = new PositionsByColorsModel();
				response.RedPositions = new List<BlanketItem>();
				response.YellowPositions = new List<BlanketItem>();
				response.OrangePositions = new List<BlanketItem>();
				response.GreenPositions = new List<BlanketItem>();

				var rahmenPosWithAbThatHasLS = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetABLinkedTorahmens();
				var PosThatHasExtensions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNrs(rahmenPosWithAbThatHasLS);
				var Colors = Helpers.BlanketHelper.GetPositionsColors(PosThatHasExtensions.Select(x => x.AngeboteArtikelNr).ToList());

				var posList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(Colors?.Select(x => x.Item2).ToList());
				var posListExtensions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNrs(Colors?.Select(x => x.Item2).ToList());

				foreach(var item in posList)
				{
					var extension = posListExtensions.Find(x => x.AngeboteArtikelNr == item.Nr);
					var colorItem = Colors.Find(x => x.Item2 == item.Nr);
					var responseItem = new BlanketItem(item, extension);
					if(colorItem.Item3 == (int)Enums.BlanketEnums.ColorStatus.Red)
						response.RedPositions.Add(responseItem);
					if(colorItem.Item3 == (int)Enums.BlanketEnums.ColorStatus.Orange)
						response.OrangePositions.Add(responseItem);
					if(colorItem.Item3 == (int)Enums.BlanketEnums.ColorStatus.Yellow)
						response.YellowPositions.Add(responseItem);
					if(colorItem.Item3 == (int)Enums.BlanketEnums.ColorStatus.Green)
						response.GreenPositions.Add(responseItem);
				}


				return ResponseModel<PositionsByColorsModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<PositionsByColorsModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<PositionsByColorsModel>.AccessDeniedResponse();
			}

			return ResponseModel<PositionsByColorsModel>.SuccessResponse();
		}

	}
}
