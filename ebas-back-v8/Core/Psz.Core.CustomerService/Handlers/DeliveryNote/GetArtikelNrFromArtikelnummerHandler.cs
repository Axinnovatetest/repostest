using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.DeliveryNote
{
	public class GetArtikelNrFromArtikelnummerHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private string _articelnummer { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetArtikelNrFromArtikelnummerHandler(string articelnummer, Identity.Models.UserModel user)
		{
			this._user = user;
			this._articelnummer = articelnummer;
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

				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(_articelnummer);
				if(articleEntity == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Article not found");

				return ResponseModel<int>.SuccessResponse(articleEntity.ArtikelNr);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _articelnummer:{_articelnummer}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}


		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
