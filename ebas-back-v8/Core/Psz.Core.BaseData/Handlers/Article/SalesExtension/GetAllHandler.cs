using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.SalesExtension
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<UserModel, ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>>
	{
		private UserModel _user { get; set; }
		public GetAllHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var salesItemEntites = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.Get();
				var packagingEntities = Infrastructure.Data.Access.Tables.BSD.Verpackungseinheiten_DefinitionenAccess.Get();
				if(salesItemEntites != null && salesItemEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>.SuccessResponse(salesItemEntites
							.Select(x => new Models.Article.SalesExtension.SalesItemModel(x,
								packagingEntities?.FirstOrDefault(y => y.Id == (int)x.VerpackungsartId))).ToList());
				}

				return ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.SalesExtension.SalesItemModel>>.SuccessResponse();
		}
	}
}
