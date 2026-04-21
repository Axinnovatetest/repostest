using Infrastructure.Data.Entities.Tables.BSD;
using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.BaseData.Models.LifeCycle;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ArticleLifeCycle
{
	public partial class ArticleLifeCycleService: IArticleLifeCycleService
	{
		public ResponseModel<List<ArticleLifeCyclePhasesModel>> GetLifeCyclesPhases(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<ArticleLifeCyclePhasesModel>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclePhasesAccess.Get();
				return ResponseModel<List<ArticleLifeCyclePhasesModel>>.SuccessResponse(
					entities?.Select(x => new ArticleLifeCyclePhasesModel(x)).ToList());
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> AddLifeCyclePhase(UserModel user, ArticleLifeCyclePhasesRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			var check= Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclePhasesAccess.GetByName(data.PhaseName);
			if(check != null && check.Count > 0)
				return ResponseModel<int>.FailureResponse("A life cycle phase with the same name already exsists .");

			try
			{
				var entity = new ArticleLifeCyclePhasesEntity
				{
					CreateTime = System.DateTime.Now,
					CreateUserId = user.Id,
					CreateUserName = user.Username,
					PhaseDescription = data.PhaseDescription,
					PhaseName = data.PhaseName,
				};
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclePhasesAccess.Insert(entity));
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> UpdateLifeCyclePhase(UserModel user, ArticleLifeCyclePhasesRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				var phase = Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclePhasesAccess.Get(data.Id);
				var model = new ArticleLifeCyclePhasesModel(phase);
				phase.PhaseName = data.PhaseName;
				phase.PhaseDescription = data.PhaseDescription;
				phase.UpdateTime = System.DateTime.Now;
				phase.UpdateUserId = user.Id;
				phase.UpdateUserName = user.Username;

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclePhasesAccess.Update(model.ToEntity()));
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> DeleteLifeCyclePhase(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var check=Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclesAccess.GetByLifeCycle(id);
			if(check != null && check.Count > 0)
				return ResponseModel<int>.FailureResponse($"Life cycle is affectd to [{check.GroupBy(x=>x.ArticleId).ToList().Count}] article(s), deletion is impossible .");

			try
			{
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclePhasesAccess.Delete(id));
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> CheckLifeCycleBeforeEditOrDelete(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var check = Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclesAccess.GetByLifeCycle(id);

			if(check != null && check.Count > 0)
				return ResponseModel<int>.SuccessResponse(check.GroupBy(x => x.ArticleId).ToList().Count);
			else
				return ResponseModel<int>.SuccessResponse(0);
		}
	}
}