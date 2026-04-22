using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetRahmenLinkToAbPosition: IHandle<Identity.Models.UserModel, ResponseModel<List<LinkToABPositionModel>>>
	{
		private Models.Blanket.BlanketAbRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRahmenLinkToAbPosition(Identity.Models.UserModel user, Models.Blanket.BlanketAbRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<LinkToABPositionModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var abEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetRahmenLinkAB(this._data.ArtikelNr, this._data.KundenNr, this._data.Anzahl).OrderBy(x => x.Position).ToList();
				List<LinkToABPositionModel> response = new List<LinkToABPositionModel>();

				if(abEntity != null && abEntity.Count > 0)
					response = abEntity.Select(x => new LinkToABPositionModel(x)).ToList();

				return ResponseModel<List<LinkToABPositionModel>>.SuccessResponse(response);



			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{this._data.ArtikelNr},{this._data.KundenNr},{this._data.Anzahl}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<LinkToABPositionModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<LinkToABPositionModel>>.AccessDeniedResponse();
			}



			return ResponseModel<List<LinkToABPositionModel>>.SuccessResponse();
		}

	}
}
