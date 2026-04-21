using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class SetLagerMinStockHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Logistics.SetLagerMinStockRequestModel _data { get; set; }
		public SetLagerMinStockHandler(UserModel user, Models.Article.Logistics.SetLagerMinStockRequestModel data)
		{
			this._user = user;
			this._data = data;
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
				// -
				List<string> _changes = new List<string>();
				var response = -1;
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
				var lagerStatusEntity = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetStandardByArticleAndId(this._data.ArticleId, this._data.WarehouseNumber);
				if(lagerStatusEntity.Mindestbestand != this._data.WarehouseMinStock)
				{
					response = Infrastructure.Data.Access.Tables.PRS.LagerAccess.UpdateMinStock(lagerStatusEntity.ID, this._data.WarehouseMinStock);

					// - save logs 
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, articleEntity.ArtikelNr, $"Mindestbestand | Lager [{lagerStatusEntity.Lagerort_id}]", $"{lagerStatusEntity.Mindestbestand}",
									$"{this._data.WarehouseMinStock}", $"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}", Enums.ObjectLogEnums.LogType.Add));
				}

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.ArticleId);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null || !(this._user.Access?.MasterData?.EditLagerMinStock == true || this._user?.IsGlobalDirector == true || this._user?.SuperAdministrator == true))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<int>.FailureResponse("Article not found");

			if(Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetStandardByArticleAndId(this._data.ArticleId, this._data.WarehouseNumber ) == null)
				return ResponseModel<int>.FailureResponse("Lager not found");

			if(this._data.WarehouseMinStock < 0)
				return ResponseModel<int>.FailureResponse("Mindestbestand should not be less then zero");


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
