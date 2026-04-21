using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class DeleteProductionTeamHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteProductionTeamHandler(UserModel user, int data)
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
				var teamEntity = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
				var insertedId = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.DeleteWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				// -
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "TeamDescription", teamEntity.Name,
						"",
						Enums.ObjectLogEnums.Objects.ArticleConfig_ArticleTeams.GetDescription(),
						Enums.ObjectLogEnums.LogType.Delete));
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

			var team = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.Get(this._data);
			if(team is null)
			{
				return ResponseModel<int>.FailureResponse("Selected team not found.");
			}
			var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByTeam(team.Name);
			if(articles?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Cannot delete team assigned to Article. Please remove from [{string.Join("], [", articles.Take(5).Select(x => x.ArtikelNummer))}] first.");
			}
			var nextTeams = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.GetNextTeams(team.SiteId ?? -1, team.TeamIndex ?? -1, team.TeamCategory ?? 'A');
			if(nextTeams?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Cannot delete team in the middle. Please remove [{string.Join("], [", nextTeams.Take(5).Select(x => x.Name))}] first.");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
