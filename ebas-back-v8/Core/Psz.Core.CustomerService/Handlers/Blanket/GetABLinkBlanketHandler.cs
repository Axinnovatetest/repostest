using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetABLinkBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<AbLinkBlanketModel>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetABLinkBlanketHandler(Identity.Models.UserModel user, int Nr)
		{
			this._user = user;
			this._data = Nr;

		}
		public ResponseModel<List<AbLinkBlanketModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<AbLinkBlanketModel> response = new List<AbLinkBlanketModel>();

				var rahmenItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data, false);
				var ABPositionsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPositions(rahmenItemsEntities?.Select(x => x.Nr)?.ToList());
				var ABRahmenLinks = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetBelegfluss(ABPositionsEntities?.Select(x => x.AngebotNr ?? -1).Distinct().ToList());
				if(ABRahmenLinks != null && ABRahmenLinks.Count > 0)
				{
					response = ABRahmenLinks.Select(x => new AbLinkBlanketModel(x)).ToList();
				}

				return ResponseModel<List<AbLinkBlanketModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<AbLinkBlanketModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<AbLinkBlanketModel>>.AccessDeniedResponse();
			}
			var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
			if(rahmenEntity == null)
				return ResponseModel<List<AbLinkBlanketModel>>.FailureResponse("rahmen not found");
			return ResponseModel<List<AbLinkBlanketModel>>.SuccessResponse();
		}
	}
}
