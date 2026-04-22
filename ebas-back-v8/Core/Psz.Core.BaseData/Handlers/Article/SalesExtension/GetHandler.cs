using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHandler: IHandle<UserModel, ResponseModel<Models.Article.SalesExtension.SalesItemModel>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHandler(UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.SalesExtension.SalesItemModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var packagingEntities = Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Get();
				var x = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(this._data);
				return ResponseModel<Models.Article.SalesExtension.SalesItemModel>.SuccessResponse(
						new Models.Article.SalesExtension.SalesItemModel(x,
						packagingEntities?.FirstOrDefault(y => y.Id == (int)x.VerpackungsartId)));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.SalesExtension.SalesItemModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.SalesExtension.SalesItemModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get(this._data) == null)
			{
				return new ResponseModel<Models.Article.SalesExtension.SalesItemModel>
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.SalesExtension.SalesItemModel>.ResponseError>
					{
						new ResponseModel<Models.Article.SalesExtension.SalesItemModel>.ResponseError
						{
							Key = "1",
							Value = "Sales item not found"
						}
					}
				};
			}


			return ResponseModel<Models.Article.SalesExtension.SalesItemModel>.SuccessResponse();
		}
	}
}
