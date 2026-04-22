using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension.CustomPrice
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var articleCustomPriceExt = Infrastructure.Data.Access.Tables.BSD.StaffelpreisExtensionAccess.Get(this._data);
				var customPriceEntity = Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.Get(articleCustomPriceExt.StaffelNr.HasValue
					? articleCustomPriceExt.StaffelNr.Value
					: -1);
				var preisgruppenEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr((int)customPriceEntity.Artikel_Nr);

				return ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>.SuccessResponse(
						new Models.Article.SalesExtension.CustomPrice.CustomPriceModel(customPriceEntity, preisgruppenEntity, articleCustomPriceExt));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.StaffelpreisExtensionAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>.ResponseError>
					{
						new ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>.ResponseError
						{
							Key = "1",
							Value = "Custom Price not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.SalesExtension.CustomPrice.CustomPriceModel>.SuccessResponse();
		}
	}
}
