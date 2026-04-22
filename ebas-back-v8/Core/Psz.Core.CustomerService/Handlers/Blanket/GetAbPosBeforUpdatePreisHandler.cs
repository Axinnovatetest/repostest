using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetAbPosBeforUpdatePreisHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<AbPosBeforPreisUpdateModel>>>
	{
		private Models.Blanket.ABPosBeforePriceUpdaterequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetAbPosBeforUpdatePreisHandler(Identity.Models.UserModel user, Models.Blanket.ABPosBeforePriceUpdaterequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<AbPosBeforPreisUpdateModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var PosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetABBeforPreisUpdate(this._data.RahmenNr, this._data.PositionNr, this._data.DateFin).OrderBy(x => x.Position).ToList();
				List<AbPosBeforPreisUpdateModel> response = new List<AbPosBeforPreisUpdateModel>();

				if(PosEntity != null && PosEntity.Count > 0)
					response = PosEntity.Select(x => new AbPosBeforPreisUpdateModel(x)).ToList();
				if(response != null && response.Count > 0)
					return ResponseModel<List<AbPosBeforPreisUpdateModel>>.SuccessResponse(response);
				else
					return ResponseModel<List<AbPosBeforPreisUpdateModel>>.SuccessResponse(null);



			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<AbPosBeforPreisUpdateModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<AbPosBeforPreisUpdateModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<AbPosBeforPreisUpdateModel>>.SuccessResponse();
		}

	}
}