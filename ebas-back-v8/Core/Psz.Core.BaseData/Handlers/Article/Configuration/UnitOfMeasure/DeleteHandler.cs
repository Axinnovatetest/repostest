using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.UnitOfMeasure
{
	using Psz.Core.SharedKernel.Interfaces;
	using Psz.Core.Common.Models;
	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
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

				#region // -- transaction-based logic -- //
				// -
				var entity = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
				var responseBody = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.DeleteWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				// -
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(ObjectLogHelper.getLog(this._user, responseBody,
						  $"",
						  $"{entity.Symbol} | {entity.Name}",
						  $"", Enums.ObjectLogEnums.Objects.ArticleConfig_UnitOfMeasure.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Delete),
						  botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(responseBody);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
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

			var entity = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.Get(this._data);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByUoMSymbol(entity.Symbol);
			if(articleEntities != null && articleEntities.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Einheit [{(entity.Symbol)} | {entity.Name}] has articles [{(string.Join(", ", articleEntities.Take(5).Select(x => x.ArtikelNummer)))}]. Please change/delete the articles first.");
			}
			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
