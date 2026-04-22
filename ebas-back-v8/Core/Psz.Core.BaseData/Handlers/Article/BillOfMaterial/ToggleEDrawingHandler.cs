using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class ToggleEDrawingHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public ToggleEDrawingHandler(UserModel user, int articleId)
		{
			_user = user;
			_data = articleId;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();

				// -
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.ToggleEDrawing(this._data, botransaction.connection, botransaction.transaction);

				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
                                    // - Status change log
                                    ObjectLogHelper.getLog(this._user, this._data, "Article BOM E-Drawing from", $"{articleEntity.IsEDrawing}",
									$"{!articleEntity.IsEDrawing}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit)
					}, botransaction.connection, botransaction.transaction);

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data);
					// -
					return ResponseModel<int>.SuccessResponse();
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Operation aborted: Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
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

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Article not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
