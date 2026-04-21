using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;

	public class AddProductionTeamHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.ArticleProductionTeamRequestModel _data { get; set; }
		public AddProductionTeamHandler(UserModel user, Models.Article.Configuration.ArticleProductionTeamRequestModel data)
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
				var sites = new Handlers.Article.Configuration.Production.GetListProductionPlaceHandler(this._user).Handle().Body;
				var lastTeam = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.GetMaxBySite(this._data.SiteId, botransaction.connection, botransaction.transaction);
				lastTeam = getNextTeamName(lastTeam);
				if(lastTeam is null)
				{

					var sitesL = new List<string> { "DE", "AL", "CZ", "TN", "BETN", "WS", "GZ" };
					var siteIdsL = new List<int> { 15, 26, 6, 7, 60, 42, 102 };
					var idx = siteIdsL.IndexOf(this._data.SiteId);
					lastTeam = new Infrastructure.Data.Entities.Tables.BSD.TeamsEntity
					{
						SiteId = this._data.SiteId,
						Description = this._data.Description,
						Id = -1,
						Name = $"{sitesL[idx]}-TW6-A".ToUpper(),
						SitePrefix = sitesL[idx],
						TeamCategory = 'A',
						TeamIndex = 6
					};
				}
				else
				{
					lastTeam.Description = this._data.Description;
				}
				// -
				var insertedId = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.InsertWithTransaction(lastTeam, botransaction.connection, botransaction.transaction);
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "Team", "",
						lastTeam.Name,
						Enums.ObjectLogEnums.Objects.ArticleConfig_ArticleTeams.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add));
				}
				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
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

			if(new Handlers.Article.Configuration.Production.GetListProductionPlaceHandler(this._user).Handle().Body?.Exists(x => x.Key == this._data.SiteId) != true)
			{
				return ResponseModel<int>.FailureResponse("Selected site not found.");
			}

			return ResponseModel<int>.SuccessResponse();
		}

		internal Infrastructure.Data.Entities.Tables.BSD.TeamsEntity getNextTeamName(Infrastructure.Data.Entities.Tables.BSD.TeamsEntity lastTeamEntity)
		{
			if(lastTeamEntity == null)
				return null;

			// -
			var nextEntity = lastTeamEntity.ShallowClone();
			nextEntity.TeamIndex = lastTeamEntity.TeamCategory == 'D' ? lastTeamEntity.TeamIndex + 1 : lastTeamEntity.TeamIndex;
			nextEntity.TeamCategory = lastTeamEntity.TeamCategory == 'D' ? 'A' : ((char)((int)(lastTeamEntity.TeamCategory ?? 'A') + 1));
			nextEntity.Name = $"{lastTeamEntity.SitePrefix}-TW{nextEntity.TeamIndex}-{nextEntity.TeamCategory}".Trim(new char[] { '-', ' ' }).ToUpper();

			return nextEntity;
		}
	}
}
