using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	public class GetBOMLogHandler: IHandle<UserModel, ResponseModel<List<Models.Article.BillOfMaterial.BomLog>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetBOMLogHandler(UserModel user, int ArticleID)
		{
			this._user = user;
			this._data = ArticleID;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.BomLog>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var logEntity = new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity>();
				if(articleEntity != null)
				{
					logEntity = Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.GetByArtikelID(articleEntity.ArtikelNummer);
				}
				var response = new List<Models.Article.BillOfMaterial.BomLog>();
				foreach(var item in logEntity)
				{
					response.Add(new Models.Article.BillOfMaterial.BomLog(item));
				}
				return ResponseModel<List<Models.Article.BillOfMaterial.BomLog>>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.BomLog>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.BomLog>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.BomLog>>.FailureResponse(key: "1", value: "BOM article not found");


			return ResponseModel<List<Models.Article.BillOfMaterial.BomLog>>.SuccessResponse();
		}
	}
}
