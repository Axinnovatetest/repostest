using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.ProductGroup
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public DeleteHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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

				var productGroup = Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get(this._data);
				var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByProductGroupName(productGroup.Warengruppe);
				if(articles != null && articles.Count > 0)
				{
					return new ResponseModel<int>
					{
						Success = false,
						Errors = new List<ResponseModel<int>.ResponseError>
						{
							new ResponseModel<int>.ResponseError
							{
								Key ="",
								Value = $"Product Group contains articles '{string.Join("', '", articles.Select(x => x.ArtikelNummer).Distinct().Take(5).ToList())}'"
							}
						}
					};
				}

				// 
				var deletedId = Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Delete(this._data);
				if(deletedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, deletedId, "Warengruppe", productGroup.Warengruppe,
						"",
						Enums.ObjectLogEnums.Objects.ArticleConfig_ProductGroup.GetDescription(),
						Enums.ObjectLogEnums.LogType.Delete));
				}
				return ResponseModel<int>.SuccessResponse(deletedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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

			if(Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get(this._data) == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Product Group not found"}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
