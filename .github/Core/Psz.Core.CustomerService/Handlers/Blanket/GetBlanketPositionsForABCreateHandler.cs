using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetBlanketPositionsForABCreateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BlanketPositionsMinimalModel>>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBlanketPositionsForABCreateHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<BlanketPositionsMinimalModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<BlanketPositionsMinimalModel>();
				var angebotePositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data, false).OrderBy(x => x.Position).ToList();
				var extensionPositions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenNr(_data)
					?? new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();

				foreach(var item in angebotePositions)
				{
					var extension = extensionPositions.Find(x => x.AngeboteArtikelNr == item.Nr);
					if(!item.erledigt_pos.HasValue || (item.erledigt_pos.HasValue && !item.erledigt_pos.Value))
						response.Add(new BlanketPositionsMinimalModel(item, extension));
				}


				return ResponseModel<List<BlanketPositionsMinimalModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<BlanketPositionsMinimalModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<BlanketPositionsMinimalModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<BlanketPositionsMinimalModel>>.SuccessResponse();
		}
	}
}
