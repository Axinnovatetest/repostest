using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UpdateProductionTeamHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.ArticleProductionTeamRequestModel _data { get; set; }
		public UpdateProductionTeamHandler(UserModel user, Models.Article.Configuration.ArticleProductionTeamRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var teamEntity = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.GetWithTransaction(this._data.Id, botransaction.connection, botransaction.transaction);
				var oldDescription = teamEntity.Description;
				teamEntity.Description = this._data.Description;
				// -
				var insertedId = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.UpdateWithTransaction(teamEntity, botransaction.connection, botransaction.transaction);
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "TeamDescription", oldDescription,
						teamEntity.Description,
						Enums.ObjectLogEnums.Objects.ArticleConfig_ArticleTeams.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
				}
				#endregion // -- transaction-based logic -- //

				// -
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insertedId);
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

			if(Infrastructure.Data.Access.Tables.BSD.TeamsAccess.Get(this._data.Id) is null)
			{
				return ResponseModel<int>.FailureResponse("Selected team not found.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
