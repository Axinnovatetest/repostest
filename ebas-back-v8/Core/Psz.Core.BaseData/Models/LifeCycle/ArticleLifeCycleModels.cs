using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.LifeCycle
{
	public class ArticleLifeCyclesModel
	{
		public int? ArticleId { get; set; }
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public string CreateUserName { get; set; }
		public int Id { get; set; }
		public int? PhaseId { get; set; }
		public string PhaseName { get; set; }
		public int? PhaseOrderInCycle { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public string UpdateUserName { get; set; }
		public ArticleLifeCyclesModel()
		{
			
		}
		public ArticleLifeCyclesModel(Infrastructure.Data.Entities.Tables.BSD.ArticleLifeCyclesEntity entiy)
		{
			ArticleId=entiy.ArticleId;
			CreateTime=entiy.CreateTime;
			CreateUserId=entiy.CreateUserId;
			CreateUserName=entiy.CreateUserName;
			Id=entiy.Id;
			PhaseId=entiy.PhaseId;
			PhaseName=entiy.PhaseName;
			PhaseOrderInCycle=entiy.PhaseOrderInCycle;
			UpdateTime=entiy.UpdateTime;
			UpdateUserId=entiy.UpdateUserId;
			UpdateUserName=entiy.UpdateUserName;
		}

		public Infrastructure.Data.Entities.Tables.BSD.ArticleLifeCyclesEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArticleLifeCyclesEntity
			{
				ArticleId = ArticleId,
				CreateTime = CreateTime,
				CreateUserId = CreateUserId,
				CreateUserName = CreateUserName,
				Id = Id,
				PhaseId = PhaseId,
				PhaseName = PhaseName,
				PhaseOrderInCycle = PhaseOrderInCycle,
				UpdateTime = UpdateTime,
				UpdateUserId = UpdateUserId,
				UpdateUserName = UpdateUserName,
			};
		}
	}
}
